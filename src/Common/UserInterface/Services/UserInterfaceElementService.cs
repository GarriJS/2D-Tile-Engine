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
			return new UiPadding
			{
				TopPadding = model.TopPadding,
				BottomPadding = model.BottomPadding,
				LeftPadding = model.LeftPadding,
				RightPadding = model.RightPadding,
			};
		}

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel)
		{
			var imageService = this._gameServices.GetService<IImageService>();
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

			if (null != uiElementModel.Texture)
			{
				uiElement.Graphic = imageService.GetImageFromModel(uiElementModel.Texture);

				if ((true == uiElementModel.ResizeTexture) ||
					(uiElement.Graphic is FillImage))
				{
					var dimensions = new Vector2
					{
						X = uiElement.InsideWidth,
						Y = uiElement.InsideHeight
					};
					uiElement.Graphic.SetDrawDimensions(dimensions);
				}
			}

			uiElement.HorizontalSizeType = uiElementModel.HorizontalSizeType;
			uiElement.VerticalSizeType = uiElementModel.VerticalSizeType;
			uiElement.PressConfig = cursorInteractionService.GetPressConfiguration<IAmAUiElement>(area);
			uiElement.PressConfig.AddSubscription(this.CheckForUiElementClick);

			if ((uiElement is IAmAUiElementWithText uiElementWithText) &&
				(uiElementModel is IAmAUiElementWithTextModel uiElementWithTextModel))
			{
				uiElementWithText.GraphicText = graphicTextService.GetGraphicTextFromModel(uiElementWithTextModel.Text);
			}

			if (true == functionService.TryGetFunction<Action<IAmAUiElement, Vector2>>(uiElementModel.HoverCursorName, out var hoverAction))
			{
				uiElement.HoverConfig?.AddSubscription(hoverAction);
			}

			if (true == functionService.TryGetFunction<Action<IAmAUiElement, Vector2, Vector2>>(uiElementModel.PressEventName, out var pressAction))
			{
				uiElement.PressConfig?.AddSubscription(pressAction);
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
			element.Graphic.TextureBox = new Rectangle
			{
				X = element.Graphic.TextureBox.X,
				Y = element.Graphic.TextureBox.Y,
				Width = element.Graphic.TextureBox.Width,
				Height = (int)height
			};

			if (element is not UiButton uiButton)
			{
				return;
			}

			uiButton.ClickConfig.Area = new SubArea
			{
				Width = uiButton.Area.Width * uiButton.ClickableAreaScaler.X,
				Height = uiButton.Area.Height * uiButton.ClickableAreaScaler.Y
			};
			uiButton.ClickConfig.Offset = new Vector2
			{
				X = (uiButton.Area.Width - uiButton.ClickConfig.Area.Width) / 2,
				Y = (uiButton.Area.Height - uiButton.ClickConfig.Area.Height) / 2
			};

			if (null == uiButton.ClickAnimation?.Frames)
			{
				return;
			}

			foreach (var frame in uiButton.ClickAnimation.Frames)
			{
				frame.TextureBox = new Rectangle
				{
					X = frame.TextureBox.X,
					Y = frame.TextureBox.Y,
					Width = frame.TextureBox.Width,
					Height = (int)(element.Graphic.TextureBox.Height * uiButton.ClickableAreaScaler.Y)
				};
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

			Vector2 elementFitDimensions = default;

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
				UiElementSizeType.FitContent => elementFitDimensions.X,
				_ => 0
			};
			var uiElementVerticalSize = elementModel.VerticalSizeType switch
			{
				UiElementSizeType.ExtraSmall => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.ExtraSmall.Y,
				UiElementSizeType.Small => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.Small.Y,
				UiElementSizeType.Medium => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.Medium.Y,
				UiElementSizeType.Large => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.Large.Y,
				UiElementSizeType.ExtraLarge => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.ExtraLarge.Y,
				UiElementSizeType.FitContent => elementFitDimensions.Y,
				_ => 0
			};

			return new SubArea
			{
				Width = uiElementHorizontalSize,
				Height = uiElementVerticalSize
			};
		}

		/// <summary>
		/// Gets the element fit dimensions.
		/// </summary>
		/// <param name="elementModel">The element model.</param>
		/// <returns>The element fit dimensions.</returns>
		private Vector2 GetElementFitDimensions(IAmAUiElementModel elementModel)
		{
			Vector2? textDimensions = null;

			if (elementModel is IAmAUiElementWithTextModel elementTextModel)
			{
				var fontService = this._gameServices.GetService<IFontService>();

				if (false == string.IsNullOrEmpty(elementTextModel.Text?.FontName))
				{
					var font = fontService.GetSpriteFont(elementTextModel.Text.FontName);
					textDimensions = font.MeasureString(elementTextModel.Text.Text);
				}
			}

			switch (elementModel)
			{
				case UiButtonModel uiButton:

					if ((false == textDimensions.HasValue) &&
						(null == uiButton.ClickableAreaAnimation))
					{
						break;
					}

					var restingFrame = uiButton.ClickableAreaAnimation?.Frames[uiButton.ClickableAreaAnimation.RestingFrameIndex];
					var width = 0;
					var height = 0;

					if (null != restingFrame)
					{
						width = restingFrame.TextureBox.Width;
						height = restingFrame.TextureBox.Height;
					}

					if (true == textDimensions.HasValue)
					{
						width = (int)Math.Max(textDimensions.Value.X, width);
						height = (int)Math.Max(textDimensions.Value.Y, height);
					}

					return new Vector2
					{
						X = width,
						Y = height
					};

				case UiTextModel uiText:

					if (true == textDimensions.HasValue)
					{
						return textDimensions.Value;
					}

					break;
			}

			return new Vector2
			{
				X = -1,
				Y = -1
			};
		}

		/// <summary>
		/// Checks the user interface element for a click.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		/// <param name="pressLocation">The press location.</param>
		public void CheckForUiElementClick(IAmAUiElement element, Vector2 elementLocation, Vector2 pressLocation)
		{
			//TODO this should possibly use inside width and height

			switch (element)
			{
				case UiButton button:

					var clickableLocation = new Vector2
					{
						X = elementLocation.X + ((element.Area.Width - button.ClickConfig.Area.Width) / 2),
						Y = elementLocation.Y + ((element.Area.Height - button.ClickConfig.Area.Height) / 2)
					};

					if ((clickableLocation.X <= pressLocation.X) &&
						(clickableLocation.X + button.ClickConfig.Area.Width >= pressLocation.X) &&
						(clickableLocation.Y <= pressLocation.Y) &&
						(clickableLocation.Y + button.ClickConfig.Area.Height >= pressLocation.Y))
					{
						button.RaiseClickEvent(elementLocation);
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
			var clickConfig = cursorInteractionService.GetClickConfiguration<IAmAUiElement>(clickableArea, clickableOffset);
			var button = new UiButton
			{
				UiElementName = buttonModel.UiElementName,
				GraphicText = graphicText,
				ClickableAreaScaler = buttonModel.ClickableAreaScaler,
				ClickConfig = clickConfig
			};

			button.ClickConfig?.AddSubscription(this.TriggerUiButtonClickAnimation);

			// LOGGING
			if (true == functionService.TryGetFunction<Action<IAmAUiElement, Vector2>>(buttonModel.ClickEventName, out var clickAction))
			{
				button.ClickConfig?.AddSubscription(clickAction);
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
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		private void TriggerUiButtonClickAnimation(IAmAUiElement element, Vector2 elementLocation)
		{
			if (element is UiButton button)
			{
				button.ClickAnimation?.TriggerAnimation(allowReset: true);
			}
		}
	}
}
