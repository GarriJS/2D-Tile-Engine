using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Enums;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user interface zone.
	/// </summary>
	public class UiZone : IAmDrawable, IHaveAnImage, IHaveArea, ICanBeHovered<UiZone>, IDisposable
	{
		/// <summary>
		/// Gets or sets the user interface zone name.
		/// </summary>
		public string UiZoneName { get; set; }

		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the user interface row justification type. 
		/// </summary>
		public UiZoneJustificationTypes JustificationType { get; set; }

		/// <summary>
		/// Gets the graphic.
		/// </summary>
		public IAmAGraphic Graphic { get => this.Image; }

		/// <summary>
		/// Gets the image.
		/// </summary>
		public Image Image { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.UserInterfaceScreenZone?.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get => this.UserInterfaceScreenZone?.Area; }

		/// <summary>
		/// Gets the base hover configuration.
		/// </summary>
		public BaseHoverConfiguration BaseHoverConfig { get => this.HoverConfig; }

		/// <summary>
		/// Gets or sets the hover configuration.
		/// </summary>
		public HoverConfiguration<UiZone> HoverConfig { get; set; }

		/// <summary>
		/// Gets or sets the user interface screen zone.
		/// </summary>
		public UiScreenZone UserInterfaceScreenZone { get; set; }

		/// <summary>
		/// Gets or sets the element rows.
		/// </summary>
		public List<UiRow> ElementRows { get; set; }

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaiseHoverEvent(Vector2 elementLocation)
		{
			this.HoverConfig?.RaiseHoverEvent(this, elementLocation);
		}

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			var drawingService = gameServices.GetService<IDrawingService>();
			var spritebatch = drawingService.SpriteBatch;

			if (null != this.Image)
			{
				spritebatch.Draw(this.Image.Texture, this.Position.Coordinates, this.Image.TextureBox, Color.White);
			}

			if (0 == this.ElementRows.Count)
			{
				return;
			}

			var height = this.ElementRows.Sum(e => e.InsideHeight + e.BottomPadding + e.TopPadding);
			var rowVerticalOffset = this.JustificationType switch
			{
				UiZoneJustificationTypes.None => 0,
				UiZoneJustificationTypes.Center => (this.Area.Height - height) / 2,
				UiZoneJustificationTypes.Top => 0,
				UiZoneJustificationTypes.Bottom => this.Area.Height - height,
				_ => 0,
			};

			if (0 > rowVerticalOffset)
			{
				rowVerticalOffset = 0;
			}

			foreach (var elementRow in this.ElementRows)
			{
				rowVerticalOffset += elementRow.TopPadding;
				var offset = new Vector2
				{
					X = 0,
					Y = rowVerticalOffset
				};
				elementRow.Draw(gameTime, gameServices, this.Position, offset);
				rowVerticalOffset += (elementRow.BottomPadding + elementRow.InsideHeight);
			}
		}

		/// <summary>
		/// Updates the zone rows offset.
		/// </summary>
		private void UpdateZoneRowsOffset()
		{
			if (0 == this.ElementRows.Count)
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
		}

		/// <summary>
		/// Disposes of the user interface container.
		/// </summary>
		public void Dispose()
		{
			this.HoverConfig?.Dispose();

			if (0 == this.ElementRows.Count)
			{
				return;
			}

			foreach (var elementRow in this.ElementRows)
			{
				elementRow?.Dispose();
			}
		}
	}
}
