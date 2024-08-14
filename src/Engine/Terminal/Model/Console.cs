using Engine.Drawing.Models;

namespace Engine.Terminal.Model
{
	/// <summary>
	/// Represents a console.
	/// </summary>
	public class Console
	{
		/// <summary>
		/// Gets or sets the console back ground.
		/// </summary>
		public Image ConsoleBackGround { get; set; }

		/// <summary>
		/// Gets or sets the console text area.
		/// </summary>
		public Image ConsoleTextArea { get; set; }
	}
}
