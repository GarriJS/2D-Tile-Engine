using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Abstract;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

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
		/// Gets the dimensions.
		/// </summary>
		public SubArea Dimensions => new()
		{
			Width = this.TextureRegions[0].Sum(e => e.DisplayArea.Width),
			Height = this.TextureRegions.Sum(e => e[0].DisplayArea.Height)
		};

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// Gets or sets the texture regions.
		/// </summary>
		/// <remarks>Each row must be the same width, and each column must be the same height.</remarks>
		public TextureRegion[][] TextureRegions { get; set; }

		/// <summary>
		/// Sets the draw dimensions.
		/// </summary>
		/// <param name="dimensions">The dimensions.</param>
		public void SetDrawDimensions(SubArea dimensions)
		{
			var topCornersWidth = this.TextureRegions[0][0].DisplayArea.Width + this.TextureRegions[0][^1].DisplayArea.Width;
			var leftCornersHeight = this.TextureRegions[0][0].DisplayArea.Height + this.TextureRegions[^1][0].DisplayArea.Height;
			var middleSetWidth = dimensions.Width - topCornersWidth;
			var middleSetHeight = dimensions.Height - leftCornersHeight;

			if (0 >= middleSetWidth)
			{
				// LOGGING
				middleSetWidth = 0;
			}

			if (0 >= middleSetHeight)
			{
				// LOGGING
				middleSetHeight = 0;
			}

			for (int i = 0; i < this.TextureRegions.Length - 1; i++)
				for (int j = 1; j < this.TextureRegions[0].Length; j++)
					this.TextureRegions[i][j].DisplayArea.Width = middleSetWidth;

			for (int i = 1; i < this.TextureRegions[0].Length - 1; i++)
				for (int j = 0; j < this.TextureRegions.Length; j++)
					this.TextureRegions[i][j].DisplayArea.Height = middleSetHeight;
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();
			var drawCoordinates = coordinates + offset;
			var verticalRowOffset = 0f;

			foreach (var textureRegionRow in this.TextureRegions)
			{
				var horizontalRowOffset = 0f;

				foreach (var textureRegion in textureRegionRow)
				{
					var textureRegionOffset = new Vector2 
					{ 
						X = horizontalRowOffset,
						Y = verticalRowOffset 
					};
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
		virtual public GraphicBaseModel ToModel()
		{
			//var textureRegions = this.TextureRegions.Select(e => e.ToModel());

			var result = new CompositeImageModel
			{
				TextureName = this.TextureName,
				//TextureRegions = textureRegions TODO
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
