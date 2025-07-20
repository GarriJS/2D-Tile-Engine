using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.RunTime.Services
{
	/// <summary>
	/// Represents a drawing service.
	/// </summary>
	public class DrawingService : IDrawingService
	{
		private readonly GameServiceContainer _gameServices;

		/// <summary>
		/// Gets the sprite batch.
		/// </summary>
		public SpriteBatch SpriteBatch { get; }

		/// <summary>
		/// Initializes a new instance of the drawing service.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public DrawingService(GameServiceContainer gameServices)
		{
			this._gameServices = gameServices;
			var graphicDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			this.SpriteBatch = new SpriteBatch(graphicDeviceService.GraphicsDevice);
		}

		/// <summary>
		/// Begins the draw.
		/// </summary>
		public void BeginDraw()
		{
			this.SpriteBatch.Begin();
		}

		/// <summary>
		/// Ends the draw.
		/// </summary>
		public void EndDraw()
		{
			this.SpriteBatch.End();
		}

		/// <summary>
		/// Draws the texture. 
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="sourceRectangle">The source rectangle.</param>
		/// <param name="color">The color.</param>
		public void Draw(Texture2D texture, Vector2 coordinates, Rectangle sourceRectangle, Color color)
		{
			this.SpriteBatch.Draw(texture, coordinates, sourceRectangle, color);
		}

		/// <summary>
		/// Draws the sub drawable. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="graphic">The graphic.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, IAmAGraphic graphic, Position position, Vector2 offset = default)
		{
			this.SpriteBatch.Draw(graphic.Texture, position.Coordinates + offset, graphic.TextureBox, Color.White);
		}
	}
}
