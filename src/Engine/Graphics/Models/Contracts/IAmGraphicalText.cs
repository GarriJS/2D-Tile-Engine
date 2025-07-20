using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represents graphical text.
	/// </summary>
	public interface IAmGraphicalText : IAmSubDrawable
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
