using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.UserInterface;
using Common.DiskModels.UserInterface.Contracts;
using Common.DiskModels.UserInterface.Elements;
using Common.UserInterface.Constants;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Models.Elements;
using Common.UserInterface.Services.Contracts;
using Engine.Core.Fonts.Services.Contracts;
using Engine.Core.Initialization.Services.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a user interface element service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface element service.
	/// </remarks>
	/// <param name="gameServices">The game service.</param>
	sealed public class UiElementService(GameServiceContainer gameServices) : IUiElementService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

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
			var cursorService = this._gameServices.GetService<ICursorService>();
			var baseMembers = this.GetBaseElementMembers(uiElementModel);
			IAmAUiElement uiElement = uiElementModel switch
			{
				UiTextModel uiTextModel => this.GetUiText(uiTextModel, baseMembers),
				UiButtonModel uiButtonModel => this.GetUiButton(uiButtonModel, baseMembers),
				_ => null,
			};

			if (uiElement is null)
			{
				// LOGGING

				return null;
			}

			if (uiElement is ICanBeClicked<IAmAUiElement> clickableElement)
				uiElement.CursorConfiguration.AddPressSubscription(this.CheckForUiElementClick);

			if (true == functionService.TryGetFunction<Action<CursorInteraction<IAmAUiElement>>>(uiElementModel.HoverEventName, out var hoverAction))
				uiElement.CursorConfiguration.AddHoverSubscription(hoverAction);

			if (true == functionService.TryGetFunction<Action<CursorInteraction<IAmAUiElement>>>(uiElementModel.PressEventName, out var pressAction))
				uiElement.CursorConfiguration.AddPressSubscription(pressAction);

			return uiElement;
		}

		/// <summary>
		/// Gets the base user interface element members.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <returns>A dictionary binding the member name to the member.</returns>
		private Dictionary<string, object> GetBaseElementMembers(IAmAUiElementModel uiElementModel)
		{ 
			if (uiElementModel is null)
				return null;

			var graphicService = this._gameServices.GetService<IGraphicService>();
			var cursorService = this._gameServices.GetService<ICursorService>();
			var uiMarginService = this._gameServices.GetService<IUiMarginService>();
			var result = new Dictionary<string, object>();
			var area = this.GetElementArea(uiElementModel);
			result["area"] = area;
			var margin = uiMarginService.GetUiMarginFromModel(uiElementModel.Margin);
			result["margin"] = margin;

			if (uiElementModel.Graphic is not null)
			{
				var graphic = graphicService.GetGraphicFromModel(uiElementModel.Graphic);

				if ((true == uiElementModel.ResizeTexture) ||
					(graphic is CompositeImage compositeImage))
				{
					var dimensions = new SubArea
					{
						Width = area.Width,
						Height = area.Height
					};
					graphic.SetDrawDimensions(dimensions);
				}

				result["graphic"] = graphic;
			}

			if ((false == string.IsNullOrEmpty(uiElementModel.HoverCursorName)) &&
				(true == cursorService.Cursors.TryGetValue(uiElementModel.HoverCursorName, out var hoverCursor)))
				result["hoverCursor"] = hoverCursor;

			return result;
		}

		/// <summary>
		/// Gets the user interface text.
		/// </summary>
		/// <param name="uiTextModel">The user interface text model.</param>
		/// <param name="baseElements">The base elements.</param>
		/// <returns>The user interface text.</returns>
		private UiText GetUiText(UiTextModel uiTextModel, Dictionary<string, object> baseElements)
		{
			var graphicTextService = this._gameServices.GetService<IGraphicTextService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var area = baseElements.GetValueOrDefault("area") as SubArea;
			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<IAmAUiElement>(area, null);
			var simpleText = graphicTextService.GetGraphicTextFromModel(uiTextModel.Text);
			var uiText = new UiText
			{
				Name = uiTextModel.Name,
				HorizontalSizeType = uiTextModel.HorizontalSizeType,
				VerticalSizeType = uiTextModel.VerticalSizeType,
				Area = area,
				Margin = baseElements.GetValueOrDefault("margin") as UiMargin? ?? default,
				Graphic = baseElements.GetValueOrDefault("graphic") as IAmAGraphic,
				HoverCursor = baseElements.GetValueOrDefault("hoverCursor") as Cursor,
				CursorConfiguration = cursorConfiguration,
				SimpleText = simpleText
			};

			return uiText;
		}

		/// <summary>
		/// Gets the user interface uiText.
		/// </summary>
		/// <param name="uiButtonModel">The user interface uiText model.</param>
		/// <param name="baseElements">The base elements.</param>
		/// <returns>The user interface uiText.</returns>
		private UiButton GetUiButton(UiButtonModel uiButtonModel, Dictionary<string, object> baseElements)
		{
			var graphicTextService = this._gameServices.GetService<IGraphicTextService>();
			var animationService = this._gameServices.GetService<IAnimationService>();
			var functionService = this._gameServices.GetService<IFunctionService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var area = baseElements.GetValueOrDefault("area") as SubArea;
			var simpleText = graphicTextService.GetGraphicTextFromModel(uiButtonModel.Text);
			var clickableArea = new SubArea
			{
				Width = area.Width * uiButtonModel.ClickableAreaScaler.X,
				Height = area.Height * uiButtonModel.ClickableAreaScaler.Y
			};
			var clickableOffset = new Vector2
			{
				X = (area.Width - clickableArea.Width) / 2,
				Y = (area.Height - clickableArea.Height) / 2
			};
			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<IAmAUiElement>(area, clickableArea, clickOffset: clickableOffset);

			TriggeredAnimation clickAnimation = null;
			if (uiButtonModel.ClickableAreaAnimation is not null)
			{
				var animation = animationService.GetFixedAnimationFromModel(uiButtonModel.ClickableAreaAnimation, (int)clickableArea.Width, (int)clickableArea.Height);
			
				if (animation is TriggeredAnimation triggeredAnimation)
					clickAnimation = triggeredAnimation;
			}

			var uiButton = new UiButton
			{
				Name = uiButtonModel.Name,
				HorizontalSizeType = uiButtonModel.HorizontalSizeType,
				VerticalSizeType = uiButtonModel.VerticalSizeType,
				Area = area,
				Margin = baseElements.GetValueOrDefault("margin") as UiMargin? ?? default,
				Graphic = baseElements.GetValueOrDefault("graphic") as IAmAGraphic,
				HoverCursor = baseElements.GetValueOrDefault("hoverCursor") as Cursor,
				CursorConfiguration = cursorConfiguration,
				ClickableAreaScaler = uiButtonModel.ClickableAreaScaler,
				SimpleText = simpleText,
				ClickAnimation = clickAnimation
			};
			uiButton.CursorConfiguration?.AddClickSubscription(this.TriggerUiButtonClickAnimation);

			if (true == functionService.TryGetFunction<Action<CursorInteraction<IAmAUiElement>>>(uiButtonModel.ClickEventName, out var clickAction))
				uiButton.CursorConfiguration?.AddClickSubscription(clickAction);

			return uiButton;
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
				return;

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

			if (uiButton.ClickAnimation?.Frames is null)
				return;

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
			var uiScreenZoneService = this._gameServices.GetService<IUiScreenZoneService>();

			if (true == elementModel.FixedSized.HasValue)
			{
				var fixedSize = elementModel.FixedSized.Value;

				if (0 > fixedSize.X)
					fixedSize.X = 0;

				if (0 > fixedSize.Y)
					fixedSize.Y = 0;

				var fixedResult = new SubArea
				{
					Width = fixedSize.X,
					Height = fixedSize.Y,
				};

				return fixedResult;
			}

			if (uiScreenZoneService?.ScreenZoneSize is null)
				return default;

			SubArea elementFitDimensions = null;

			if ((UiElementSizeType.FlexMin == elementModel.HorizontalSizeType) ||
				(UiElementSizeType.FlexMin == elementModel.VerticalSizeType))
				elementFitDimensions = this.GetElementFitDimensions(elementModel);

			var uiElementHorizontalSize = elementModel.HorizontalSizeType switch
			{
				UiElementSizeType.ExtraSmall => uiScreenZoneService.ScreenZoneSize.Width * ElementSizesScalars.ExtraSmall.X,
				UiElementSizeType.Small => uiScreenZoneService.ScreenZoneSize.Width * ElementSizesScalars.Small.X,
				UiElementSizeType.Medium => uiScreenZoneService.ScreenZoneSize.Width * ElementSizesScalars.Medium.X,
				UiElementSizeType.Large => uiScreenZoneService.ScreenZoneSize.Width * ElementSizesScalars.Large.X,
				UiElementSizeType.ExtraLarge => uiScreenZoneService.ScreenZoneSize.Width * ElementSizesScalars.ExtraLarge.X,
				UiElementSizeType.FlexMin => elementFitDimensions.Width,
				_ => 0
			};
			var uiElementVerticalSize = elementModel.VerticalSizeType switch
			{
				UiElementSizeType.ExtraSmall => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.ExtraSmall.Y,
				UiElementSizeType.Small => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.Small.Y,
				UiElementSizeType.Medium => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.Medium.Y,
				UiElementSizeType.Large => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.Large.Y,
				UiElementSizeType.ExtraLarge => uiScreenZoneService.ScreenZoneSize.Height * ElementSizesScalars.ExtraLarge.Y,
				UiElementSizeType.FlexMin => elementFitDimensions.Height,
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

			if (elementModel.Graphic is not null)
				result = elementModel.Graphic.GetDimensions();

			switch (elementModel)
			{
				case UiButtonModel uiButtonModel:

					if (uiButtonModel.ClickableAreaAnimation is null)
						break;

					var restingFrame = uiButtonModel.ClickableAreaAnimation?.Frames[uiButtonModel.ClickableAreaAnimation.RestingFrameIndex];

					if (restingFrame is not null)
						result = restingFrame.GetDimensions();

					break;
			}

			if ((elementModel is IAmAUiElementWithTextModel elementTextModel) &&
				(elementTextModel.Text is not null))
			{
				var fontService = this._gameServices.GetService<IFontService>();

				if (false == string.IsNullOrEmpty(elementTextModel.Text.FontName))
				{
					var font = fontService.GetSpriteFont(elementTextModel.Text.FontName);
					var textDimensions = font.MeasureString(elementTextModel.Text.Text);
					result.Width = (int)Math.Max(
						textDimensions.X
						+ (elementTextModel.Margin?.LeftMargin ?? 0)
						+ (elementTextModel.Margin?.RightMargin ?? 0),
						width);
					result.Height = (int)Math.Max(
						textDimensions.Y
						+ (elementTextModel.Margin?.TopMargin ?? 0)
						+ (elementTextModel.Margin?.BottomMargin ?? 0),
						height);
				}

				if (elementTextModel.Text is GraphicalTextWithMarginModel graphicalTextWithMarginModel)
				{
					result.Width += (graphicalTextWithMarginModel.Margin?.LeftMargin ?? 0)
									+ (graphicalTextWithMarginModel.Margin?.RightMargin ?? 0);
					result.Height += (graphicalTextWithMarginModel.Margin?.TopMargin ?? 0)
									 + (graphicalTextWithMarginModel.Margin?.BottomMargin ?? 0);
				}
			}

			return result;
		}

		/// <summary>
		/// Checks the user interface element for a click.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		private void CheckForUiElementClick(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			switch (cursorInteraction.Subject)
			{
				case UiButton button:

					var clickableLocation = new Vector2
					{
						X = cursorInteraction.SubjectLocation.X + button.CursorConfiguration.ClickOffset.X + ((cursorInteraction.Subject.Area.Width - button.CursorConfiguration.ClickArea.Width) / 2),
						Y = cursorInteraction.SubjectLocation.Y + button.CursorConfiguration.ClickOffset.Y + ((cursorInteraction.Subject.Area.Height - button.CursorConfiguration.ClickArea.Height) / 2)
					};

					if ((clickableLocation.X <= cursorInteraction.CursorLocation.X) &&
						(clickableLocation.X + button.CursorConfiguration.ClickArea.Width >= cursorInteraction.CursorLocation.X) &&
						(clickableLocation.Y <= cursorInteraction.CursorLocation.Y) &&
						(clickableLocation.Y + button.CursorConfiguration.ClickArea.Height >= cursorInteraction.CursorLocation.Y))
						button.RaiseClickEvent(cursorInteraction);

					break;
			}
		}

		/// <summary>
		/// Triggers the user interface elements click animation.
		/// </summary>
		/// <param name="cursorInteraction">The element with location.</param>
		private void TriggerUiButtonClickAnimation(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			if (cursorInteraction.Subject is UiButton button)
				button.ClickAnimation?.TriggerAnimation(allowReset: true);
		}
	}
}
