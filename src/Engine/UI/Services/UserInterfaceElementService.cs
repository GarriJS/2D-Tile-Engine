using Engine.Controls.Services.Contracts;
using Engine.DiskModels.UI.Contracts;
using Engine.DiskModels.UI.Elements;
using Engine.Drawables.Models;
using Engine.Drawables.Services.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Constants;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Elements;
using Engine.UI.Models.Enums;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
		/// Gets or sets the button click event processors.
		/// </summary>
		public Dictionary<string, Action<UiButton, Vector2>> ButtonClickEventProcessors { get; set; } = [];

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
			element.Image.TextureBox = new Rectangle(element.Image.TextureBox.X, element.Image.TextureBox.Y, element.Image.TextureBox.Width, (int)height);

			if (element is UiButton uiButton)
			{
				uiButton.ClickableArea = new Vector2(uiButton.ClickableArea.X, (int)(element.Image.TextureBox.Height * uiButton.ClickableAreaScaler.Y));

				if (true == uiButton.ClickAnimation?.Frames?.Any())
				{
					foreach (var frame in uiButton.ClickAnimation.Frames)
					{
						frame.TextureBox = new Rectangle(frame.TextureBox.X, frame.TextureBox.Y, frame.TextureBox.Width, (int)(element.Image.TextureBox.Height * uiButton.ClickableAreaScaler.Y));
					}
				}
			}
		}

		/// <summary>
		/// Processes the user interface element being pressed.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void ProcessUiElementPress(IAmAUiElement element, Vector2 elementLocation)
		{
			switch (element)
			{ 
				case UiButton button:
					var controlService = this._gameServices.GetService<IControlService>();
					var mouseLocation = controlService.ControlState.MouseState.Position;
					var clickableLocation = new Vector2(elementLocation.X + ((element.Area.X - button.ClickableArea.X) / 2), elementLocation.Y + ((element.Area.Y - button.ClickableArea.Y) / 2));

					if (clickableLocation.X <= mouseLocation.X &&
						clickableLocation.X + button.ClickableArea.X >= mouseLocation.X &&
						clickableLocation.Y <= mouseLocation.Y &&
						clickableLocation.Y + button.ClickableArea.Y >= mouseLocation.Y)
					{
						button.RaiseClickEvent(elementLocation);
					}

					break;
			}
		}

		/// <summary>
		/// Process the user interface button being clicked.
		/// </summary>
		/// <param name="button">The button.</param>
		/// <param name="elementLocation">The element location.</param>
		public void ProcessUiButtonClick(UiButton button, Vector2 elementLocation)
		{
			var uiService = this._gameServices.GetService<IUserInterfaceService>();
			uiService.ToggleUserInterfaceGroupVisibility(button.VisibilityGroup == 1 ? 2 : 1);

			if (null != button.ClickAnimation)
			{
				var animationService = this._gameServices.GetService<IAnimationService>();
				animationService.TriggerAnimation(button.ClickAnimation, true);
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
			var elementSize = this.GetElementDimensions(uiZone, uiElementModel);
			var width = true == elementSize.HasValue
						? elementSize.Value.X
						: fillWidth;
			var height = true == elementSize.HasValue
						 ? elementSize.Value.Y
						 : 0;

			var imageService = this._gameServices.GetService<IImageService>();
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
				uiElement.Image = image;
				uiElement.PressEvent += this.ProcessUiElementPress;
				this.UserInterfaceElements.Add(uiElement);
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
			return new UiText
			{
				UiElementName = textModel.UiElementName,
				Text = textModel.Text,
				LeftPadding = textModel.LeftPadding,
				RightPadding = textModel.RightPadding,
				ElementType = UiElementTypes.Button,
				Area = area,
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
			var clickableArea = new Vector2(area.X * buttonModel.ClickableAreaScaler.X, area.Y * buttonModel.ClickableAreaScaler.Y);
			var button =  new UiButton
			{
				UiElementName = buttonModel.UiElementName,
				Text = buttonModel.Text,
				LeftPadding = buttonModel.LeftPadding,
				RightPadding = buttonModel.RightPadding,
				ElementType = UiElementTypes.Button,
				Area = area,
				ClickableArea = clickableArea,
				ClickableAreaScaler = buttonModel.ClickableAreaScaler
			};

			button.ClickEvent += this.ProcessUiButtonClick;

			if ((false == string.IsNullOrEmpty(buttonModel.ButtonClickEventName)) &&
				(true == this.ButtonClickEventProcessors.TryGetValue(buttonModel.ButtonClickEventName, out var action)))
			{
				button.ClickEvent += action;
			}

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
