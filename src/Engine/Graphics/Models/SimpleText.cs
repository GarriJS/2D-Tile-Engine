using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

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
		/// Gets the text.
		/// </summary>
		required public string Text { get; set; }

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
		/// <param name="alwaysGetFontHeight">A value indicating whether to always get the font height.</param>
		/// <returns>The text dimensions.</returns>
		virtual public Vector2 GetTextDimensions(bool alwaysGetFontHeight = false)
		{
			var result = this.Font.MeasureString(this.Text);

			if (alwaysGetFontHeight &&
				result.Y <= 0)
			{
				var simpleMeasure = this.Font.MeasureString("a");
				result.Y = simpleMeasure.Y;
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
			var sourceText = text.ReplaceLineEndings();
			var paragraphs = maintainExistingLineBreaks
				? sourceText.Split(Environment.NewLine)
				: [sourceText.Replace(Environment.NewLine, " ")];

			foreach (var paragraph in paragraphs)
			{
				var words = paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var currentLineLength = 0;

				foreach (var word in words)
				{
					var testLength = currentLineLength == 0
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

				if (currentLineLength > 0)
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
			if ((false == this.MaxLineCharacterCount.HasValue) && 
				(false == MaxLinesCount.HasValue))
				return;

			var maxChars = this.MaxLineCharacterCount ?? int.MaxValue;
			var maxLines = this.MaxLinesCount ?? int.MaxValue;
			List<string> finalLines = [];
			var sourceText = this.Text.ReplaceLineEndings();
			var paragraphs = maintainExistingLineBreaks
				? sourceText.Split(Environment.NewLine)
				: [sourceText.Replace(Environment.NewLine, " ")];

			foreach (var paragraph in paragraphs)
			{
				var words = paragraph.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				var currentLine = new StringBuilder();

				foreach (var word in words)
				{
					var testLength = currentLine.Length == 0
						? word.Length
						: currentLine.Length + 1 + word.Length;

					if (testLength <= maxChars)
					{
						if (0 < currentLine.Length)
							currentLine.Append(' ');

						currentLine.Append(word);
					}
					else
					{
						if (0 < currentLine.Length)
						{
							finalLines.Add(currentLine.ToString());

							if (finalLines.Count >= maxLines)
							{
								this.Text = string.Join(Environment.NewLine, finalLines);
								
								return;
							}
						}

						currentLine.Clear();
						currentLine.Append(word);
					}
				}

				if (currentLine.Length > 0)
				{
					finalLines.Add(currentLine.ToString());

					if (finalLines.Count >= maxLines)
					{
						this.Text = string.Join(Environment.NewLine, finalLines);
						return;
					}
				}
			}

			this.Text = string.Join(Environment.NewLine, finalLines);
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
			if (true == string.IsNullOrEmpty(this.Text))
				return;
		
			var writingService = gameServices.GetService<IWritingService>();
			var textOffset = coordinates + offset;
			writingService.Draw(this.Font, this.Text, textOffset, this.TextColor);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		virtual public SimpleTextModel ToModel()
		{
			var result = new SimpleTextModel
			{
				Text = this.Text,
				TextColor = this.TextColor,
				FontName = this.FontName,
			};

			return result;
		}
	}
}
