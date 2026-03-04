using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents a text editing state.
	/// </summary>
	public class TextEditingState : IAmSubDrawable
	{
		/// <summary>
		/// Gets a value indicating whether the text editing state is highlighting text.
		/// </summary>
		public bool IsHighlighting { get => 0 != this.SelectionOffset; }

		/// <summary>
		/// Gets or sets the text editor position.
		/// </summary>
		required public int TextEditorPosition { get; set; }

		/// <summary>
		/// Gets or sets the text editor selection offset.
		/// </summary>
		required public int SelectionOffset { get; set; }

		/// <summary>
		/// Gets or sets the typing cursor color.
		/// </summary>
		required public Color TypingCursorColor { get; set; }

		/// <summary>
		/// Gets or sets the text highlighting color.
		/// </summary>
		required public Color TextHighlightColor { get; set; }

		/// <summary>
		/// Gets or sets the text editor coordinates.
		/// </summary>
		private Vector2 TextEditorCoordinates { get; set; }

		/// <summary>
		/// Gets or sets the highlighted rectangle.
		/// </summary>
		private Rectangle? HighlightedRectangle { get; set; }

		/// <summary>
		/// Gets or sets the typing cursor.
		/// </summary>
		required public TypingCursor TypingCursor { get; set; }

		/// <summary>
		/// Updates the text editor offsets.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		public void UpdateTextEditorOffsets(string text, SpriteFont font, Vector2 coordinates, Vector2 offset = default)
		{
			this.UpdateTextEditorOffset(text, font, coordinates, offset);
			this.UpdateHighlightedRectangle(text, font);
		}

		/// <summary>
		/// Updates the text editor offset.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		private void UpdateTextEditorOffset(string text, SpriteFont font, Vector2 coordinates, Vector2 offset = default)
		{
			if ((text.Length < this.TextEditorPosition) ||
				(0 > this.TextEditorPosition))
				return;

			var position = coordinates + offset;
			var textDimensions = font.MeasureString(text[this.TextEditorPosition..]);
			this.TextEditorCoordinates = new Vector2
			{
				X = position.X - textDimensions.X - 1,
				Y = position.Y
			};
		}

		/// <summary>
		/// Updates the highlighted text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		private void UpdateHighlightedRectangle(string text, SpriteFont font)
		{
			this.HighlightedRectangle = null;

			if (string.IsNullOrEmpty(text))
				return;

			if (false == this.IsHighlighting)
				return;

			(var start, var length) = this.GetHighlightedTextStartAndLength();
			var highlightedText = text.Substring(start, length); ;
			var highlightedDimensions = font.MeasureString(highlightedText);
			var horizontalPosition = 0 < this.SelectionOffset ?
				(int)this.TextEditorCoordinates.X :
				(int)(this.TextEditorCoordinates.X - highlightedDimensions.X);
			
			this.HighlightedRectangle = new Rectangle
			{
				X = horizontalPosition,
				Y = (int)this.TextEditorCoordinates.Y - 2,
				Width = (int)highlightedDimensions.X,
				Height = (int)highlightedDimensions.Y
			};
		}

		/// <summary>
		/// Gets the highlighted text start and length.
		/// </summary>
		/// <returns>The text start and length.</returns>
		public (int start, int length) GetHighlightedTextStartAndLength()
		{
			var start = Math.Min(this.TextEditorPosition, this.TextEditorPosition + this.SelectionOffset);
			var length = Math.Abs(this.SelectionOffset);

			if (0 > start)
				start = 0;

			if (0 > length)
				length = 0;

			return new(start, length);
		}

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
			if (true == this.IsHighlighting)
				this.DrawHighlightedTextIndicator(gameTime, gameServices);
			else
				this.DrawTextCursor(gameTime, gameServices);
		}

		/// <summary>
		/// Draws the text cursor.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		private void DrawTextCursor(GameTime gameTime, GameServiceContainer gameServices)
		{
			this.TypingCursor?.Draw(gameTime, gameServices, this.TextEditorCoordinates, this.TypingCursorColor);
		}

		/// <summary>
		/// Draws the highlighted text indicator.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		private void DrawHighlightedTextIndicator(GameTime gameTime, GameServiceContainer gameServices)
		{
			if (false == this.IsHighlighting)
				return;

			if (this.HighlightedRectangle is null)
				return;

			var drawingService = gameServices.GetService<IDrawingService>();
			drawingService.DrawRectangle(this.HighlightedRectangle.Value, this.TextHighlightColor);
		}
	}
}
