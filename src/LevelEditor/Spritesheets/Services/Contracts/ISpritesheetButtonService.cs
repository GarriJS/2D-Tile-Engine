using Engine.DiskModels.UI.Elements;
using Microsoft.Xna.Framework;

namespace LevelEditor.Spritesheets.Services.Contracts
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
	public interface ISpritesheetButtonService
	{
		/// <summary>
		/// Gets the user interface buttons for the spritesheet.
		/// </summary>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <param name="spriteDimensions">The sprite dimensions.</param>
		/// <returns></returns>
		UiButtonModel[][] GetUiButtonsForSpritesheet(string spritesheetName, Point spriteDimensions);
	}
}
