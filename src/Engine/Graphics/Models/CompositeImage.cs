using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Contracts;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.SubAreas;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents a composite image.
	/// </summary>
	public class CompositeImage : IAmAImage
	{
		/// <summary>
		/// Gets or sets the texture name.
		/// </summary>
		public string TextureName { get; set; }

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// Gets or sets the texture regions.
		/// </summary>
		/// <remarks>The rows and columns must each have the same height and width.</remarks>
		public TextureRegion[][] TextureRegions { get; set; }

		/// <summary>
		/// Sets the draw dimensions.
		/// </summary>
		/// <param name="dimensions">The dimensions.</param>
		public void SetDrawDimensions(SubArea dimensions)
		{

		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();

			var drawCoordinates = position.Coordinates + offset;
			var verticalRowOffset = 0f;

			foreach (var textureRegionRow in this.TextureRegions)
			{
				var horizontalRowOffset = 0f;

				foreach (var textureRegion in textureRegionRow)
				{
					var textureRegionOffset = new Vector2 { X = verticalRowOffset, Y = horizontalRowOffset };
					textureRegion.Draw(drawingService, this.Texture, textureRegionOffset + drawCoordinates);
					horizontalRowOffset += textureRegion.DisplayArea.Width;
				}

				verticalRowOffset += textureRegionRow[0].DisplayArea.Height;
			}
		}


		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		virtual public IAmAGraphicModel ToModel()
		{
			var result = new SimpleImageModel
			{
				TextureName = this.TextureName,
				//TextureBox = this.TextureBox
			};

			return result;
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
