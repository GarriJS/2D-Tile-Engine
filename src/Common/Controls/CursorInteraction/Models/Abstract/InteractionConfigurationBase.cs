using Engine.Physics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteraction.Models.Abstract
{
	/// <summary>
	/// Represents a interaction configuration.
	/// </summary>
	abstract public class InteractionConfigurationBase : IHaveASubArea
	{
		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public SubArea Area { get; set; }

		/// <summary>
		/// Gets or sets the offset;
		/// </summary>
		public Vector2 Offset { get; set; }
	}
}
