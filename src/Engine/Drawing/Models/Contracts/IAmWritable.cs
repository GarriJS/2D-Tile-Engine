using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing.Models.Contracts
{
	/// <summary>
	/// Represents something that is writable.
	/// </summary>
	public interface IAmWritable : IHavePosition
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
