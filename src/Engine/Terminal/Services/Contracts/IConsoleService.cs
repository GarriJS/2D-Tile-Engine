namespace Engine.Terminal.Services.Contracts
{
	/// <summary>
	/// Represents a console service.
	/// </summary>
	public interface IConsoleService
	{
		/// <summary>
		/// Toggles the console.
		/// </summary>
		public void ToggleConsole();

		/// <summary>
		/// Starts the console.
		/// </summary>
		public void StartConsole();

		/// <summary>
		/// Stops the console.
		/// </summary>
		public void StopConsole();
	}
}
