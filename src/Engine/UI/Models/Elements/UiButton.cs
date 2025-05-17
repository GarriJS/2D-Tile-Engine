using Engine.Drawables.Models;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Enums;
using Microsoft.Xna.Framework;
using System;

namespace Engine.UI.Models.Elements
{
	/// <summary>
	/// Represents a user interface button.
	/// </summary>
	public class UiButton : IAmAUiElementWithText, ICanBeClicked<UiButton>
	{
		/// <summary>
		/// Gets or sets the user interface element name.
		/// </summary>
		public string UiElementName { get; set; }

		/// <summary>
		/// Gets or sets the visibility group.
		/// </summary>
		public int VisibilityGroup { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the left padding.
		/// </summary>
		public float LeftPadding { get; set; }

		/// <summary>
		/// Gets or sets the right padding.
		/// </summary>
		public float RightPadding { get; set; }

		/// <summary>
		/// Gets or sets the user interface element type.
		/// </summary>
		public UiElementTypes ElementType { get; set; }

		/// <summary>
		/// Gets or sets the user interface size type.
		/// </summary>
		public UiElementSizeTypes SizeType { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public Vector2 Area { get; set; }

		/// <summary>
		/// Gets or sets the clickable area.
		/// </summary>
		public Vector2 ClickableArea { get; set; }

		/// <summary>
		/// Gets or sets the clickable area scaler.
		/// </summary>
		public Vector2 ClickableAreaScaler { get; set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		public Image Image { get; set; }

		/// <summary>
		/// Gets or sets the clickable image.
		/// </summary>
		public Image ClickableImage { get => this.ClickAnimation?.CurrentFrame; }

		/// <summary>
		/// Gets or sets the clickable animation.
		/// </summary>
		public TriggeredAnimation ClickAnimation { get; set; }

		/// <summary>
		/// Gets or set the press event.
		/// </summary>
		public event Action<IAmAUiElement, Vector2> HoverEvent;

		/// <summary>
		/// Gets or set the press event.
		/// </summary>
		public event Action<IAmAUiElement, Vector2> PressEvent;

		/// <summary>
		/// Gets or sets the click event.
		/// </summary>
		public event Action<UiButton, Vector2> ClickEvent;

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaiseHoverEvent(Vector2 elementLocation)
		{ 
			this.HoverEvent?.Invoke(this, elementLocation);
		}

		/// <summary>
		/// Raises the press event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaisePressEvent(Vector2 elementLocation)
		{ 
			this.PressEvent?.Invoke(this, elementLocation);
		}

		/// <summary>
		/// Raises the click event.
		/// </summary>
		public void RaiseClickEvent(Vector2 elementLocation)
		{
			this.ClickEvent?.Invoke(this, elementLocation);
		}

		/// <summary>
		/// Disposes of the user interface button.
		/// </summary>
		public void Dispose()
		{ 
			this.Image?.Dispose();
		}
	}
}
