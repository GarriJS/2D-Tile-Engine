using Engine.Physics.Models.Contracts;
using System;

namespace Engine.Drawables.Models.Contracts
{
	/// <summary>
	/// Represents something that has an image.
	/// </summary>
	public interface IHaveAnImage : ICanBeDrawn, IHavePosition, IDisposable
	{    
		/// <summary>
		/// Gets the image.
		/// </summary>
		public Image Image { get; }
	}
}
