using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Drawables.Models;
using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Enums;
using Microsoft.Xna.Framework;

namespace Engine.UI.Models
{
    /// <summary>
    /// Represents a user interface row.
    /// </summary>
    public class UiRow : IAmSubDrawable, IDisposable
	{
		/// <summary>
		/// Gets or sets the user interface row name.
		/// </summary>
		public string UiRowName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user interface row should flex.
		/// </summary>
		public bool Flex { get; set; }

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public float Height { get; set; }

		/// <summary>
		/// Gets or sets the top padding.
		/// </summary>
		public float TopPadding { get; set; }

		/// <summary>
		/// Gets or sets the bottom padding.
		/// </summary>
		public float BottomPadding { get; set; }

		/// <summary>
		/// Gets or sets the user interface row horizontal justification type. 
		/// </summary>
		public UiRowHorizontalJustificationTypes HorizontalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the user interface row vertical justification type.
		/// </summary>
		public UiRowVerticalJustificationTypes VerticalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		public Image Image { get; set; }

		/// <summary>
		/// Gets or sets the sub elements.
		/// </summary>
		public List<IAmAUiElement> SubElements { get; set; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="position">The position.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();
			var spritebatch = drawingService.SpriteBatch;

			if (null != this.Image)
			{
				spritebatch.Draw(this.Image.Texture, position.Coordinates + new Vector2(0, offset.Y - this.TopPadding), this.Image.TextureBox, Color.White);
			}

			if (true != this?.SubElements.Any())
			{
				return;
			}

			var width = this.SubElements.Sum(e => e.Area.X + e.LeftPadding + e.RightPadding);
			var elementHorizontalOffset = this.HorizontalJustificationType switch
			{
				UiRowHorizontalJustificationTypes.None => 0,
				UiRowHorizontalJustificationTypes.Center => (this.Width - width) / 2,
				UiRowHorizontalJustificationTypes.Left => 0,
				UiRowHorizontalJustificationTypes.Right => this.Width - width,
				_ => 0,
			};

			if (0 > elementHorizontalOffset)
			{
				elementHorizontalOffset = 0;
			}

			var largestHeight = this.SubElements.OrderByDescending(e => e.Area.Y)
												 .FirstOrDefault().Area.Y;

			foreach (var element in this.SubElements)
			{
				var verticallyCenterOffset = 0f;

				switch (this.VerticalJustificationType)
				{
					case UiRowVerticalJustificationTypes.Bottom:
						verticallyCenterOffset = (largestHeight - element.Area.Y);
						break;
					case UiRowVerticalJustificationTypes.Center:
						verticallyCenterOffset = (largestHeight - element.Area.Y) / 2;
						break;
					case UiRowVerticalJustificationTypes.None:
					case UiRowVerticalJustificationTypes.Top:
						break;
				}

				switch (this.HorizontalJustificationType)
				{
					case UiRowHorizontalJustificationTypes.Right:
					case UiRowHorizontalJustificationTypes.Center:
					case UiRowHorizontalJustificationTypes.None:
					case UiRowHorizontalJustificationTypes.Left:
					default:
						elementHorizontalOffset += element.LeftPadding;
						var elementOffset = new Vector2(elementHorizontalOffset, offset.Y + verticallyCenterOffset);
						element.Draw(gameTime, gameServices, position, elementOffset);
						elementHorizontalOffset += (element.RightPadding + element.Area.X);
						break;
				}
			}
		}

		/// <summary>
		/// Disposes of the user interface row.
		/// </summary>
		public void Dispose()
		{
			if (true != this.SubElements?.Any())
			{ 
				return;
			}

			foreach (var subElement in this.SubElements)
			{ 
				subElement?.Dispose();
			}
		}
	}
}
