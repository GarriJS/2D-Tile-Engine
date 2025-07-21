using System;

namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represents something that has a graphic.
	/// </summary>
	public interface IHaveAGraphic : IDisposable
	{
		/// <summary>
		/// Get the graphic.
		/// </summary>
		public IAmAGraphic Graphic { get; }
	}
}
