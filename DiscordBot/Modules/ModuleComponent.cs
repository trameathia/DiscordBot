using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
	public abstract class ModuleComponent<TConfiguration, TResult> : IDisposable
		where TConfiguration : ModuleComponentConfiguration
	{
		private Timer _Timer;
		private int _Ticks;

		public TConfiguration Configuration { get; }
		public bool IsRunning { get; private set; }
		protected int TickCount => _Ticks;

		protected ModuleComponent(TConfiguration configuration)
		{
			Configuration = configuration;

			if (Configuration.StartOnLoad)
			{
				Start();
			}
		}

		private void InternalOnTick(object state)
		{
			if (Configuration.TimerInterval == Timeout.InfiniteTimeSpan)
			{
				End(state);
			}
			else
			{
				++_Ticks;

				if (Configuration.TimerTicksMax > 0 && _Ticks >= Configuration.TimerTicksMax)
				{
					End(state);
				}
				else
				{
					OnTick();
				}
			}
		}

		private void End(object state)
		{
			IsRunning = false;

			if (_Timer != null)
			{
				_Timer.Dispose();
				_Timer = null;
			}

			OnEnd();

			if (state is Func<TResult, Task> onEnd)
			{
				onEnd.Invoke(BuildResult());
			}
		}

		protected virtual TResult BuildResult() => default;

		protected virtual void OnStart()
		{
		}

		protected virtual void OnTick()
		{
		}

		protected virtual void OnEnd()
		{
		}

		public bool Start(Func<TResult, Task> onEnd = null)
		{
			if (IsRunning)
			{
				return false;
			}

			IsRunning = true;
			_Ticks = 0;

			OnStart();

			if (Configuration.TimerDelay != TimeSpan.Zero || Configuration.TimerInterval != Timeout.InfiniteTimeSpan)
			{
				_Timer = new Timer(InternalOnTick, onEnd, Configuration.TimerDelay, Configuration.TimerInterval);
			}

			return true;
		}

		public void Dispose()
		{
			if (_Timer != null)
			{
				_Timer.Dispose();
				_Timer = null;
			}
		}
	}
}