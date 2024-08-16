using System;

namespace Engine.Terminal.Commands.Models
{
	/// <summary>
	/// Represents a command argument definition.
	/// </summary>
	public class CommandArgumentDefinition
	{
		/// <summary>
		/// Gets or sets the argument order.
		/// </summary>
		public int ArgumentOrder { get; set; }

		/// <summary>
		/// Gets or sets the argument name.
		/// </summary>
		public string ArgumentName { get; set; }

		/// <summary>
		/// Gets or sets the argument type.
		/// </summary>
		public Type ArgumentType { get; set; }
	}
}
