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
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user interface zone.
	/// </summary>
	public class UiZone : IAmDrawable, IRequirePreRender, IHaveArea, IHaveAHoverCursor, ICanBeHovered<UiZone>, IDisposable
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
			var drawingService = gameServices.GetService<IDrawingService>();

			if (this.Name == "Level Editor Label Row")
			{
				drawingService.SpriteBatch.Draw(this._scrollRenderTarget, this.Position.Coordinates, Color.White);
			}
			else
				this.DrawContents(gameTime, gameServices, this.Position.Coordinates, Color.White);
		}

		/// <summary>
		/// Draws the contents.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="scrollOffset">The scroll offset.</param>
		private void DrawContents(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 scrollOffset = default)
		{
			this.Graphic?.Draw(gameTime, gameServices, coordinates, color, scrollOffset);

			if (true == this.ResetCalculateCachedOffsets)
				this.UpdateZoneOffsets();

			foreach (var block in this.Blocks ?? [])
				block.Draw(gameTime, gameServices, coordinates, color, scrollOffset);
		}

		private RenderTarget2D _scrollRenderTarget;
		public bool IsScrollable { get; set; }
		public int ScrollOffsetY { get; private set; } = -10;
		public int ScrollSpeed { get; set; } = 30;
		public int MaxVisibleHeight { get; set; } = 20;// set this to your limit
		public bool ShouldPreRender()
		{
			return this.Name == "Level Editor Label Row";
		}

		public void PreRender(GameTime gameTime, GameServiceContainer gameServices)
		{
			var graphicDeviceService = gameServices.GetService<IGraphicsDeviceService>();
			var device = graphicDeviceService.GraphicsDevice;
			var drawingService = gameServices.GetService<IDrawingService>();

			// Create RT if needed
			if (_scrollRenderTarget == null ||
				_scrollRenderTarget.Width != this.Area.Width ||
				_scrollRenderTarget.Height != MaxVisibleHeight)
			{
				_scrollRenderTarget?.Dispose();
				_scrollRenderTarget = new RenderTarget2D(device, (int)this.Area.Width, MaxVisibleHeight);
			}

			// Draw content into RT
			var previousTargets = device.GetRenderTargets();
			device.SetRenderTarget(_scrollRenderTarget);
			device.Clear(Color.Transparent);

			drawingService.BeginDraw();

			var scrollOffset = new Vector2(0, -ScrollOffsetY);
			this.DrawContents(gameTime, gameServices, default, Color.White, scrollOffset);

			drawingService.EndDraw();

			device.SetRenderTargets(previousTargets);

			// Draw RT to screen at zone position
			//spriteBatch.Begin();
			//spriteBatch.Draw(_scrollRenderTarget, this.Position.Coordinates, Color.White);
			//spriteBatch.End();
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
				var horizontalOffset = block.HorizontalJustificationType switch
				{
					UiHorizontalJustificationType.Center => (block.AvailableWidth - block.TotalWidth) / 2,
					UiHorizontalJustificationType.Right => block.AvailableWidth - block.TotalWidth,
					_ => 0
				};
				var blockTop = verticalOffset + block.Margin.TopMargin;
				var blockLeft = horizontalOffset + block.Margin.LeftMargin;
				var result = new BlockLayoutInfo
				{
					Block = block,
					Offset = new Vector2
					{
						X = blockLeft,
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
				elementRow?.Dispose();
		}
	}
}
