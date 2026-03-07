using Microsoft.Xna.Framework;

namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents a text highlighting state.
	/// </summary>
	public struct TextHighlightingState
	{
		/// <summary>
		/// Gets a value indicating whether the text highlighting state is highlighting text.
		/// </summary>
		readonly public bool IsHighlighting => this.TextAnchor is not null;

		/// <summary>
		/// Gets or sets the text anchor.
		/// </summary>
		required public TextPosition? TextAnchor { get; set; }

		/// <summary>
		/// Gets or sets the text highlighting color.
		/// </summary>
		required public Color TextHighlightColor { get; set; }

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
			//if (true == this.IsHighlighting)
			//	this.DrawHighlightedTextIndicator(gameTime, gameServices);
			//else
			//	this.DrawTextCursor(gameTime, gameServices);
		}
	}
}
