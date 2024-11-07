using Engine.UserInterface.Models;

namespace Engine.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a text input line service.
	/// </summary>
	public interface ITextLineService
	{
		/// <summary>
		/// Moves the text line view area.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		/// <param name="stringIndex">The string index.</param>
		public void MoveTextLineViewArea(SubTextLine textLine, int stringIndex);

		/// <summary>
		/// Updates the text line sprite.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		public void UpdateTextLineSprite(SubTextLine textLine, int viewableTextOffset = 0);
	}
}
