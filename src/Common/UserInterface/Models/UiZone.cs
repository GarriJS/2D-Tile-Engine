using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Models.LayoutInfo;
using Engine.Debugging.Models.Contracts;
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
	public class UiZone : IAmDrawable, IAmPreRenderable, IAmDebugDrawable, IAmScrollable, IHaveArea, IHaveAHoverCursor, ICanBeHovered<UiZone>, IDisposable
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
		/// Gets the scroll state.
		/// </summary>
		public ScrollState ScrollState { get; set; }

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

			if (null != this.ScrollState?.ScrollRenderTarget)
			{
				var sourceRectangle = this.ScrollState.GetSourceRectanlge();
				drawingService.Draw(this.ScrollState.ScrollRenderTarget, this.Position.Coordinates, sourceRectangle, Color.White); 
				this.ScrollState.Draw(gameTime, gameServices, this.Position.Coordinates, Color.White);
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
		/// <param name="offset">The offset.</param>
		private void DrawContents(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			this.Graphic?.Draw(gameTime, gameServices, coordinates, color, offset);

			if (true == this.ResetCalculateCachedOffsets)
				this.UpdateZoneOffsets();

			foreach (var block in this.Blocks ?? [])
				block.Draw(gameTime, gameServices, coordinates, color, offset);
		}

		/// <summary>
		/// Assess if prerendering is needed.
		/// </summary>
		/// <returns>A value indicating whether prerendering is needed.</returns>
		public bool ShouldPreRender()
		{
			var needsSubRender = this.Blocks.Any(e => true == e.ShouldPreRender());

			if (true == needsSubRender)
				return true;

			if (null == this.ScrollState)
				return false;

			var contentHeight = this.Blocks.Sum(e => e.TotalHeight);
			var hasExessHeight = contentHeight > this.ScrollState.MaxVisibleHeight;

			return hasExessHeight;
		}

		/// <summary>
		/// Does the prerender.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game service.</param>
		public void PreRender(GameTime gameTime, GameServiceContainer gameServices)
		{
			var subPrerenders = this.Blocks.Where(e => true == e.ShouldPreRender())
										   .ToArray();

			foreach (var subPrerender in subPrerenders ?? [])
				subPrerender.PreRender(gameTime, gameServices, default, Color.White);

			if ((null == this.ScrollState) ||
				(true == this.ScrollState.DisableScrolling))
				return;

			var graphicDeviceService = gameServices.GetService<IGraphicsDeviceService>();
			var device = graphicDeviceService.GraphicsDevice;
			var drawingService = gameServices.GetService<IDrawingService>();
			var contentHeight = this.Blocks.Sum(e => e.TotalHeight);

			if ((null == this.ScrollState.ScrollRenderTarget) ||
				(this.ScrollState.ScrollRenderTarget.Width != this.Area.Width) ||
				(this.ScrollState.ScrollRenderTarget.Height != contentHeight))
			{
				this.ScrollState.ScrollRenderTarget?.Dispose();
				this.ScrollState.ScrollRenderTarget = new RenderTarget2D(device, (int)this.Area.Width, (int)contentHeight);
			}

			var previousTargets = device.GetRenderTargets();
			device.SetRenderTarget(this.ScrollState.ScrollRenderTarget);
			device.Clear(Color.Transparent);
			drawingService.BeginDraw();

			this.DrawContents(gameTime, gameServices, default, Color.White);

			drawingService.EndDraw();
			device.SetRenderTargets(previousTargets);
		}

		/// <summary>
		/// Updates the zone offsets.
		/// </summary>
		public void UpdateZoneOffsets()
		{
			foreach (var blockLayout in this.EnumerateLayout(includeScrollOffset: false) ?? [])
			{
				blockLayout.Block.CachedOffset = blockLayout.Offset;
				blockLayout.Block.UpdateOffsets();
			}

			this.ScrollState?.UpdateOffset(this.Area.Width, this.Area.Width, 0);
			this.ResetCalculateCachedOffsets = false;
		}

		/// <summary>
		/// Enumerates the Block blockLayout.
		/// </summary>
		/// <param name="includeScrollOffset">A value indicating whether to include the scroll offset.</param>
		/// <returns>The enumerated Block blockLayout.</returns>
		public IEnumerable<BlockLayoutInfo> EnumerateLayout(bool includeScrollOffset)
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

			if ((true == includeScrollOffset) &&
				(null != this.ScrollState))
				verticalOffset -= this.ScrollState.VerticalScrollOffset;

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
		/// Draws the debug drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices)
		{
			var offset = new Vector2
			{
				X = 0,
				Y = -this.ScrollState?.VerticalScrollOffset ?? default
			};

			foreach (var block in this.Blocks ?? [])
				block.DrawDebug(gameTime, gameServices, this.Position.Coordinates, Color.MonoGameOrange, offset);

			this.Area.Draw(gameTime, gameServices, Color.MonoGameOrange);
		}

		/// <summary>
		/// Disposes of the user interface container.
		/// </summary>
		public void Dispose()
		{
			this.ScrollState.Dispose();
			this.CursorConfiguration?.Dispose();

			foreach (var elementRow in this.Blocks ?? [])
				elementRow?.Dispose();
		}
	}
}
