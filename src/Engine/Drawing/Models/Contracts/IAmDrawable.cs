using Engine.Physics.Models.Contracts;
using System;

namespace Engine.Drawing.Models.Contracts
{
	/// <summary>
	/// Represents something that can be drawn.
	/// </summary>
	public interface IAmDrawable : IHavePosition, IDisposable
	{    
		/// <summary>
		/// Gets the draw layer.
		/// </summary>
		public int DrawLayer { get; }

		/// <summary>
		/// Gets the image.
		/// </summary>
		public Image Image { get; }
	}
}
