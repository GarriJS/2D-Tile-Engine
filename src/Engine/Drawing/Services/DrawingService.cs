using Game.Drawing.Models.Contracts;
using Game.Drawing.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Drawing.Services
{
	/// <summary>
	/// Represents a drawing service.
	/// </summary>
	public class DrawingService : IDrawingService
    {
		private readonly GameServiceContainer _gameServiceContainer;
        
		/// <summary>
		/// Gets the sprite batch.
		/// </summary>
		private SpriteBatch SpriteBatch { get; }

		/// <summary>
		/// Initializes a new instance of the drawing service.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
        public DrawingService(GameServiceContainer gameServices)
		{
			this._gameServiceContainer = gameServices;
			var graphicDeviceService = this._gameServiceContainer.GetService<IGraphicsDeviceService>();
			this.SpriteBatch = new SpriteBatch(graphicDeviceService.GraphicsDevice);
		}

		/// <summary>
		/// Draws the draw data. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawable">The drawable.</param>
		public void Draw(GameTime gameTime, ICanBeDrawn drawable)
		{
			this.SpriteBatch.Draw(drawable.Sprite.Texture, drawable.Position.Coordinates, drawable.Sprite.TextureBox, Color.White);
		}
	}
}
