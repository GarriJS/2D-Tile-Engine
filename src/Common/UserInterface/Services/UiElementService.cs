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
				UiWritableTextModel uiWritableTextModel => this.GetUiWritableText(uiWritableTextModel, baseMembers),
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
			var simpleText = graphicTextService.GetSimpleTextFromModel(uiTextModel.Text);
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
		/// Gets the user interface writable text.
		/// </summary>
		/// <param name="uiWritableTextModel">The user interface writable text model.</param>
		/// <param name="baseElements">The base elements.</param>
		/// <returns>The user interface writable text.</returns>
		private UiWritableText GetUiWritableText(UiWritableTextModel uiWritableTextModel, Dictionary<string, object> baseElements)
		{
			var graphicTextService = this._gameServices.GetService<IGraphicTextService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var area = baseElements.GetValueOrDefault("area") as SubArea;
			var clickableArea = new SubArea
			{
				Width = area.Width * uiWritableTextModel.ClickableAreaScaler.X,
				Height = area.Height * uiWritableTextModel.ClickableAreaScaler.Y
			};
			var clickableOffset = new Vector2
			{
				X = (area.Width - clickableArea.Width) / 2,
				Y = (area.Height - clickableArea.Height) / 2
			};
			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<IAmAUiElement>(area, clickableArea, clickOffset: clickableOffset);
			var writableText = graphicTextService.GetWritableTextFromModel(uiWritableTextModel.Text);
			var uiWritableText = new UiWritableText
			{
				Name = uiWritableTextModel.Name,
				HorizontalSizeType = uiWritableTextModel.HorizontalSizeType,
				VerticalSizeType = uiWritableTextModel.VerticalSizeType,
				Area = area,
				Margin = baseElements.GetValueOrDefault("margin") as UiMargin? ?? default,
				Graphic = baseElements.GetValueOrDefault("graphic") as IAmAGraphic,
				HoverCursor = baseElements.GetValueOrDefault("hoverCursor") as Cursor,
				CursorConfiguration = cursorConfiguration,
				ClickableAreaScaler = uiWritableTextModel.ClickableAreaScaler,
				WritableText = writableText,
				ClickAnimation = null
			};
			uiWritableText.CursorConfiguration?.AddClickSubscription(this.TriggerUiWriting);

			return uiWritableText;
		}

		/// <summary>
		/// Gets the user interface uiWritableText.
		/// </summary>
		/// <param name="uiButtonModel">The user interface uiWritableText model.</param>
		/// <param name="baseElements">The base elements.</param>
		/// <returns>The user interface uiWritableText.</returns>
		private UiButton GetUiButton(UiButtonModel uiButtonModel, Dictionary<string, object> baseElements)
		{
			var graphicTextService = this._gameServices.GetService<IGraphicTextService>();
			var animationService = this._gameServices.GetService<IAnimationService>();
			var functionService = this._gameServices.GetService<IFunctionService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var area = baseElements.GetValueOrDefault("area") as SubArea;
			var simpleText = graphicTextService.GetSimpleTextFromModel(uiButtonModel.Text);
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
		/// <param name="width">The width.</param>
		public void UpdateElementWidth(IAmAUiElement element, float width)
		{
			element.Area.Width = width;
			element.Graphic.SetDrawDimensions(element.Area);

			if (element is not ICanBeClicked<IAmAUiElement> clickable)
				return;

			clickable.CursorConfiguration.Area = new SubArea
			{
				Width = clickable.Area.Width * clickable.ClickableAreaScaler.X,
				Height = clickable.Area.Height * clickable.ClickableAreaScaler.Y
			};
			clickable.CursorConfiguration.ClickOffset = new Vector2
			{
				X = (clickable.Area.Width - clickable.CursorConfiguration.ClickArea.Width) / 2,
				Y = (clickable.Area.Height - clickable.CursorConfiguration.ClickArea.Height) / 2
			};

			//TODO updates press and hover areas?

			if (clickable.ClickAnimation?.Frames is null)
				return;

			foreach (var frame in clickable.ClickAnimation.Frames)
			{
				var subArea = new SubArea
				{
					Width = (int)(element.Graphic.Dimensions.Width * clickable.ClickableAreaScaler.X),
					Height = frame.Dimensions.Width
				};

				frame.SetDrawDimensions(subArea);
			}
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

			if (element is not ICanBeClicked<IAmAUiElement> clickable)
				return;

			clickable.CursorConfiguration.Area = new SubArea
			{
				Width = clickable.Area.Width * clickable.ClickableAreaScaler.X,
				Height = clickable.Area.Height * clickable.ClickableAreaScaler.Y
			};
			clickable.CursorConfiguration.ClickOffset = new Vector2
			{
				X = (clickable.Area.Width - clickable.CursorConfiguration.ClickArea.Width) / 2,
				Y = (clickable.Area.Height - clickable.CursorConfiguration.ClickArea.Height) / 2
			};

			//TODO updates press and hover areas?

			if (clickable.ClickAnimation?.Frames is null)
				return;

			foreach (var frame in clickable.ClickAnimation.Frames)
			{
				var subArea = new SubArea
				{
					Width = frame.Dimensions.Width,
					Height = (int)(element.Graphic.Dimensions.Height * clickable.ClickableAreaScaler.Y)
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
			var uiScreenService = this._gameServices.GetService<IUiScreenService>();

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

			if (uiScreenService?.ScreenZoneSize is null)
				return default;

			SubArea elementFitDimensions = null;

			if ((UiElementSizeType.FitContent == elementModel.HorizontalSizeType) ||
				(UiElementSizeType.FitContent == elementModel.VerticalSizeType))
				elementFitDimensions = this.GetElementFitDimensions(elementModel);

			var uiElementHorizontalSize = elementModel.HorizontalSizeType switch
			{
				UiElementSizeType.ExtraSmall => uiScreenService.ScreenZoneSize.Width * ElementSizesScalars.ExtraSmall.X,
				UiElementSizeType.Small => uiScreenService.ScreenZoneSize.Width * ElementSizesScalars.Small.X,
				UiElementSizeType.Medium => uiScreenService.ScreenZoneSize.Width * ElementSizesScalars.Medium.X,
				UiElementSizeType.Large => uiScreenService.ScreenZoneSize.Width * ElementSizesScalars.Large.X,
				UiElementSizeType.ExtraLarge => uiScreenService.ScreenZoneSize.Width * ElementSizesScalars.ExtraLarge.X,
				UiElementSizeType.FitContent => elementFitDimensions.Width,
				_ => 0
			};
			var uiElementVerticalSize = elementModel.VerticalSizeType switch
			{
				UiElementSizeType.ExtraSmall => uiScreenService.ScreenZoneSize.Height * ElementSizesScalars.ExtraSmall.Y,
				UiElementSizeType.Small => uiScreenService.ScreenZoneSize.Height * ElementSizesScalars.Small.Y,
				UiElementSizeType.Medium => uiScreenService.ScreenZoneSize.Height * ElementSizesScalars.Medium.Y,
				UiElementSizeType.Large => uiScreenService.ScreenZoneSize.Height * ElementSizesScalars.Large.Y,
				UiElementSizeType.ExtraLarge => uiScreenService.ScreenZoneSize.Height * ElementSizesScalars.ExtraLarge.Y,
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
			var fontService = this._gameServices.GetService<IFontService>();
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
				case UiTextModel textModel:

					if (textModel.Text is null)
						break;

					if (false == string.IsNullOrEmpty(textModel.Text.FontName))
					{
						var font = fontService.GetSpriteFont(textModel.Text.FontName);
						var textDimensions = font.MeasureString(textModel.Text.Text);
						result.Width = (int)Math.Max(
							textDimensions.X
							+ (textModel.Margin?.LeftMargin ?? 0)
							+ (textModel.Margin?.RightMargin ?? 0),
							width);
						result.Height = (int)Math.Max(
							textDimensions.Y
							+ (textModel.Margin?.TopMargin ?? 0)
							+ (textModel.Margin?.BottomMargin ?? 0),
							height);
					}

					if (textModel.Text is SinmpleTextWithMarginModel graphicalTextWithMarginModel)
					{
						result.Width += (graphicalTextWithMarginModel.Margin?.LeftMargin ?? 0)
										+ (graphicalTextWithMarginModel.Margin?.RightMargin ?? 0);
						result.Height += (graphicalTextWithMarginModel.Margin?.TopMargin ?? 0)
										 + (graphicalTextWithMarginModel.Margin?.BottomMargin ?? 0);
					}

					break;

				case UiWritableTextModel writableTextModel:

					if (writableTextModel.Text is null)
						break;

					if (false == string.IsNullOrEmpty(writableTextModel.Text.FontName))
					{
						var font = fontService.GetSpriteFont(writableTextModel.Text.FontName);
						var textDimensions = font.MeasureString(writableTextModel.Text.Text);
						result.Width = (int)Math.Max(
							textDimensions.X
							+ (writableTextModel.Margin?.LeftMargin ?? 0)
							+ (writableTextModel.Margin?.RightMargin ?? 0),
							width);
						result.Height = (int)Math.Max(
							textDimensions.Y
							+ (writableTextModel.Margin?.TopMargin ?? 0)
							+ (writableTextModel.Margin?.BottomMargin ?? 0),
							height);
					}

					break;

				case UiButtonModel uiButtonModel:

					if (uiButtonModel.ClickableAreaAnimation is null)
						break;

					var restingFrame = uiButtonModel.ClickableAreaAnimation?.Frames[uiButtonModel.ClickableAreaAnimation.RestingFrameIndex];

					if (restingFrame is not null)
						result = restingFrame.GetDimensions();

					break;
			}

			return result;
		}

		/// <summary>
		/// Checks the user interface element for a click.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		private void CheckForUiElementClick(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			var clickableLocation = new Vector2
			{
				X = cursorInteraction.SubjectLocation.X + cursorInteraction.Subject.CursorConfiguration.ClickOffset.X + ((cursorInteraction.Subject.Area.Width - cursorInteraction.Subject.CursorConfiguration.ClickArea.Width) / 2),
				Y = cursorInteraction.SubjectLocation.Y + cursorInteraction.Subject.CursorConfiguration.ClickOffset.Y + ((cursorInteraction.Subject.Area.Height - cursorInteraction.Subject.CursorConfiguration.ClickArea.Height) / 2)
			};

			switch (cursorInteraction.Subject)
			{
				case UiWritableText writableText:

					if ((clickableLocation.X <= cursorInteraction.CursorLocation.X) &&
						(clickableLocation.X + writableText.CursorConfiguration.ClickArea.Width >= cursorInteraction.CursorLocation.X) &&
						(clickableLocation.Y <= cursorInteraction.CursorLocation.Y) &&
						(clickableLocation.Y + writableText.CursorConfiguration.ClickArea.Height >= cursorInteraction.CursorLocation.Y))
					writableText.RaiseClickEvent(cursorInteraction);

					break;

				case UiButton button:

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
		/// <param name="cursorInteraction">The cursor interaction.</param>
		private void TriggerUiButtonClickAnimation(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			if (cursorInteraction.Subject is UiButton button)
				button.ClickAnimation?.TriggerAnimation(allowReset: true);
		}

		/// <summary>
		/// Triggers the user interface writing.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		private void TriggerUiWriting(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			if (cursorInteraction.Subject is UiWritableText writableText)
				writableText.WritableText.UpdateText(writableText.WritableText.Text + "a");
		}
	}
}
