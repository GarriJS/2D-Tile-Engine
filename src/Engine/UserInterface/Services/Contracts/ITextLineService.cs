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
		/// <param name="newText">The new text.</param>
		/// <param name="stringIndex">The string index.</param>
		public void UpdateTextLineSprite(SubTextLine textLine, string newText = null, int? stringIndex = null);

		/// <summary>
		/// Updates the text line collection sprite.
		/// </summary>
		/// <param name="textLineCollection">The text line collection.</param>
		/// <param name="newTextLine">The new text line.</param>
		/// <param name="frontAdd">A value indicating whether to do a front add.</param>
		/// <param name="bottomToTop">A value indicating whether the text line collection is populated bottom to top.</param>
		public void UpdateTextLineCollectionSprite(TextLineCollection textLineCollection, SubTextLine newTextLine = null, bool frontAdd = false, bool bottomToTop = false);
	}
}
