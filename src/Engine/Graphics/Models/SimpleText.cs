using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
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
	public class SimpleText : IAmGraphicText, ICanBeSerialized<GraphicalTextModel>
	{
		/// <summary>
		/// Gets or sets the max line width.
		/// </summary>
		public float? MaxLineWidth { get; set; }

		/// <summary>
		/// Gets or sets the font name.
		/// </summary>
		public string FontName { get; set; }

		/// <summary>
		/// Gets the text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the text color
		/// </summary>
		public Color TextColor { get; set; }

		/// <summary>
		/// Gets or sets the font.
		/// </summary>
		public SpriteFont Font { get; set; }

		/// <summary>
		/// Gets the text dimensions.
		/// </summary>
		/// <returns>The text dimensions.</returns>
		virtual public Vector2 GetTextDimensions()
		{
			var result = this.Font.MeasureString(this.Text);

			return result;
		}

		/// <summary>
		/// Conforms the text to the max width.
		/// </summary>
		/// <param name="maintainExistingLineBreaks">A value indicating whether to maintain the existing line breaks in the text.</param>
		public void ConformTextToMaxWidth(bool maintainExistingLineBreaks = false)
		{
			var finalLines = new List<string>();
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
					var testLine = currentLine.Length == 0
						? word
						: currentLine + " " + word;
					var testDimensions = this.Font.MeasureString(testLine);

					if (testDimensions.X <= this.MaxLineWidth)
					{
						currentLine.Clear();
						currentLine.Append(testLine);
					}
					else
					{
						if (currentLine.Length > 0)
							finalLines.Add(currentLine.ToString());

						currentLine.Clear();
						currentLine.Append(word);
					}
				}

				if (currentLine.Length > 0)
					finalLines.Add(currentLine.ToString());
			}

			this.Text = string.Join(Environment.NewLine, finalLines);
		}

		/// <summary>
		/// Writes the sub writable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		virtual public void Write(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			if (true == string.IsNullOrEmpty(this.Text))
				return;
		
			var writingService = gameServices.GetService<IWritingService>();
			var textOffset = position.Coordinates + offset;
			writingService.Draw(this.Font, this.Text, textOffset, this.TextColor);
		}


		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		virtual public GraphicalTextModel ToModel()
		{
			var result = new GraphicalTextModel
			{
				Text = this.Text,
				TextColor = this.TextColor,
				FontName = this.FontName,
			};

			return result;
		}
	}
}
