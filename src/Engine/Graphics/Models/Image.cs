using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Contracts;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents a image.
	/// </summary>
	public class Image : IAmAGraphic
	{
		protected Rectangle _textureBox;

		/// <summary>
		/// Gets or sets the texture name.
		/// </summary>
		public string TextureName { get; set; }

		/// <summary>
		/// Gets or sets the texture box.
		/// </summary>
		public Rectangle TextureBox { get => this._textureBox; set => this._textureBox = value; }

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// Gets the graphic.
		/// </summary>
		public IAmAGraphic Graphic { get => this; }

		/// <summary>
		/// Sets the draw dimensions.
		/// </summary>
		/// <param name="dimensions">The dimensions.</param>
		virtual public void SetDrawDimensions(Vector2 dimensions)
		{ 
			this._textureBox.Width = (int)dimensions.X;
			this._textureBox.Height = (int)dimensions.Y;
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		virtual public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();

			drawingService.Draw(this, position, offset);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		virtual public IAmAGraphicModel ToModel()
		{
			return new ImageModel
			{
				TextureName = this.TextureName,
				TextureBox = this.TextureBox
			};
		}

		/// <summary>
		/// Disposes of the draw data texture.
		/// </summary>
		public void Dispose()
		{
			this.Texture?.Dispose();
		}
	}
}
