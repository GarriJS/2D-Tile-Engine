using Engine.Controls.Typing;
using Engine.Controls.Typing.Models;
using Engine.Controls.Typing.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents writable text.
	/// </summary>
	sealed public class WritableText : SimpleText, IHaveATextEditingState
	{
		/// <summary>
		/// Gets or sets a value indicating whether the text is being edited.
		/// </summary>
		required public bool TextIsBeingEdited { get; set; }

		/// <summary>
		/// Gets or sets the text editing state.
		/// </summary>
		required public TextEditingState TextEditingState { get; set; }

		/// <summary>
		/// Updates the text.
		/// </summary>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <param name="conformText">A value indicating whether to conform the text.</param>
		/// <returns>The dimensions of the next text.</returns>
		public void UpdateText(List<Keys> freshKeys, List<ElaspedTimeExtender<Keys>> pressedKeys, bool conformText = true)
		{
			var typingResult = KeyboardTyping.ModifyTextFromKeys(this.Text, this.TextEditingState.Copy(), freshKeys, pressedKeys);

			if (this.MaxLineCharacterCount < typingResult.Text.Length)
				return;
			
			this.Text = typingResult.Text;
			this.TextEditingState = typingResult.TextEditingState;
		}

		/// <summary>
		/// Writes the sub writable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		override public void Write(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Vector2 offset = default)
		{
			if (true == string.IsNullOrEmpty(this.Text))
				return;

			var writingService = gameServices.GetService<IWritingService>();
			var textOffset = coordinates + offset;
			writingService.Draw(this.Font, this.Text, textOffset, this.TextColor);

			if (true == this.TextIsBeingEdited)
				this.DrawTextEditingState(gameTime, gameServices, coordinates, offset);
		}

		/// <summary>
		/// Draws the text editing state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		private void DrawTextEditingState(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Vector2 offset = default)
		{
			var textDimensions = this.GetTextDimensions();
			var textEditingStateOffset = offset + new Vector2
			{
				X = textDimensions.X,
				Y = (textDimensions.Y - this.TextEditingState.TypingCursor.Area.Height) / 2
			};
			this.TextEditingState.UpdateTextEditorOffsets(this.Text, this.Font, coordinates, textEditingStateOffset);
			this.TextEditingState.Draw(gameTime, gameServices, coordinates, Color.White, textEditingStateOffset);
		}
	}
}
