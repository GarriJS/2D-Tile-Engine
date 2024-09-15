using Engine.Core.Contracts;
using Engine.UserInterface.Models;
using Microsoft.Xna.Framework;

namespace Engine.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a cursor service.
	/// </summary>
	public interface ICursorService : ILoadContent
	{
		/// <summary>
		/// Selects the text line.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		public void SelectTextLine(TextLine textLine);

		/// <summary>
		/// Unselects the text line.
		/// </summary>
		public void UnselectTextLine();

		/// <summary>
		/// Updates the selected text line.
		/// </summary>
		/// <param name="newText">The new text.</param>
		public void UpdateSelectedTextLine(string newText);

		/// <summary>
		/// Inserts text into the selected text line.
		/// </summary>
		/// <param name="insertText">The insert text.</param>
		/// <param name="gameTime">The game time.</param>
		public void InsertTextIntoSelectedTextLine(string insertText, GameTime gameTime);

		/// <summary>
		/// Deletes text from the selected text line.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public void DeleteTextFromSelectedTextLine(GameTime gameTime);
	}
}
