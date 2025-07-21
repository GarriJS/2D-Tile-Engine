using Common.DiskModels.UI.Elements;
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
		/// The spritesheet button hover event processor.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void SpritesheetButtonHoverEventProcessor(IAmAUiElement element, Vector2 elementLocation);

		/// <summary>
		/// The spritesheet button click event processor.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void SpritesheetButtonClickEventProcessor(IAmAUiElement element, Vector2 elementLocation);

		/// <summary>
		/// Gets the user interface buttons for the spritesheet.
		/// </summary>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <param name="spriteDimensions">The sprite dimensions.</param>
		/// <returns></returns>
		public UiButtonModel[][] GetUiButtonsForSpritesheet(string spritesheetName, Point spriteDimensions);
	}
}
