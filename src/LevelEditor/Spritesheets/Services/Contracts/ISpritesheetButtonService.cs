using Common.Controls.CursorInteraction.Models;
using Common.Controls.Cursors.Models;
using Common.DiskModels.UI.Elements;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Microsoft.Xna.Framework;

namespace LevelEditor.Spritesheets.Services.Contracts
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
	public interface ISpritesheetButtonService
	{
		/// <summary>
		/// The spritesheet button click event processor.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void SpritesheetButtonClickEventProcessor(CursorInteraction<IAmAUiElement> cursorInteraction);

		/// <summary>
		/// Updates the spritesheet button trailing cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>\
		/// <param name="gameTime">The game time.</param>
		public void SpritesheetButtonCursorUpdater(Cursor cursor, GameTime gameTime);

		/// <summary>
		/// Gets the user interface zone for the spritesheet buttons.
		/// </summary>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <param name="backgroundTexture">The background texture.</param>
		/// <param name="uiScreenZoneType">The user interface zone type.</param>
		/// <returns>The user interface zone.</returns>
		public UiZone GetUiZoneForSpritesheet(string spritesheetName, string backgroundTexture, UiScreenZoneTypes uiScreenZoneType);

		/// <summary>
		/// Gets the user interface buttons for the spritesheet.
		/// </summary>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <param name="spriteDimensions">The sprite dimensions.</param>
		/// <returns></returns>
		public UiButtonModel[][] GetUiButtonsForSpritesheet(string spritesheetName, Point spriteDimensions);
	}
}
