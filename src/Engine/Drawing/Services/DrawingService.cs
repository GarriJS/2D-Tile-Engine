using Engine.Drawing.Models.Contracts;
using Engine.Drawing.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing.Services
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
		private SpriteBatch SpriteBatch { get; }

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
		/// Draws the drawable. 
		/// </summary>
		public void Draw(Texture2D texture, Vector2 coordinates, Rectangle sourceRectangle, Color color)
		{
			this.SpriteBatch.Draw(texture, coordinates, sourceRectangle, color);
		}

		/// <summary>
		/// Draws the drawable. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawable">The drawable.</param>
		public void Draw(GameTime gameTime, IAmDrawable drawable)
		{
			this.SpriteBatch.Draw(drawable.Image.Texture, drawable.Position.Coordinates, drawable.Image.TextureBox, Color.White);
		}
	}
}
