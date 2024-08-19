using Engine.UserInterface.Models;

namespace Engine.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a text input line service.
	/// </summary>
	public interface ITextInputLineService
	{
		/// <summary>
		/// Moves the text line view area.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		/// <param name="horizontalMoveAmount">The horizontal move amount.</param>
		public void MoveTextLineViewArea(TextLine textLine, int horizontalMoveAmount);

		/// <summary>
		/// Updates the text line sprite.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		/// <param name="newText">The new text.</param>
		public void UpdateTextLineSprite(TextLine textLine, string newText = null);
	}
}
