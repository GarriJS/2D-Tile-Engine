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
			this.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
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
		/// <param name="drawCoordinates">The draw drawCoordinates.</param>
		/// <param name="sourceRectangle">The source rectangle.</param>
		/// <param name="color">The color.</param>
		public void Draw(Texture2D texture, Vector2 drawCoordinates, Rectangle sourceRectangle, Color color)
		{
			this.SpriteBatch.Draw(texture, drawCoordinates, sourceRectangle, color);
		}

		/// <summary>
		/// Draws the texture. 
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="drawCoordinates">The draw coordinates.</param>
		/// <param name="sourceRectangle">The source rectangle.</param>
		/// <param name="stretchBox">The stretch box.</param>
		/// <param name="color">The color.</param>
		public void Draw(Texture2D texture, Vector2 drawCoordinates, Rectangle sourceRectangle, Vector2 stretchBox, Color color)
		{
			var destinationRectangle = new Rectangle
			{
				X = (int)drawCoordinates.X,
				Y = (int)drawCoordinates.Y,
				Width = (int)stretchBox.X,
				Height = (int)stretchBox.Y
			};

			this.SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
		}
	}
}
