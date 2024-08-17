using Engine.Terminal.Models;

namespace Engine.Terminal.Services.Contracts
{
	/// <summary>
	/// Represents a console service.
	/// </summary>
	public interface IConsoleService
	{
		/// <summary>
		/// Gets or sets the console.
		/// </summary>
		public Console Console { get; }

		/// <summary>
		/// Gets or sets a value indicating whether the console is active.
		/// </summary>
		public bool ConsoleActive { get; }

		/// <summary>
		/// Toggles the console.
		/// </summary>
		public void ToggleConsole();
	}
}
