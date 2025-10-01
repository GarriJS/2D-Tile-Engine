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
using Engine.Core.Fonts.Contracts;
using Engine.Core.Initialization.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Services.Contracts;
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

			if ((null == uiScreenZone?.Area) ||
				(false == elementModel.SizeType.HasValue))
			{
				return new Vector2
				{
					X = -1,
					Y = -1
				};
			}

			var uiElementSizeType = Enum.IsDefined(typeof(UiElementSizeTypes), elementModel.SizeType)
									? (UiElementSizeTypes)elementModel.SizeType
									: UiElementSizeTypes.None;

			return uiElementSizeType switch
			{
				UiElementSizeTypes.None => null,
				UiElementSizeTypes.ExtraSmall => new Vector2
				{
					X = uiScreenZone.Area.Width * ElementSizesScalars.ExtraSmall.X,
					Y = uiScreenZone.Area.Height * ElementSizesScalars.ExtraSmall.Y
				},
				UiElementSizeTypes.Small => new Vector2
				{
					X = uiScreenZone.Area.Width * ElementSizesScalars.Small.X,
					Y = uiScreenZone.Area.Height * ElementSizesScalars.Small.Y
				},
				UiElementSizeTypes.Medium => new Vector2
				{
					X = uiScreenZone.Area.Width * ElementSizesScalars.Medium.X,
					Y = uiScreenZone.Area.Height * ElementSizesScalars.Medium.Y
				},
				UiElementSizeTypes.Large => new Vector2
				{
					X = uiScreenZone.Area.Width * ElementSizesScalars.Large.X,
					Y = uiScreenZone.Area.Height * ElementSizesScalars.Large.Y
				},
				UiElementSizeTypes.ExtraLarge => new Vector2
				{
					X = uiScreenZone.Area.Width * ElementSizesScalars.ExtraLarge.X,
					Y = uiScreenZone.Area.Height * ElementSizesScalars.ExtraLarge.Y
				},
				UiElementSizeTypes.Full => new Vector2
				{
					X = uiScreenZone.Area.Width,
					Y = uiScreenZone.Area.Height
				},
				UiElementSizeTypes.Fit => this.GetElementFitDimensions(elementModel),
				_ => new Vector2
				{
					X = -1,
					Y = -1,
				},
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
					int width = 0;
					int height = 0;

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
		/// Updates the element height.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="height">The height.</param>
		public void UpdateElementHeight(IAmAUiElement element, float height)
		{
			element.Area = new Vector2(element.Area.X, height);
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

			uiButton.ClickConfig.Area = new Vector2
			{
				X = uiButton.Area.X * uiButton.ClickableAreaScaler.X,
				Y = uiButton.Area.Y * uiButton.ClickableAreaScaler.Y
			};
			uiButton.ClickConfig.Offset = new Vector2
			{
				X = (uiButton.Area.X - uiButton.ClickConfig.Area.X) / 2,
				Y = (uiButton.Area.Y - uiButton.ClickConfig.Area.Y) / 2
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

					var clickableLocation = new Vector2
					{
						X = elementLocation.X + ((element.Area.X - button.ClickConfig.Area.X) / 2),
						Y = elementLocation.Y + ((element.Area.Y - button.ClickConfig.Area.Y) / 2)
					};

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
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel, UiScreenZone uiZone, float fillWidth)
		{
			var imageService = this._gameServices.GetService<IImageService>();
			var functionService = this._gameServices.GetService<IFunctionService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();

			var elementSize = this.GetElementDimensions(uiZone, uiElementModel);
			var width = true == elementSize.HasValue
				? elementSize.Value.X
				: fillWidth;
			var height = true == elementSize.HasValue
				? elementSize.Value.Y
				: 0;

			Image background = null;

			if (null != uiElementModel.Texture)
			{
				background = imageService.GetImageFromModel(uiElementModel.Texture);

				if ((true == uiElementModel.ResizeTexture) ||
					(background is FillImage))
				{
					var textureWidth = width + uiElementModel.InsidePadding.LeftPadding + uiElementModel.InsidePadding.RightPadding;
					var dimensions = new Vector2(textureWidth, height);
					background.SetDrawDimensions(dimensions);
				}
			}

			var area = new Vector2(width, height);
			IAmAUiElement uiElement = uiElementModel switch
			{
				UiTextModel textModel => this.GetUiText(textModel, area),
				UiButtonModel buttonModel => this.GetUiButton(buttonModel, area),
				_ => null,
			};

			if (null != uiElement)
			{
				uiElement.Graphic = background;
				uiElement.PressConfig?.AddSubscription(this.CheckForUiElementClick);

				// LOGGING
				if (true == functionService.TryGetFunction<Action<IAmAUiElement, Vector2>>(uiElementModel.HoverCursorName, out var hoverAction))
				{
					uiElement.HoverConfig?.AddSubscription(hoverAction);
				}

				// LOGGING
				if (true == functionService.TryGetFunction<Action<IAmAUiElement, Vector2, Vector2>>(uiElementModel.pressEventName, out var pressAction))
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
			var graphicTextService = this._gameServices.GetService<IGraphicTextService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();

			var graphicText = graphicTextService.GetGraphicTextFromModel(textModel.Text);
			var hoverConfig = cursorInteractionService.GetHoverConfiguration<IAmAUiElement>(area, textModel.HoverCursorName);
			var pressConfig = cursorInteractionService.GetPressConfiguration<IAmAUiElement>(area);

			return new UiText
			{
				UiElementName = textModel.UiElementName,
				GraphicText = graphicText,
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
			var graphicTextService = this._gameServices.GetService<IGraphicTextService>();
			var animationService = this._gameServices.GetService<IAnimationService>();
			var functionService = this._gameServices.GetService<IFunctionService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();

			var graphicText = graphicTextService.GetGraphicTextFromModel(buttonModel.Text);
			var clickableArea = new Vector2
			{
				X = area.X * buttonModel.ClickableAreaScaler.X,
				Y = area.Y * buttonModel.ClickableAreaScaler.Y
			};
			var clickableOffset = new Vector2
			{
				X = (area.X - clickableArea.X) / 2,
				Y = (area.Y - clickableArea.Y) / 2
			};
			var hoverConfig = cursorInteractionService.GetHoverConfiguration<IAmAUiElement>(area, buttonModel.HoverCursorName);
			var pressConfig = cursorInteractionService.GetPressConfiguration<IAmAUiElement>(area);
			var clickConfig = cursorInteractionService.GetClickConfiguration<IAmAUiElement>(clickableArea, clickableOffset);
			var button = new UiButton
			{
				UiElementName = buttonModel.UiElementName,
				GraphicText = graphicText,
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

			// LOGGING
			if (true == functionService.TryGetFunction<Action<IAmAUiElement, Vector2>>(buttonModel.ClickEventName, out var clickAction))
			{
				button.ClickConfig?.AddSubscription(clickAction);
			}

			if (null != buttonModel.ClickableAreaAnimation)
			{
				var clickAnimation = animationService.GetFixedAnimationFromModel(buttonModel.ClickableAreaAnimation, (int)clickableArea.X, (int)clickableArea.Y);

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
