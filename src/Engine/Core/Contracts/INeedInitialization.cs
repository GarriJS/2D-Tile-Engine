namespace Engine.Core.Contracts
{
	/// <summary>
	/// Represents something that need initialization.
	/// </summary>
	public interface INeedInitialization
	{
		/// <summary>
		/// Performs initializes.
		/// </summary>
		public void Initialize();
	}
}
