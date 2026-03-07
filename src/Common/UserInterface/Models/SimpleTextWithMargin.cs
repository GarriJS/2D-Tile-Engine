using Common.DiskModels.UserInterface;
using Engine.Graphics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents simple text with margin.
	/// </summary>
	sealed public class SimpleTextWithMargin : SimpleText
	{
		/// <summary>
		/// Gets or sets the user interface margin.
		/// </summary>
		required public UiMargin Margin { get; set; }

		/// <summary>
		/// Gets the text dimensions.
		/// </summary>
		/// <param name="includeFontHeightWhenEmpty">value indicating whether the font height should be returned when the text contains no lines.</param>
		/// <returns>The text dimensions.</returns>
		override public Vector2 GetTextDimensions(bool includeFontHeightWhenEmpty = false)
		{
			var result = Vector2.Zero;

			foreach (var textLine in this.TextLines)
			{
				var lineDimensions = this.Font.MeasureString(textLine);
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

			result.X += this.Margin.LeftMargin + this.Margin.RightMargin;
			result.Y += this.Margin.TopMargin + this.Margin.BottomMargin;

			return result;
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		override public void Write(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Vector2 offset = default)
		{
			if (0 == this.TextLines.Count)
				return;

			var writingService = gameServices.GetService<IWritingService>();
			var textOffset = coordinates + offset + new Vector2
			{
				X = this.Margin.LeftMargin,
				Y = this.Margin.TopMargin
			};

			foreach (var textLine in this.TextLines)
			{
				writingService.Draw(this.Font, textLine, textOffset, this.TextColor);
				var lineDimensions = this.Font.MeasureString(textLine);
				textOffset.Y += lineDimensions.Y;
			}
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		new public SinmpleTextWithMarginModel ToModel()
		{
			var result = new SinmpleTextWithMarginModel
			{
				//Text = this.Text,
				TextColor = this.TextColor,
				FontName = this.FontName,
				Margin = this.Margin.ToModel()
			};

			return result;
		}
	}
}
