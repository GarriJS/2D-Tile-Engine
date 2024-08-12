using Game.Physics.Models.Contracts;
using System;

namespace Game.Drawing.Models.Contracts
{
	/// <summary>
	/// Represents something that can be drawn.
	/// </summary>
	public interface ICanBeDrawn : IHavePosition, IDisposable
	{    
		/// <summary>
		/// Gets the sprite.
		/// </summary>
		public Sprite Sprite { get; }
	}
}
