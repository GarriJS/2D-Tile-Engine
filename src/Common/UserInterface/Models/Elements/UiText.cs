using Common.UserInterface.Abstract;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.Elements
{
	/// <summary>
	/// Represents a user interface text.
	/// </summary>
	public class UiText : UiElementBase, IHaveGraphicText
	{
		/// <summary>
		/// Gets or sets the SimpleText text.
		/// </summary>
		public IAmGraphicText GraphicText { get => this.SimpleText; }

		/// <summary>
		/// Gets or sets the simple text.
		/// </summary>
		public SimpleText SimpleText { get; set; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		override public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			this.Graphic?.Draw(gameTime, gameServices, position, offset);
			
			if (null != this.GraphicText)
			{
				var textDimensions = this.GraphicText.GetTextDimensions();
				var centeredOffset = new Vector2
				{
					X = (this.Area.Width - textDimensions.X) / 2f,
					Y = (this.Area.Height - textDimensions.Y) / 2f
				};
				this.GraphicText.Draw(gameTime, gameServices, position, offset + centeredOffset);
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
