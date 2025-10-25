using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents a texture region image.
	/// </summary>
	public class TextureRegionImage : Image
	{
		/// <summary>
		/// Gets or sets the texture region.
		/// </summary>
		public TextureRegion TextureRegion { get; set; }

		/// <summary>
		/// Sets the draw dimensions.
		/// </summary>
		/// <param name="dimensions">The dimensions.</param>
		override public void SetDrawDimensions(Vector2 dimensions)
		{

		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		override public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();

			var drawCoordinates = position.Coordinates + offset;
			this.TextureRegion.Draw(drawingService, this.Texture, drawCoordinates);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		override public IAmAGraphicModel ToModel()
		{
			var textureRegionModel = this.TextureRegion.ToModel();
			var result = new TextureRegionImageModel
			{
				TextureName = this.TextureName,
				TextureBox = this.TextureBox,
				TextureRegion = textureRegionModel
			};

			return result;
		}
	}
}
