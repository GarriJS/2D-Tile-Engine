using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics.Models
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
		/// Gets or sets the text color
		/// </summary>
		public Color TextColor { get; set; }

		/// <summary>
		/// Gets the font.
		/// </summary>
		public SpriteFont Font { get; set; }

		/// <summary>
		/// Gets the text dimensions.
		/// </summary>
		/// <returns>The text dimensions.</returns>
		public Vector2 GetTextDimensions()
		{
			return this.Font.MeasureString(this.Text);
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			if (true == string.IsNullOrEmpty(this.Text))
			{
				return;
			}

			var writingService = gameServices.GetService<IWritingService>();

			writingService.Draw(this.Font, this.Text, position.Coordinates + offset, this.TextColor);
		}
	}
}
