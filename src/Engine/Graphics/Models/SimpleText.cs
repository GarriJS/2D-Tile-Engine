using Engine.Controls.Typing.Models;
using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents simple text.
	/// </summary>
	public class SimpleText : IAmGraphicText, ICanBeSerialized<SimpleTextModel>
	{
		/// <summary>
		/// Gets or sets the max line character count.
		/// </summary>
		required public int? MaxLineCharacterCount { get; set; }

		/// <summary>
		/// Gets or sets the max line count.
		/// </summary>
		required public int? MaxLinesCount { get; set; }

		/// <summary>
		/// Gets or sets the font name.
		/// </summary>
		required public string FontName { get; set; }

		/// <summary>
		/// Gets or sets the text lines.
		/// </summary>
		required public List<TextLine> TextLines { get; set; }

		/// <summary>
		/// Gets or sets the text color
		/// </summary>
		required public Color TextColor { get; set; }

		/// <summary>
		/// Gets or sets the font.
		/// </summary>
		required public SpriteFont Font { get; set; }

		/// <summary>
		/// Gets the text dimensions.
		/// </summary>
		/// <param name="includeFontHeightWhenEmpty">value indicating whether the font height should be returned when the text contains no lines.</param>
		/// <returns>The text dimensions.</returns>
		virtual public Vector2 GetTextDimensions(bool includeFontHeightWhenEmpty = false)
		{
			var result = Vector2.Zero;

			foreach (var textLine in this.TextLines)
			{ 
				var lineDimensions = this.Font.MeasureString(textLine.Text);
				result.Y += lineDimensions.Y;

				if (result.X < lineDimensions.X)
					result.X = lineDimensions.X;
			}

			if ((true == includeFontHeightWhenEmpty) &&
				(0 >= result.Y))
			{
				var dummyMeasure = this.Font.MeasureString("A");
				result.Y = dummyMeasure.Y;
			}

			return result;
		}


		/// <summary>
		/// Gets the text dimensions.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="includeFontHeightWhenEmpty">value indicating whether the font height should be returned when the text contains no lines.</param>
		/// <returns>The text dimensions.</returns>
	    public Vector2 GetTextDimensions(string text, bool includeFontHeightWhenEmpty = false)
		{
			var result = this.Font.MeasureString(text);

			if ((true == includeFontHeightWhenEmpty) &&
				(0 >= result.Y))
			{
				var dummyMeasure = this.Font.MeasureString("A");
				result.Y = dummyMeasure.Y;
			}

			return result;
		}

		/// <summary>
		/// Gets the line offset height.
		/// </summary>
		/// <param name="lineNumber">The number.</param>
		/// <returns>The line offset height.</returns>
		public float GetLineOffsetHeight(int lineNumber)
		{
			float result = 0;

			for (int i = 0; i < lineNumber; i++)
			{
				var textLines = this.TextLines[i];
				var lineDimensions = this.GetTextDimensions(textLines.Text, includeFontHeightWhenEmpty: true);
				result += lineDimensions.Y;
			}

			return result;
		}

		/// <summary>
		/// Determines if the text needs conforming.
		/// </summary>
		/// <param name="maintainExistingLineBreaks">A value indicating whether to maintain existing line breaks.</param>
		/// <returns>A value indicating whether the text needs conforming.</returns>
		public bool NeedsConforming(string text, bool maintainExistingLineBreaks = false)
		{
			if ((false == this.MaxLineCharacterCount.HasValue) && 
				(false == this.MaxLinesCount.HasValue))
				return false;

			var maxChars = this.MaxLineCharacterCount ?? int.MaxValue;
			var maxLines = this.MaxLinesCount ?? int.MaxValue;
			var lineCount = 0;
			var sourceLines = true == maintainExistingLineBreaks
				? text.ReplaceLineEndings().Split(Environment.NewLine)
				: [string.Join(" ", text.ReplaceLineEndings().Split(Environment.NewLine))];

			foreach (var line in sourceLines)
			{
				var words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var currentLineLength = 0;

				foreach (var word in words)
				{
					var testLength = 0 == currentLineLength
						? word.Length
						: currentLineLength + 1 + word.Length;

					if (testLength <= maxChars)
						currentLineLength = testLength;
					else
					{
						lineCount++;

						if (lineCount >= maxLines)
							return true;

						currentLineLength = word.Length;

						if (currentLineLength > maxChars)
							return true;
					}
				}

				if (0 < currentLineLength)
				{
					lineCount++;

					if (lineCount > maxLines)
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Conforms the text to its line limits and characters per line limit.
		/// </summary>
		/// <param name="maintainExistingLineBreaks">A value indicating whether to maintain the existing line breaks in the text.</param>
		public void ConformTextToLimits(bool maintainExistingLineBreaks = false)
		{
			
		}

		/// <summary>
		/// Writes the sub writable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		virtual public void Write(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Vector2 offset = default)
		{
			if (0 == this.TextLines.Count)
				return;

			var writingService = gameServices.GetService<IWritingService>();
			var textOffset = coordinates + offset;

			foreach (var textLine in this.TextLines)
			{
				writingService.Draw(this.Font, textLine.Text, textOffset, this.TextColor);
				var lineDimensions = this.Font.MeasureString(textLine.Text);
				textOffset.Y += lineDimensions.Y;
			}
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		virtual public SimpleTextModel ToModel()
		{
			var result = new SimpleTextModel
			{
				//Text = this.Text,
				TextColor = this.TextColor,
				FontName = this.FontName,
			};

			return result;
		}
	}
}
