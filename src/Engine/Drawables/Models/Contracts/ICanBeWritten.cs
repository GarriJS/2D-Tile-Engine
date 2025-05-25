using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawables.Models.Contracts
{
	/// <summary>
	/// Represents something that can be written.
	/// </summary>
	public interface ICanBeWritten : ICanBeDrawn, IHavePosition
	{
		/// <summary>
		/// Gets the text.
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// Gets the font.
		/// </summary>
		public SpriteFont Font { get; }
	}
}
