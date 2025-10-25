using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Enum;
using Engine.Physics.Models.SubAreas;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents a texture region.
	/// </summary>
	public class TextureRegion : ICanBeSerialized<TextureRegionModel>
	{
		/// <summary>
		/// Gets or sets the texture region type.
		/// </summary>
		public TextureRegionType TextureRegionType { get; set; }

		/// <summary>
		/// Gets or sets the texture area.
		/// </summary>
		public Rectangle TextureArea { get; set; }

		/// <summary>
		/// Gets or sets the display area.
		/// </summary>
		public SubArea DisplayArea { get; set; }

		/// <summary>
		/// Draws the texture region.
		/// </summary>
		/// <param name="drawingService">The drawing service.</param>
		/// <param name="texture">The texture.</param>
		/// <param name="drawCoordinates">The draw coordinates.</param>
		public void Draw(IDrawingService drawingService, Texture2D texture, Vector2 drawCoordinates)
		{
			switch (this.TextureRegionType)
			{
				case TextureRegionType.Tile:
					this.DrawTiledTextureRegion(drawingService, texture, drawCoordinates);
					break;
				default:
					this.DrawFilledTextureRegion(drawingService, texture, drawCoordinates);
					break;
			}
		}

		/// <summary>
		/// Draws the filled texture region.
		/// </summary>
		/// <param name="drawingService">The drawing service.</param>
		/// <param name="texture">The texture.</param>
		/// <param name="drawCoordinates">The draw coordinates.</param>
		private void DrawFilledTextureRegion(IDrawingService drawingService, Texture2D texture, Vector2 drawCoordinates)
		{
			drawingService.Draw(texture, drawCoordinates, this.TextureArea, this.DisplayArea.ToVector, Color.White);
		}

		/// <summary>
		/// Draws the tiled texture region.
		/// </summary>
		/// <param name="drawingService">The drawing service.</param>
		/// <param name="texture">The texture.</param>
		/// <param name="drawCoordinates">The draw coordinates.</param>
		private void DrawTiledTextureRegion(IDrawingService drawingService, Texture2D texture, Vector2 drawCoordinates)
		{
			var horizontalRepeats = (int)(this.DisplayArea.Width / this.TextureArea.Width);
			var verticalRepeats = (int)(this.DisplayArea.Height / this.TextureArea.Height);
			var remainderX = (int)(this.DisplayArea.Width % this.TextureArea.Width);
			var remainderY = (int)(this.DisplayArea.Height % this.TextureArea.Height);

			for (int x = 0; x < horizontalRepeats; x++)
			{
				for (int y = 0; y < verticalRepeats; y++)
				{
					var repeatOffset = new Vector2 { X = x * this.TextureArea.Width, Y = y * this.TextureArea.Height };
					drawingService.Draw(texture, repeatOffset + drawCoordinates, this.TextureArea, Color.White);
				}
			}

			if (0 < remainderX)
			{
				var remainderBox = new Rectangle { X = this.TextureArea.X, Y = this.TextureArea.Y, Width = remainderX, Height = this.TextureArea.Height };

				for (int y = 0; y < verticalRepeats; y++)
				{
					var repeatOffset = new Vector2 { X = horizontalRepeats * this.TextureArea.Width, Y = y * this.TextureArea.Height };
					drawingService.Draw(texture, repeatOffset + drawCoordinates, remainderBox, Color.White);
				}
			}

			if (0 < remainderY)
			{
				var remainderBox = new Rectangle { X = this.TextureArea.X, Y = this.TextureArea.Y, Width = this.TextureArea.Width, Height = remainderY };

				for (int x = 0; x < horizontalRepeats; x++)
				{
					var repeatOffset = new Vector2 { X = x * this.TextureArea.Width, Y = verticalRepeats * this.TextureArea.Height };
					drawingService.Draw(texture, repeatOffset + drawCoordinates, remainderBox, Color.White);
				}
			}

			if ((0 < remainderX) &&
				(0 < remainderY))
			{
				var remainderBox = new Rectangle { X = this.TextureArea.X, Y = this.TextureArea.Y, Width = remainderX, Height = remainderY };
				var repeatOffset = new Vector2 { X = horizontalRepeats * this.TextureArea.Width, Y = verticalRepeats * this.TextureArea.Height };
				drawingService.Draw(texture, repeatOffset + drawCoordinates, remainderBox, Color.White);
			}
		}


		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public TextureRegionModel ToModel()
		{
			var subAreaModel = this.DisplayArea.ToModel();
			var result = new TextureRegionModel
			{
				TextureRegionType = this.TextureRegionType,
				TextureArea = this.TextureArea,
				DisplayArea = subAreaModel
			};

			return result;
		}
	}
}
