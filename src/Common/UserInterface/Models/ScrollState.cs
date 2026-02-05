using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a scroll state.
	/// </summary>
	public class ScrollState
	{	
		/// <summary>
		/// Gets or sets the vertical scroll offset.
		/// </summary>
		required public float VerticalScrollOffset { get; set; } = -10;
		
		/// <summary>
		/// Gets or sets the scroll speed.
		/// </summary>
		required public float ScrollSpeed { get; set; } = 30;
		
		/// <summary>
		/// Gets or sets the max visible height.
		/// </summary>
		required public float MaxVisibleHeight { get; set; } = 20;

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
			if (null == ScrollRenderTarget)
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
	}
}
