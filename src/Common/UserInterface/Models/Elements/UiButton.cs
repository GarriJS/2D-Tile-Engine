using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Abstract;
using Common.UserInterface.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.Elements
{
    /// <summary>
    /// Represents a user interface button.
    /// </summary>
    public class UiButton : UiElementBase, IHaveGraphicText, ICanBeClicked<IAmAUiElement>
	{
		/// <summary>
		/// Gets or sets the pressed text offset. 
		/// </summary>
		public Vector2 PressedTextOffset { get; set; } = new Vector2(0, 5); 

		/// <summary>
		/// Gets or sets the clickable area scaler.
		/// </summary>
		public Vector2 ClickableAreaScaler { get; set; }

		/// <summary>
		/// Gets or sets the graphic text.
		/// </summary>
		public IAmGraphicText GraphicText { get => this.SimpleText; }

		/// <summary>
		/// Gets or sets the simple text.
		/// </summary>
		public SimpleText SimpleText { get; set; }

		/// <summary>
		/// Gets or sets the clickable animation.
		/// </summary>
		public TriggeredAnimation ClickAnimation { get; set; }

		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseClickEvent(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			this.CursorConfiguration?.RaiseClickEvent(cursorInteraction);
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		override public void Draw(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			this.Graphic?.Draw(gameTime, gameServices, coordinates, color, offset);
			this.ClickAnimation?.Draw(gameTime, gameServices, coordinates, color, offset);

			if (null != this.GraphicText)
			{
				var textDimensions = this.GraphicText.GetTextDimensions();
				var centeredOffset = new Vector2
				{ 
					X = (this.Area.Width - textDimensions.X) / 2f,
					Y = (this.Area.Height - textDimensions.Y) / 2f
				};
				var animationOffset = Vector2.Zero;

				if (true == this.ClickAnimation?.AnimationIsTrigged)
					animationOffset = this.PressedTextOffset;

				var graphicTextOffset = offset + centeredOffset + animationOffset;
				this.GraphicText.Write(gameTime, gameServices, coordinates, graphicTextOffset);
			}
		}

		/// <summary>
		/// Disposes of the user interface button.
		/// </summary>
		override public void Dispose()
		{
			this.CursorConfiguration?.Dispose();
		}
	}
}
