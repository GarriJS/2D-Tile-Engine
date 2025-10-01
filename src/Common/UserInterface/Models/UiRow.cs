using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user interface row.
	/// </summary>
	public class UiRow : IAmSubDrawable, IHaveASubArea, ICanBeHovered<UiRow>, IDisposable
	{
		/// <summary>
		/// Gets or sets the cached element offset.
		/// </summary>
		private Vector2? CachedRowOffset { get; set; }

		/// <summary>
		/// Gets or sets the user interface row name.
		/// </summary>
		public string UiRowName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user interface row should flex.
		/// </summary>
		public bool Flex { get; set; }

		/// <summary>
		/// Get the width.
		/// </summary>
		public float InsideWidth { get => this.InsidePadding.LeftPadding + this.SubElements.Sum(e => e.InsideWidth) + this.InsidePadding.RightPadding; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		public float InsideHeight { get => this.InsidePadding.TopPadding + this.SubElements.Max(e => e.InsideHeight) + this.InsidePadding.BottomPadding; }

		/// <summary>
		/// Gets or sets the inside user interface padding. 
		/// </summary>
		public UiPadding InsidePadding { get; set; }

		/// <summary>
		/// Gets or sets the user interface row horizontal justification type. 
		/// </summary>
		public UiRowHorizontalJustificationTypes HorizontalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the user interface row vertical justification type.
		/// </summary>
		public UiRowVerticalJustificationTypes VerticalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the cached element offset.
		/// </summary>
		public Vector2? CachedElementOffset { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public Vector2 Area { get; set; }

		/// <summary>
		/// Gets the image.
		/// </summary>
		public Image Image { get; set; }

		/// <summary>
		/// Gets the base hover configuration.
		/// </summary>
		public BaseHoverConfiguration BaseHoverConfig { get => this.HoverConfig; }

		/// <summary>
		/// Gets or sets the hover configuration.
		/// </summary>
		public HoverConfiguration<UiRow> HoverConfig { get; set; }

		/// <summary>
		/// Gets or sets the sub elements.
		/// </summary>
		public List<IAmAUiElement> SubElements { get; set; }

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaiseHoverEvent(Vector2 elementLocation)
		{
			this.HoverConfig?.RaiseHoverEvent(this, elementLocation);
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="position">The position.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			this.Image?.Draw(gameTime, gameServices, position, new Vector2(offset.X, offset.Y + this.InsidePadding.TopPadding));

			if (true == this.SubElements.Any(e => false == e.CachedElementOffset.HasValue))
			{ 
				this.UpdateRowElementsOffset();
			}

			foreach (var element in this.SubElements)
			{
				element.Draw(gameTime, gameServices, position, offset + element.CachedElementOffset.Value);
			}
		}

		/// <summary>
		/// Updates the rows elements offset.
		/// </summary>
		private void UpdateRowElementsOffset()
		{
			if (0 == this.SubElements.Count)
			{
				return;
			}

			var rowHorizontalJustificationOffset = this.HorizontalJustificationType switch
			{
				UiRowHorizontalJustificationTypes.Center => (this.Area.X - this.InsideWidth) / 2,
				UiRowHorizontalJustificationTypes.Right => this.Area.X - this.InsideWidth,
				_ => 0
			};
			var rowVerticalJustificationOffset = this.VerticalJustificationType switch
			{
				UiRowVerticalJustificationTypes.Center => (this.Area.Y - this.InsideHeight) / 2,
				UiRowVerticalJustificationTypes.Top => this.Area.Y - this.InsideHeight,
				_ => 0
			};
			var elementOffset = new Vector2
			{
				X = rowHorizontalJustificationOffset,
				Y = rowVerticalJustificationOffset
			};

			foreach (var element in this.SubElements)
			{
				if (UiRowVerticalJustificationTypes.Center == this.VerticalJustificationType)
				{ 
					//will need to do something additional here
				}

				element.CachedElementOffset = elementOffset;
				elementOffset.X += element.InsideWidth;
			}
		}

		/// <summary>
		/// Disposes of the user interface row.
		/// </summary>
		public void Dispose()
		{
			if (0 == this.SubElements.Count)
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
