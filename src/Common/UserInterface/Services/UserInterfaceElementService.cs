using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Services.Contracts;
using Common.DiskModels.UI;
using Common.DiskModels.UI.Contracts;
using Common.DiskModels.UI.Elements;
using Common.UserInterface.Constants;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Models.Elements;
using Common.UserInterface.Services.Contracts;
using Engine.Core.Fonts.Services.Contracts;
using Engine.Core.Initialization.Services.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using System;

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
		/// Gets the user interface insidePadding from the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>The user interface insidePadding model.</returns>
		public UiPadding GetUiPaddingFromModel(UiPaddingModel model)
		{
			var result = new UiPadding
			{
				TopPadding = model.TopPadding,
				BottomPadding = model.BottomPadding,
				LeftPadding = model.LeftPadding,
				RightPadding = model.RightPadding,
			};

			return result;
		}

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel)
		{
			var graphicService = this._gameServices.GetService<IGraphicService>();
			var functionService = this._gameServices.GetService<IFunctionService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var graphicTextService = this._gameServices.GetService<IGraphicTextService>();

			var area = this.GetElementArea(uiElementModel);
			IAmAUiElement uiElement = uiElementModel switch
			{
				UiTextModel textModel => new UiText { },
				UiButtonModel buttonModel => this.GetUiButton(buttonModel, area),
				_ => null,
			};

			if (null == uiElement)
			{
				// LOGGING

				return null;
			}

			uiElement.OutsidePadding = this.GetUiPaddingFromModel(uiElementModel.OutsidePadding);
			uiElement.InsidePadding = this.GetUiPaddingFromModel(uiElementModel.InsidePadding);
			uiElement.Area = area;

			if (null != uiElementModel.Graphic)
			{
				uiElement.Graphic = graphicService.GetGraphicFromModel(uiElementModel.Graphic);

				if ((true == uiElementModel.ResizeTexture) ||
					(uiElement.Graphic is CompositeImage compositeImage))
				{
					var dimensions = new SubArea
					{
						Width = uiElement.InsideWidth,
						Height = uiElement.InsideHeight
					};
					uiElement.Graphic.SetDrawDimensions(dimensions);
				}
			}

			uiElement.HorizontalSizeType = uiElementModel.HorizontalSizeType;
			uiElement.VerticalSizeType = uiElementModel.VerticalSizeType;
			uiElement.CursorConfiguration ??= cursorInteractionService.GetCursorConfiguration<IAmAUiElement>(area);

			if ((uiElement is IAmAUiElementWithText uiElementWithText) &&
				(uiElementModel is IAmAUiElementWithTextModel uiElementWithTextModel))
			{
				uiElementWithText.GraphicText = graphicTextService.GetGraphicTextFromModel(uiElementWithTextModel.Text);
			}

			if (true == functionService.TryGetFunction<Action<CursorInteraction<IAmAUiElement>>>(uiElementModel.HoverCursorName, out var hoverAction))
			{
				uiElement.CursorConfiguration?.AddHoverSubscription(hoverAction);
			}

			if (true == functionService.TryGetFunction<Action<CursorInteraction<IAmAUiElement>>>(uiElementModel.PressEventName, out var pressAction))
			{
				uiElement.CursorConfiguration?.AddPressSubscription(pressAction);
			}

			return uiElement;
		}

		/// <summary>
		/// Updates the element height.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="height">The height.</param>
		public void UpdateElementHeight(IAmAUiElement element, float height)
		{
			element.Area.Height = height;
			element.Graphic.SetDrawDimensions(element.Area);

			if (element is not UiButton uiButton)
			{
				return;
			}

			uiButton.CursorConfiguration.Area = new SubArea
			{
				Width = uiButton.Area.Width * uiButton.ClickableAreaScaler.X,
				Height = uiButton.Area.Height * uiButton.ClickableAreaScaler.Y
			};
			uiButton.CursorConfiguration.ClickOffset = new Vector2
			{
				X = (uiButton.Area.Width - uiButton.CursorConfiguration.ClickArea.Width) / 2,
				Y = (uiButton.Area.Height - uiButton.CursorConfiguration.ClickArea.Height) / 2
			};

			//TODO updates press and hover areas?

			if (null == uiButton.ClickAnimation?.Frames)
			{
				return;
			}

			foreach (var frame in uiButton.ClickAnimation.Frames)
			{
				var subArea = new SubArea
				{
					Width = frame.Dimensions.Width,
					Height = (int)(element.Graphic.Dimensions.Height * uiButton.ClickableAreaScaler.Y)
				};

				frame.SetDrawDimensions(subArea);
			}
		}

		/// <summary>
		/// Gets the element area.
		/// </summary>
		/// <param name="elementModel">The user interface element model.</param>
		/// <returns>The element area.</returns>
		private SubArea GetElementArea(IAmAUiElementModel elementModel)
		{
			var uiScreenZoneService = this._gameServices.GetService<IUserInterfaceScreenZoneService>();

			if (true == elementModel.FixedSized.HasValue)
			{
				var fixedSize = elementModel.FixedSized.Value;

				if (0 > fixedSize.X)
				{
					fixedSize.X = 0;
				}

				if (0 > fixedSize.Y)
				{
					fixedSize.Y = 0;
				}

				return new SubArea
				{
					Width = fixedSize.X,
					Height = fixedSize.Y,
				};
			}

			if (null == uiScreenZoneService?.ScreenZoneSize)
			{
				return default;
			}

			SubArea elementFitDimensions = null;

			if ((UiElementSizeType.FitContent == elementModel.HorizontalSizeType) ||
				(UiElementSizeType.FitContent == elementModel.VerticalSizeType))
			{
				elementFitDimensions = this.GetElementFitDimensions(elementModel);
			}

			var uiElementHorizontalSize = elementModel.HorizontalSizeType switch
			{
				UiElementSizeType.ExtraSmall => uiScreenZoneService.ScreenZoneSize.Width * ElementSizesScalars.ExtraSmall.X,
				UiElementSizeType.Small => uiScreenZoneService.ScreenZoneSize.Width * ElementSizesScalars.Small.X,
				UiElementSizeType.Medium => uiScreenZoneService.ScreenZoneSize.Width * ElementSizesScalars.Medium.X,
				UiElementSizeType.Large => uiScreenZoneService.ScreenZoneSize.Width * ElementSizesScalars.Large.X,
				UiElementSizeType.ExtraLarge => uiScreenZoneService.ScreenZoneSize.Width * ElementSizesScalars.ExtraLarge.X,
				UiElementSizeType.FitContent => elementFitDimensions.Width,
				_ => 0
			};
			var uiElementVerticalSize = elementModel.VerticalSizeType switch
			{
				UiElementSizeType.ExtraSmall => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.ExtraSmall.Y,
				UiElementSizeType.Small => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.Small.Y,
				UiElementSizeType.Medium => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.Medium.Y,
				UiElementSizeType.Large => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.Large.Y,
				UiElementSizeType.ExtraLarge => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.ExtraLarge.Y,
				UiElementSizeType.FitContent => elementFitDimensions.Height,
				_ => 0
			};

			var result = new SubArea
			{
				Width = uiElementHorizontalSize,
				Height = uiElementVerticalSize
			};

			return result;
		}

		/// <summary>
		/// Gets the element fit dimensions.
		/// </summary>
		/// <param name="elementModel">The element model.</param>
		/// <returns>The element fit dimensions.</returns>
		private SubArea GetElementFitDimensions(IAmAUiElementModel elementModel)
		{
			var width = 0;
			var height = 0;
			var result = new SubArea
			{
				Width = -1,
				Height = -1
			};

			if (null != elementModel.Graphic)
			{
				result = elementModel.Graphic.GetDimensions();
			}

			switch (elementModel)
			{
				case UiButtonModel uiButtonModel:

					return new SubArea
					{
						Width = 64,
						Height = 64
					};


					if (null == uiButtonModel.ClickableAreaAnimation)
					{
						break;
					}

					var restingFrame = uiButtonModel.ClickableAreaAnimation?.Frames[uiButtonModel.ClickableAreaAnimation.RestingFrameIndex];

					if (null != restingFrame)
					{
						result = restingFrame.GetDimensions();
					}

					break;

				case UiTextModel uiTextModel:

					break;
			}

			if (elementModel is IAmAUiElementWithTextModel elementTextModel)
			{
				var fontService = this._gameServices.GetService<IFontService>();

				if (false == string.IsNullOrEmpty(elementTextModel.Text?.FontName))
				{
					var font = fontService.GetSpriteFont(elementTextModel.Text.FontName);
					var textDimensions = font.MeasureString(elementTextModel.Text.Text);
					result.Width = (int)Math.Max(textDimensions.X, width);
					result.Height = (int)Math.Max(textDimensions.Y, height);
				}
			}

			return result;
		}

		/// <summary>
		/// Checks the user interface element for a click.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		/// <param name="cursorLocation">The cursor location.</param>
		public void CheckForUiElementClick(IAmAUiElement element, Vector2 elementLocation, Vector2 cursorLocation)
		{
			switch (element)
			{
				case UiButton button:

					var clickableLocation = new Vector2
					{
						X = elementLocation.X + button.CursorConfiguration.ClickOffset.X + ((element.InsideWidth - button.CursorConfiguration.ClickArea.Width) / 2),
						Y = elementLocation.Y + button.CursorConfiguration.ClickOffset.Y + ((element.InsideHeight - button.CursorConfiguration.ClickArea.Height) / 2)
					};

					if ((clickableLocation.X <= cursorLocation.X) &&
						(clickableLocation.X + button.CursorConfiguration.ClickArea.Width >= cursorLocation.X) &&
						(clickableLocation.Y <= cursorLocation.Y) &&
						(clickableLocation.Y + button.CursorConfiguration.ClickArea.Height >= cursorLocation.Y))
					{
						var cursorInteraction = new CursorInteraction<IAmAUiElement>
						{
							CursorLocation = cursorLocation,
							ElementLocation = clickableLocation,
							Element = button
						};

						button.RaiseClickEvent(cursorInteraction);
					}

					break;
			}
		}

		/// <summary>
		/// Gets the user interface button.
		/// </summary>
		/// <param name="buttonModel">The user interface button model.</param>
		/// <param name="area">The area.</param>
		/// <returns>The user interface button.</returns>
		private UiButton GetUiButton(UiButtonModel buttonModel, SubArea area)
		{
			var graphicTextService = this._gameServices.GetService<IGraphicTextService>();
			var animationService = this._gameServices.GetService<IAnimationService>();
			var functionService = this._gameServices.GetService<IFunctionService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();

			var graphicText = graphicTextService.GetGraphicTextFromModel(buttonModel.Text);
			var clickableArea = new SubArea
			{
				Width = area.Width * buttonModel.ClickableAreaScaler.X,
				Height = area.Height * buttonModel.ClickableAreaScaler.Y
			};
			var clickableOffset = new Vector2
			{
				X = (area.Width - clickableArea.Width) / 2,
				Y = (area.Height - clickableArea.Height) / 2
			};
			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<IAmAUiElement>(clickableArea, clickableOffset);
			var button = new UiButton
			{
				UiElementName = buttonModel.UiElementName,
				GraphicText = graphicText,
				ClickableAreaScaler = buttonModel.ClickableAreaScaler,
				CursorConfiguration = cursorConfiguration
			};

			button.CursorConfiguration?.AddClickSubscription(this.TriggerUiButtonClickAnimation);

			// LOGGING
			if (true == functionService.TryGetFunction<Action<CursorInteraction<IAmAUiElement>>>(buttonModel.ClickEventName, out var clickAction))
			{
				button.CursorConfiguration?.AddClickSubscription(clickAction);
			}

			if (null != buttonModel.ClickableAreaAnimation)
			{
				var clickAnimation = animationService.GetFixedAnimationFromModel(buttonModel.ClickableAreaAnimation, (int)clickableArea.Width, (int)clickableArea.Height);

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
		/// <param name="cursorInteraction">The element with location.</param>
		private void TriggerUiButtonClickAnimation(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			if (cursorInteraction.Element is UiButton button)
			{
				button.ClickAnimation?.TriggerAnimation(allowReset: true);
			}
		}
	}
}
