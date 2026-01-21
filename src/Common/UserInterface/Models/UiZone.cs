using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.LayoutInfo;
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
	public class UiZone : IAmDrawable, IHaveArea, IHaveAHoverCursor, ICanBeHovered<UiZone>, IDisposable
	{
		/// <summary>
		/// Gets or sets the user interface zone name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating if the user interface zone will recalculate the cached offsets on the next draw.
		/// </summary>
		public bool ResetCalculateCachedOffsets { get; set; }

		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the user interface zone vertical justification type. 
		/// </summary>
		public UiVerticalJustificationType VerticalJustificationType { get; set; }

		/// <summary>
		/// Gets the SimpleText.
		/// </summary>
		public IAmAGraphic Graphic { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.UserInterfaceScreenZone?.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get => this.UserInterfaceScreenZone?.Area; }

		/// <summary>
		/// Gets or sets the hover cursor.
		/// </summary>
		public Cursor HoverCursor { get; set; }

		/// <summary>
		/// Gets the base cursor configuration.
		/// </summary>
		public BaseCursorConfiguration BaseCursorConfiguration { get => this.CursorConfiguration; }

		/// <summary>
		/// Gets or sets the cursor configuration
		/// </summary>
		public CursorConfiguration<UiZone> CursorConfiguration { get; set; }

		/// <summary>
		/// Gets or sets the user interface screen zone.
		/// </summary>
		public UiScreenZone UserInterfaceScreenZone { get; set; }

		/// <summary>
		/// Gets or sets the user interface blocks.
		/// </summary>
		public List<UiBlock> Blocks { get; set; }

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseHoverEvent(CursorInteraction<UiZone> cursorInteraction)
		{
			this.CursorConfiguration?.RaiseHoverEvent(cursorInteraction);
		}

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			this.Graphic?.Draw(gameTime, gameServices, this.Position);

			if (true == this.ResetCalculateCachedOffsets)
			{
				this.UpdateZoneOffsets();
			}

			foreach (var block in this.Blocks ?? [])
			{
				block.Draw(gameTime, gameServices, this.Position);
			}
		}

		/// <summary>
		/// Updates the zone offsets.
		/// </summary>
		public void UpdateZoneOffsets()
		{
			foreach (var blockLayout in this.EnumerateLayout() ?? [])
			{
				blockLayout.Block.CachedOffset = blockLayout.Offset;
				blockLayout.Block.UpdateOffsets();
			}

			this.ResetCalculateCachedOffsets = false;
		}

		/// <summary>
		/// Enumerates the Block blockLayout.
		/// </summary>
		/// <returns>The enumerated Block blockLayout.</returns>
		public IEnumerable<BlockLayoutInfo> EnumerateLayout()
		{
			var contentHeight = this.Blocks.Sum(r => r.TotalHeight);
			var verticalOffset = this.VerticalJustificationType switch
			{
				UiVerticalJustificationType.Center => (this.Area.Height - contentHeight) / 2,
				UiVerticalJustificationType.Bottom => this.Area.Height - contentHeight,
				_ => 0
			};

			if (verticalOffset < 0)
				verticalOffset = 0;

			foreach (var block in this.Blocks ?? [])
			{
				var blockTop = verticalOffset + block.Margin.TopMargin;
				var result = new BlockLayoutInfo
				{
					Block = block,
					Offset = new Vector2
					{
						X = 0,
						Y = blockTop
					}
				};

				yield return result;

				verticalOffset += block.TotalHeight;
			}
		}

		/// <summary>
		/// Disposes of the user interface container.
		/// </summary>
		public void Dispose()
		{
			this.CursorConfiguration?.Dispose();

			foreach (var elementRow in this.Blocks ?? [])
			{
				elementRow?.Dispose();
			}
		}
	}
}
