using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics.Models
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
	}
}
