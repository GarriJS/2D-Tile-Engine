using Common.UserInterface.Enums;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a scroll state.
	/// </summary>
	sealed public class ScrollState : IAmSubDrawable, IDisposable
	{
		/// <summary>
		/// Gets or sets the cached offset.
		/// </summary>
		public Vector2? CachedOffset { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to disable scrolling.
		/// </summary>
		required public bool DisableScrolling { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to draw the scroll wheel. 
		/// </summary>
		required public bool DrawScrollWheel { get; set; }

		/// <summary>
		/// Gets or sets the vertical scroll offset.
		/// </summary>
		required public float VerticalScrollOffset { get; set; }
		
		/// <summary>
		/// Gets or sets the scroll speed.
		/// </summary>
		required public float ScrollSpeed { get; set; }
		
		/// <summary>
		/// Gets or sets the max visible height.
		/// </summary>
		required public float MaxVisibleHeight { get; set; }

		/// <summary>
		/// Gets or sets the scroll bar width.
		/// </summary>
		required public int ScrollBarWidth { get; set; }

		/// <summary>
		/// Gets or sets the scroll state justification type.
		/// </summary>
		required public ScrollStateJustificationType ScrollStateJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the scroll background color.
		/// </summary>
		required public Color ScrollBackgroundColor { get; set; }

		/// <summary>
		/// Gets or sets the scroll notch color.
		/// </summary>
		required public Color ScrollNotchColor { get; set; }

		/// <summary>
		/// Gets or sets the scroll render target.
		/// </summary>
		public RenderTarget2D ScrollRenderTarget { get; set; }

		/// <summary>
		/// Increase the vertical offset of the scroll state.
		/// </summary>
		/// <param name="scrollDelta">The scroll delta.</param>
		public void Scroll(float scrollDelta)
		{
			if ((this.ScrollRenderTarget is null) ||
				(true == this.DisableScrolling))
				return;

			this.VerticalScrollOffset -= scrollDelta * ScrollSpeed;
			var maxScroll = Math.Max(0, this.ScrollRenderTarget.Height - this.MaxVisibleHeight);
			this.VerticalScrollOffset = MathHelper.Clamp(this.VerticalScrollOffset, 0, maxScroll);
		}

		/// <summary>
		/// Gets the source rectangle.
		/// </summary>
		/// <returns>The source rectangle.</returns>
		public Rectangle GetSourceRectanlge()
		{
			var result = new Rectangle
			{
				X = 0,
				Y = (int)this.VerticalScrollOffset,
				Width = this.ScrollRenderTarget.Width,
				Height = (int)this.MaxVisibleHeight
			};

			return result;
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			if ((true == this.DisableScrolling) ||
				(false == this.DrawScrollWheel))
				return;

			var drawingSerivce = gameServices.GetService<IDrawingService>();
			var scrollStateOffset = offset + this.CachedOffset ?? default;
			var backgroundRectangle = new Rectangle
			{
				X = (int)(coordinates.X + scrollStateOffset.X),
				Y = (int)(coordinates.Y + scrollStateOffset.Y),
				Width = this.ScrollBarWidth,
				Height = (int)this.MaxVisibleHeight
			};
			drawingSerivce.DrawRectangle(backgroundRectangle, this.ScrollBackgroundColor);
			var maxScroll = Math.Max(0, this.ScrollRenderTarget.Height - this.MaxVisibleHeight);
			var notchHeight = (int)Math.Max(8f, this.MaxVisibleHeight * (this.MaxVisibleHeight / this.ScrollRenderTarget.Height));
			var scrollRatio = this.VerticalScrollOffset / maxScroll;
			var maxThumbTravel = this.MaxVisibleHeight - notchHeight;
			var notchRectangle = new Rectangle
			{
				X = (int)(coordinates.X + scrollStateOffset.X) + 1,
				Y = (int)(coordinates.Y + scrollStateOffset.Y + (scrollRatio * maxThumbTravel)),
				Width = this.ScrollBarWidth - 2,
				Height = notchHeight
			};
			drawingSerivce.DrawRectangle(notchRectangle, this.ScrollNotchColor);
		}

		/// <summary>
		/// Updates the offset.
		/// </summary>
		/// <param name="availableWidth">The available width.</param>
		/// <param name="insideWidth">The inside width.</param>
		/// <param name="leftOffset">The left offset.</param>
		public void UpdateOffset(float availableWidth, float insideWidth, float leftOffset)
		{
			var horizontalOffset = this.ScrollStateJustificationType switch
			{
				ScrollStateJustificationType.FarLeft => 0,
				ScrollStateJustificationType.NearLeft => leftOffset - this.ScrollBarWidth,
				ScrollStateJustificationType.NearRight => leftOffset + insideWidth,
				ScrollStateJustificationType.FarRight => availableWidth - this.ScrollBarWidth,
				_ => 0
			};
			this.CachedOffset = new Vector2
			{
				X = horizontalOffset,
				Y = 0
			};
		}

		/// <summary>
		/// Disposes of the scroll state.
		/// </summary>
		public void Dispose()
		{ 
			this.ScrollRenderTarget?.Dispose();
		}
	}
}
