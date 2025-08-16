using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Constants;
using Common.DiskModels.UI.Contracts;
using Common.DiskModels.UI.Elements;
using Common.UserInterface.Constants;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Models.Elements;
using Common.UserInterface.Services.Contracts;
using Engine.Core.Initialization.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Common.UserInterface.Services
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

			return uiElementSizeType switch
			{
				UiElementSizeTypes.ExtraSmall => new Vector2(uiScreenZone.Area.Width * ElementSizesScalars.ExtraSmall.X, uiScreenZone.Area.Height * ElementSizesScalars.ExtraSmall.Y),
				UiElementSizeTypes.Small => new Vector2(uiScreenZone.Area.Width * ElementSizesScalars.Small.X, uiScreenZone.Area.Height * ElementSizesScalars.Small.Y),
				UiElementSizeTypes.Medium => new Vector2(uiScreenZone.Area.Width * ElementSizesScalars.Medium.X, uiScreenZone.Area.Height * ElementSizesScalars.Medium.Y),
				UiElementSizeTypes.Large => new Vector2(uiScreenZone.Area.Width * ElementSizesScalars.Large.X, uiScreenZone.Area.Height * ElementSizesScalars.Large.Y),
				UiElementSizeTypes.ExtraLarge => new Vector2(uiScreenZone.Area.Width * ElementSizesScalars.ExtraLarge.X, uiScreenZone.Area.Height * ElementSizesScalars.ExtraLarge.Y),
				UiElementSizeTypes.Full => new Vector2(uiScreenZone.Area.Width, uiScreenZone.Area.Height),
				_ => null,
			};
		}

		/// <summary>
		/// Updates the element height.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="height">The height.</param>
		public void UpdateElementHeight(IAmAUiElement element, float height)
		{
			element.Area = new Vector2(element.Area.X, height);
			element.Graphic.TextureBox = new Rectangle(element.Graphic.TextureBox.X, element.Graphic.TextureBox.Y, element.Graphic.TextureBox.Width, (int)height);

			if (element is UiButton uiButton)
			{
				uiButton.ClickConfig.Area = new Vector2(uiButton.Area.X * uiButton.ClickableAreaScaler.X, uiButton.Area.Y * uiButton.ClickableAreaScaler.Y);
				uiButton.ClickConfig.Offset = new Vector2((uiButton.Area.X - uiButton.ClickConfig.Area.X) / 2, (uiButton.Area.Y - uiButton.ClickConfig.Area.Y) / 2);

				if ((null != uiButton.ClickAnimation) &&
					(0 < uiButton.ClickAnimation.Frames.Length))
				{
					foreach (var frame in uiButton.ClickAnimation.Frames)
					{
						frame.TextureBox = new Rectangle(frame.TextureBox.X, frame.TextureBox.Y, frame.TextureBox.Width, (int)(element.Graphic.TextureBox.Height * uiButton.ClickableAreaScaler.Y));
					}
				}
			}
		}

		/// <summary>
		/// Checks the user interface element for a click.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		/// <param name="pressLocation">The press location.</param>
		public void CheckForUiElementClick(IAmAUiElement element, Vector2 elementLocation, Vector2 pressLocation)
		{
			switch (element)
			{ 
				case UiButton button:

					var clickableLocation = new Vector2(elementLocation.X + ((element.Area.X - button.ClickConfig.Area.X) / 2), elementLocation.Y + ((element.Area.Y - button.ClickConfig.Area.Y) / 2));

					if ((clickableLocation.X <= pressLocation.X) &&
						(clickableLocation.X + button.ClickConfig.Area.X >= pressLocation.X) &&
						(clickableLocation.Y <= pressLocation.Y) &&
						(clickableLocation.Y + button.ClickConfig.Area.Y >= pressLocation.Y))
					{
						button.RaiseClickEvent(elementLocation);
					}

					break;
			}
		}

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="fillWidth">The fill width of the user interface element.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface element.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel, UiScreenZone uiZone, float fillWidth, int visibilityGroup)
		{
			var imageService = this._gameServices.GetService<IImageService>();
			var functionService = this._gameServices.GetService<IFunctionService>();

			var elementSize = this.GetElementDimensions(uiZone, uiElementModel);
			var width = true == elementSize.HasValue
				? elementSize.Value.X
				: fillWidth;
			var height = true == elementSize.HasValue
				? elementSize.Value.Y
				: 0;

			var image = imageService.GetImage(uiElementModel.BackgroundTextureName, (int)width, (int)height);
			var area = new Vector2(width, height);
			IAmAUiElement uiElement = uiElementModel switch
			{
				UiTextModel textModel => this.GetUiText(textModel, area),
				UiButtonModel buttonModel => this.GetUiButton(buttonModel, area),
				_ => null,
			};

			if (null != uiElement)
			{
				uiElement.VisibilityGroup = visibilityGroup;
				uiElement.Graphic = image;
				uiElement.PressConfig?.AddSubscription(this.CheckForUiElementClick);

				if ((false == string.IsNullOrEmpty(uiElementModel.ButtonHoverEventName)) &&
					(true == functionService.TryGetFunction<Action<IAmAUiElement, Vector2>>(uiElementModel.ButtonHoverEventName, out var hoverAction))) // LOGGING
				{
					uiElement.HoverConfig?.AddSubscription(hoverAction);
				}

				if ((false == string.IsNullOrEmpty(uiElementModel.ButtonPressEventName)) &&
					(true == functionService.TryGetFunction<Action<IAmAUiElement, Vector2, Vector2>>(uiElementModel.ButtonPressEventName, out var pressAction))) // LOGGING
				{
					uiElement.PressConfig?.AddSubscription(pressAction);
				}
			}

			return uiElement;
		}

		/// <summary>
		/// Gets the user interface text.
		/// </summary>
		/// <param name="textModel">The text model.</param>
		/// <param name="area">The area.</param>
		/// <returns>The user interface text.</returns>
		private UiText GetUiText(UiTextModel textModel, Vector2 area)
		{
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();

			var hoverConfig = cursorInteractionService.GetHoverConfiguration<IAmAUiElement>(area, CommonCursorNames.PrimaryCursorName);
			var pressConfig = cursorInteractionService.GetPressConfiguration<IAmAUiElement>(area);

			return new UiText
			{
				UiElementName = textModel.UiElementName,
				Text = textModel.Text,
				LeftPadding = textModel.LeftPadding,
				RightPadding = textModel.RightPadding,
				ElementType = UiElementTypes.Button,
				Area = area,
				HoverConfig = hoverConfig,
				PressConfig = pressConfig
			};
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
			var functionService = this._gameServices.GetService<IFunctionService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();

			var clickableArea = new Vector2(area.X * buttonModel.ClickableAreaScaler.X, area.Y * buttonModel.ClickableAreaScaler.Y);
			var clickableOffset = new Vector2((area.X - clickableArea.X) / 2, (area.Y - clickableArea.Y) /2);
			var hoverConfig = cursorInteractionService.GetHoverConfiguration<IAmAUiElement>(area, CommonCursorNames.PrimaryCursorName);
			var pressConfig = cursorInteractionService.GetPressConfiguration<IAmAUiElement>(area);
			var clickConfig = cursorInteractionService.GetClickConfiguration<IAmAUiElement>(clickableArea, clickableOffset);
			var button = new UiButton
			{
				UiElementName = buttonModel.UiElementName,
				Text = buttonModel.Text,
				LeftPadding = buttonModel.LeftPadding,
				RightPadding = buttonModel.RightPadding,
				ElementType = UiElementTypes.Button,
				ClickableAreaScaler = buttonModel.ClickableAreaScaler,
				Area = area,
				HoverConfig = hoverConfig,
				PressConfig = pressConfig,
				ClickConfig = clickConfig
			};

			button.ClickConfig?.AddSubscription(this.TriggerUiButtonClickAnimation);

			if ((false == string.IsNullOrEmpty(buttonModel.ButtonClickEventName)) &&
				(true == functionService.TryGetFunction<Action<IAmAUiElement, Vector2>>(buttonModel.ButtonClickEventName, out var clickAction))) // LOGGING
			{
				button.ClickConfig?.AddSubscription(clickAction);
			}

			if (null != buttonModel.ClickableAreaAnimation)
			{
				//, (int)clickableArea.X, (int)clickableArea.Y
				var clickAnimation = animationService.GetAnimationFromModel(buttonModel.ClickableAreaAnimation);

				if (clickAnimation is TriggeredAnimation triggeredAnimation)
				{
					button.ClickAnimation = triggeredAnimation;
				}
			}

			return button;
		}

		/// <summary>
		/// Triggers the user interface elements click animation.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		private void TriggerUiButtonClickAnimation(IAmAUiElement element, Vector2 elementLocation)
		{
			var uiService = this._gameServices.GetService<IUserInterfaceService>();
			var animationService = this._gameServices.GetService<IAnimationService>();

			uiService.ToggleUserInterfaceGroupVisibility(element.VisibilityGroup == 1 ? 2 : 1);

			if ((element is UiButton button) &&
				(null != button.ClickAnimation))
			{
				button.ClickAnimation.TriggerAnimation(true);
			}
		}
	}
}
