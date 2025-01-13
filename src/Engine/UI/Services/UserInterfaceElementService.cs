using DiscModels.Engine.UI.Contracts;
using DiscModels.Engine.UI.Elements;
using Engine.Drawing.Services.Contracts;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Elements;
using Engine.UI.Models.Enums;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.UI.Services
{
	/// <summary>
	/// Represents a user interface element service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface element service.
	/// </remarks>
	/// <param name="gameServices">The game service.</param>
	public class UserInterfaceElementService(GameServiceContainer gameServices) : IUserInterfaceElementService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets the user interface elements.
		/// </summary>
		private List<IAmAUiElement> UserInterfaceElements { get; set; } = [];

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <param name="width">The width of the user interface element.</param>
		/// <param name="height">The height of the user interface element model.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel, float width, float height)
		{
			var imageService = this._gameServices.GetService<IImageService>();
			var image = imageService.GetImage(uiElementModel.BackgroundTextureName, (int)width, (int)height);

			var uiElement = uiElementModel switch
			{
				UiButtonModel buttonModel => GetUiButton(buttonModel),
				_ => null,
			};

			if (null != uiElement)
			{
				uiElement.Area = new Vector2(width, height);
				uiElement.Image = image;
				this.UserInterfaceElements.Add(uiElement);
			}

			return uiElement;
		}

		/// <summary>
		/// Gets the user interface button.
		/// </summary>
		/// <param name="buttonModel">The user interface button model.</param>
		/// <returns>The user interface button.</returns>
		private static UiButton GetUiButton(UiButtonModel buttonModel)
		{
			return new UiButton
			{
				UiElementName = buttonModel.UiElementName,
				LeftPadding = buttonModel.LeftPadding,
				RightPadding = buttonModel.RightPadding,
				ElementType = UiElementTypes.Button
				//Signal = ?? TODO
			};
		}
	}
}
