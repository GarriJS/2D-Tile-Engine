using Engine.DiskModels.UI.Elements;
using Engine.UI.Models.Elements;
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
		/// <param name="button">The button.</param>
		public void SpritesheetButtonClickEventProcessor(UiButton button);

		/// <summary>
		/// Gets the user interface buttons for the spritesheet.
		/// </summary>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <param name="spriteDimensions">The sprite dimensions.</param>
		/// <returns></returns>
		public UiButtonModel[][] GetUiButtonsForSpritesheet(string spritesheetName, Point spriteDimensions);
	}
}
