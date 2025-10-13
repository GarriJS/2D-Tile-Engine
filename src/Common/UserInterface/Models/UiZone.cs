using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Enums;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
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
			this.Image?.Draw(gameTime, gameServices, this.Position);

			if (0 == this.ElementRows.Count)
			{
				return;
			}

			if (true == this.ElementRows.Any(e => false == e.CachedElementOffset.HasValue))
			{ 
				this.UpdateZoneRowsOffset();
			}

			foreach (var elementRow in this.ElementRows)
			{
				elementRow.Draw(gameTime, gameServices, this.Position, elementRow.CachedElementOffset.Value);
			}
		}

		/// <summary>
		/// Updates the zone rows rowOffset.
		/// </summary>
		private void UpdateZoneRowsOffset()
		{
			if (0 == this.ElementRows.Count)
			{
				return;
			}

			var height = this.ElementRows.Sum(e => e.InsideHeight + e.InsidePadding.BottomPadding + e.InsidePadding.TopPadding);
			var rowVerticalJustificationOffset = this.JustificationType switch
			{
				UiZoneJustificationTypes.None => 0,
				UiZoneJustificationTypes.Center => (this.Area.Height - height) / 2,
				UiZoneJustificationTypes.Top => 0,
				UiZoneJustificationTypes.Bottom => this.Area.Height - height,
				_ => 0,
			};

			if (0 > rowVerticalJustificationOffset)
			{
				rowVerticalJustificationOffset = 0;
			}

			var rowOffset = new Vector2
			{
				X = 0,
				Y = rowVerticalJustificationOffset
			};

			foreach (var elementRow in this.ElementRows)
			{
				if (UiZoneJustificationTypes.Center == this.JustificationType)
				{ 
					//will need to do something additional here?
				}

				elementRow.CachedElementOffset = rowOffset;
				rowOffset.Y += elementRow.InsideHeight;
			}
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
