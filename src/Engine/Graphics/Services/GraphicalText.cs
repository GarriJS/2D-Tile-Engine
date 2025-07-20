using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics.Services
{
	/// <summary>
	/// Represents graphical text.
	/// </summary>
	public class GraphicalText : IAmGraphicalText
	{
		/// <summary>
		/// Gets the text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets the font.
		/// </summary>
		public SpriteFont Font { get; set; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var writingService = gameServices.GetService<IWritingService>();

			if (false == string.IsNullOrEmpty(this.Text))
			{
				var textMeasurements = writingService.MeasureString("Monobold", this.Text);
				writingService.Draw("Monobold", this.Text, position.Coordinates, Color.Maroon);
			}
		}
	}
}
