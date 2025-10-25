using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents a image by parts.
	/// </summary>
	public class ImageByParts : Image
	{
		/// <summary>
		/// Gets or sets the texture regions.
		/// </summary>
		/// <remarks>The rows and columns must each have the same height and width.</remarks>
		public TextureRegion[][] TextureRegions { get; set; }

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
	}
}
