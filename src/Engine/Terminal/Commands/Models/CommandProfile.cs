using System.Collections.Generic;

namespace Engine.Terminal.Commands.Models
{
	/// <summary>
	/// Represents a command profile.
	/// </summary>
	public class CommandProfile
	{
		/// <summary>
		/// Gets or sets the domain.
		/// </summary>
		public string Domain { get; set; }

		/// <summary>
		/// Gets or sets the commands.
		/// </summary>
		public string[] Commands { get; set; }

		/// <summary>
		/// Gets or sets the parameters.
		/// </summary>
		public Dictionary<string, List<CommandArgumentDefinition>> Parameters { get; set; }
	}
}
