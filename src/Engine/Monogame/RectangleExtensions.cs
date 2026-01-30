using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Monogame
{
	/// <summary>
	/// Provides extensions to the Monogame rectangle.
	/// </summary>
	static public class RectangleExtensions
	{
		/// <summary>
		/// Returns four rectangles representing the edges of the rectangle.
		/// </summary>
		/// <param name="rect">The initial rectangle.</param>
		/// <param name="thickness">The thickness of the edge rectangles.</param>
		static public IEnumerable<Rectangle> GetEdgeRectangles(this Rectangle rect, int thickness)
		{
			yield return new Rectangle(rect.X, rect.Y, rect.Width, thickness); // Top
			yield return new Rectangle(rect.X, rect.Bottom - thickness, rect.Width, thickness); // Bottom
			yield return new Rectangle(rect.X, rect.Y, thickness, rect.Height); // Left
			yield return new Rectangle(rect.Right - thickness, rect.Y, thickness, rect.Height); // Right
		}
	}
}
