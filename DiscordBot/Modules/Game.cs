using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
	public abstract class Game<TConfiguration, TResult> : IDisposable
		where TConfiguration : GameConfiguration
	{
		private Timer _Timer;
		private int _Ticks;

		public TConfiguration Configuration { get; }
		public bool IsRunning { get; private set; }

		protected Game(TConfiguration configuration)
		{
			Configuration = configuration;
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

				if (_Ticks >= Configuration.TimerTicksMax)
				{
					End(state);
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

		protected abstract TResult BuildResult();

		protected abstract void OnStart();

		protected virtual void OnTick()
		{
		}

		protected virtual void OnEnd()
		{
		}

		public bool Start(Func<TResult, Task> onEnd)
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