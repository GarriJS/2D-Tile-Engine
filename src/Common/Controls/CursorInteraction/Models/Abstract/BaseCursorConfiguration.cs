using Engine.Physics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteraction.Models.Abstract
{
	/// <summary>
	/// Represents a base cursor configuration.
	/// </summary>
	abstract public class BaseCursorConfiguration : IHaveASubArea
	{
		/// Gets or sets the offset;
		/// </summary>
		public Vector2 Offset { get; set; }

		/// <summary>
		/// Gets or sets the click offset.
		/// </summary>
		public Vector2 ClickOffset { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public SubArea Area { get; set; }

		/// <summary>
		/// Gets or sets the click area.
		/// </summary>
		public SubArea ClickArea { get; set; }
	}
}
