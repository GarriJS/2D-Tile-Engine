using Common.DiskModels.UserInterface;
using Engine.Graphics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents graphical text with margin.
	/// </summary>
	sealed public class GraphicalTextWithMargin : SimpleText
	{
		/// <summary>
		/// Gets or sets the user interface margin.
		/// </summary>
		required public UiMargin Margin { get; set; }

		/// <summary>
		/// Gets the text dimensions.
		/// </summary>
		/// <returns>The text dimensions.</returns>
		override public Vector2 GetTextDimensions()
		{
			var result = this.Font.MeasureString(this.Text);
			result.X = result.X + this.Margin.LeftMargin + this.Margin.RightMargin;
			result.Y = result.Y + this.Margin.TopMargin + this.Margin.BottomMargin;

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
			if (true == string.IsNullOrEmpty(this.Text))
				return;

			var writingService = gameServices.GetService<IWritingService>();
			var contentOffset = coordinates + offset + new Vector2
			{
				X = this.Margin.LeftMargin,
				Y = this.Margin.TopMargin
			};
			writingService.Draw(this.Font, this.Text, contentOffset, this.TextColor);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		override public GraphicalTextWithMarginModel ToModel()
		{
			var result = new GraphicalTextWithMarginModel
			{
				Text = this.Text,
				TextColor = this.TextColor,
				FontName = this.FontName,
				//Margin = 
			};

			return result;
		}
	}
}
