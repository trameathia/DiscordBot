using System;

namespace DiscordBot
{
    /// <summary>
    /// A class that implements the singleton pattern. Can be used one of two ways:
    /// By referencing <see cref="Singleton{T}.Instance"/> OR
    /// By inheriting from <see cref="Singleton{T}"/> for situations where the object's lifecycle must be managed separately
    /// </summary>
    /// <typeparam name="T">The underlying object type of the singleton instance</typeparam>
    public class Singleton<T>
		where T : new()
	{
		/// <summary>
		/// A boolean value indicating if a reference to the singleton object has been created
		/// </summary>
		private static bool _IsReferenceCreated;
		/// <summary>
		/// The single, underlying instance of the type <typeparamref name="T"/>
		/// </summary>
		private static Lazy<T> _Instance;

		/// <summary>
		/// Returns the single, underlying instance of the type
		/// </summary>
		public static T Instance { get { return _Instance.Value; } }

		/// <summary>
		/// Static constructor defined so that the compiler does not mark the type with "BeforeFieldInit"
		/// </summary>
		static Singleton()
		{
			_Instance = new Lazy<T>();
		}

		/// <summary>
		/// Instantiates a new instance of the <see cref="Singleton{T}"/> class
		/// </summary>
		protected Singleton()
		{
			// Ensure that the pattern has not been violated
			if (_Instance.IsValueCreated || _IsReferenceCreated)
				throw new ApplicationException("Fatal Error: Singleton pattern violated!");

			// Save the underlying instance
			_Instance = new Lazy<T>(() => (T)((object)this));
			_IsReferenceCreated = true;
		}
	}
}
