using DiscModels.Engine.UI.Contracts;
using DiscModels.Engine.UI.Elements;
using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Elements;
using Engine.UI.Models.Enums;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
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
		/// Gets the element dimensions.
		/// </summary>
		/// <param name="uiScreenZone">The user interface screen zone.</param>
		/// <param name="elementModel">The user interface element model.</param>
		/// <returns>The element dimensions.</returns>
		public Vector2? GetElementDimensions(UiScreenZone uiScreenZone, IAmAUiElementModel elementModel)
		{
			if (true == elementModel.FixedSized.HasValue)
			{ 
				return elementModel.FixedSized.Value;
			}

			if (null == uiScreenZone?.Area)
			{
				return null;
			}

			var uiElementSizeType = Enum.IsDefined(typeof(UiElementSizeTypes), elementModel.SizeType)
									? (UiElementSizeTypes)elementModel.SizeType
									: UiElementSizeTypes.None;

			switch (uiElementSizeType)
			{
				default:
				case UiElementSizeTypes.None:
				case UiElementSizeTypes.Fill:
					return null;
				case UiElementSizeTypes.ExtraSmall:
					return new Vector2(uiScreenZone.Area.Width / 6, uiScreenZone.Area.Height / 6);
				case UiElementSizeTypes.Small:
					return new Vector2(uiScreenZone.Area.Width / 5, uiScreenZone.Area.Height / 5);
				case UiElementSizeTypes.Medium:
					return new Vector2(uiScreenZone.Area.Width / 4, uiScreenZone.Area.Height / 4);
				case UiElementSizeTypes.Large:
					return new Vector2(uiScreenZone.Area.Width / 3, uiScreenZone.Area.Height / 3);
				case UiElementSizeTypes.ExtraLarge:
					return new Vector2(uiScreenZone.Area.Width / 2, uiScreenZone.Area.Height / 2);
				case UiElementSizeTypes.Full:
					return new Vector2(uiScreenZone.Area.Width, uiScreenZone.Area.Height);
			}
		}

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="fillWidth">The fill width of the user interface element.</param>
		/// <param name="fillHeight">The fill height of the user interface element model.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel, UiScreenZone uiZone, float fillWidth, float fillHeight)
		{
			var elementSize = this.GetElementDimensions(uiZone, uiElementModel);
			var width = true == elementSize.HasValue
						? elementSize.Value.X
						: fillWidth;
			var height = true == elementSize.HasValue
						 ? elementSize.Value.Y
						 : fillHeight;

			var imageService = this._gameServices.GetService<IImageService>();
			var image = imageService.GetImage(uiElementModel.BackgroundTextureName, (int)width, (int)height);
			var area = new Vector2(width, height);

			var uiElement = uiElementModel switch
			{
				UiButtonModel buttonModel => GetUiButton(buttonModel, area),
				_ => null,
			};

			if (null != uiElement)
			{
				uiElement.Image = image;
				this.UserInterfaceElements.Add(uiElement);
			}

			return uiElement;
		}

		/// <summary>
		/// Gets the user interface button.
		/// </summary>
		/// <param name="buttonModel">The user interface button model.</param>
		/// <param name="area">The area.</param>
		/// <returns>The user interface button.</returns>
		private UiButton GetUiButton(UiButtonModel buttonModel, Vector2 area)
		{
			var animationService = this._gameServices.GetService<IAnimationService>();
			var clickableArea = new Vector2(area.X * buttonModel.ClickableAreaScaler.X, area.Y * buttonModel.ClickableAreaScaler.Y);
			var button =  new UiButton
			{
				UiElementName = buttonModel.UiElementName,
				ButtonText = buttonModel.ButtonText,
				LeftPadding = buttonModel.LeftPadding,
				RightPadding = buttonModel.RightPadding,
				ElementType = UiElementTypes.Button,
				Area = area,
				ClickableArea = clickableArea
				//Signal = ?? TODO
			};

			if (null != buttonModel.ClickableAreaAnimation)
			{
				var clickAnimation = animationService.GetAnimation(buttonModel.ClickableAreaAnimation, (int)clickableArea.X, (int)clickableArea.Y);

				if (clickAnimation is TriggeredAnimation triggeredAnimation)
				{
					button.ClickAnimation = triggeredAnimation;
				}
			}

			return button;
		}
	}
}
