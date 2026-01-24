using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Abstract;
using Common.UserInterface.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.Elements
{
	/// <summary>
	/// Represents user interface editable text.
	/// </summary>
	public class UiEditableText : UiElementBase, IHaveGraphicText, ICanBeClicked<IAmAUiElement>
	{
		/// <summary>
		/// Gets or sets the clickable area scaler.
		/// </summary>
		public Vector2 ClickableAreaScaler { get; set; }

		/// <summary>
		/// Gets the graphic text.
		/// </summary>
		public IAmGraphicText GraphicText { get => this.WritableText; }

		/// <summary>
		/// Gets or sets the writable text.
		/// </summary>
		public WritableText WritableText { get; set; }

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
		/// <param name="position">The position.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		override public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Color color, Vector2 offset = default)
		{
			this.Graphic?.Draw(gameTime, gameServices, position, color, offset);

			if (null != this.GraphicText)
			{
				var textDimensions = this.GraphicText.GetTextDimensions();
				var centeredOffset = new Vector2
				{
					X = (this.Area.Width - textDimensions.X) / 2f,
					Y = (this.Area.Height - textDimensions.Y) / 2f
				};
				var graphicTextOffset = offset + centeredOffset;
				this.GraphicText.Write(gameTime, gameServices, position, graphicTextOffset);
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
