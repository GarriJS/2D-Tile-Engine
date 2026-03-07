using Engine.Physics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents a text cursor.
	/// </summary>
	sealed public class TextCursor : IAmSubDrawable, IHaveASubArea
	{
		/// <summary>
		/// Get or sets a value indicating whether the text cursor should blink.
		/// </summary>
		required public bool Blink { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the text cursor is visible.
		/// </summary>
		required public bool IsVisible { get; set; }

		/// <summary>
		/// Gets or sets the current blink time in milliseconds.
		/// </summary>
		required public double ElaspedFrameDuration { get; set; }

		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		required public Color Color { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		required public TextPosition Position { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		required public SubArea Area { get; set; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			if (this.Blink)
			{
				this.ElaspedFrameDuration += gameTime.ElapsedGameTime.TotalMilliseconds;

				if (this.ElaspedFrameDuration >= 500) // blink time is always 1/2 a second
				{
					this.IsVisible = !this.IsVisible;
					this.ElaspedFrameDuration = 0;
				}
			}
			else if (false == this.IsVisible)
				this.IsVisible = true;

			if (false == this.IsVisible)
				return;

			var drawingService = gameServices.GetService<IDrawingService>();
			drawingService.DrawRectangle(this.Area.ToRectangle(coordinates + offset), color);
		}
	}
}
