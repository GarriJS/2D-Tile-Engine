using Engine.Drawing.Models;

namespace Engine.Terminal.Models
{
	/// <summary>
	/// Represents a console line.
	/// </summary>
	public class ConsoleLine
	{
		/// <summary>
		/// Gets or sets the command.
		/// </summary>
		public DrawableText Command { get; set; }
		
		/// <summary>
		/// Gets or sets the response.
		/// </summary>
		public DrawableText Response { get; set; }
	}
}
