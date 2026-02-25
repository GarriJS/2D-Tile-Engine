using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Models.Elements.Abstract;
using Engine.Controls.Models;
using Engine.Controls.Models.Contracts;
using Engine.Controls.Typing;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Common.UserInterface.Models.Elements
{
	/// <summary>
	/// Represents user interface writable text.
	/// </summary>
	sealed public class UiWritableText : UiElementBase, IHaveGraphicText, ICanBeClicked<IAmAUiElement>, IAmAControlContextComponent
	{
		/// <summary>
		/// Gets or sets the clickable area scaler.
		/// </summary>
		required public Vector2 ClickableAreaScaler { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user interface writable text is being written to.
		/// </summary>
		required public bool Active { get; set; }

		/// <summary>
		/// Gets the graphic text.
		/// </summary>
		public IAmGraphicText GraphicText { get => this.WritableText; }

		/// <summary>
		/// Gets or sets the writable text.
		/// </summary>
		required public WritableText WritableText { get; set; }

		/// <summary>
		/// Gets or sets the active graphic.
		/// </summary>
		required public IAmAGraphic ActiveGraphic { get; set; }

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
			var graphicOffset = offset + this.CachedOffset ?? default;
			this.Graphic?.Draw(gameTime, gameServices, coordinates, color, graphicOffset);

			if (this.GraphicText is not null)
			{
				var textDimensions = this.GraphicText.GetTextDimensions();
				var centeredOffset = new Vector2
				{
					X = (this.Area.Width - textDimensions.X) / 2f,
					Y = (this.Area.Height - textDimensions.Y) / 2f
				};
				var graphicTextOffset = graphicOffset + centeredOffset;
				this.GraphicText.Write(gameTime, gameServices, coordinates, graphicTextOffset);
			}
		}

		/// <summary>
		/// Consumes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		public void ConsumeControlState(GameTime gameTime, ControlState controlState, ControlState priorControlState)
		{
			string newText = string.Empty;
			var shiftIsPressed = true == controlState.PressedKeys.Any(e => e == Keys.LeftShift || e == Keys.RightShift);
			var backSpaceIsPressed = true == controlState.FreshPressedKeys.Any(e => e == Keys.Back || e == Keys.Delete);

			foreach (var key in controlState.FreshPressedKeys)
			{
				if (key != Keys.Back &&
					key != Keys.Delete)
					newText += KeyboardTyping.ToChar(key, shiftIsPressed);
			}

			this.WritableText.UpdateText(this.WritableText.Text + newText);

			if (backSpaceIsPressed &&
				this.WritableText.Text.Length > 0)
				this.WritableText.UpdateText(this.WritableText.Text[..^1]);
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
