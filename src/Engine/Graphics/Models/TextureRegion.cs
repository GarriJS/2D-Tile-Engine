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
	sealed public class TextureRegion : ICanBeSerialized<TextureRegionModel>
	{
		/// <summary>
		/// Gets or sets the texture region type.
		/// </summary>
		required public TextureRegionType TextureRegionType { get; set; }

		/// <summary>
		/// Gets or sets the texture box.
		/// </summary>
		required public Rectangle TextureBox { get; set; }

		/// <summary>
		/// Gets or sets the display area.
		/// </summary>
		required public SubArea DisplayArea { get; set; }

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
				case TextureRegionType.Fill:
					this.DrawFilledTextureRegion(drawingService, texture, drawCoordinates);
					break;
				default:
					drawingService.Draw(texture, drawCoordinates, this.TextureBox, Color.White);
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
			drawingService.Draw(texture, drawCoordinates, this.TextureBox, this.DisplayArea.ToVector, Color.White);
		}

		/// <summary>
		/// Draws the tiled texture region.
		/// </summary>
		/// <param name="drawingService">The drawing service.</param>
		/// <param name="texture">The texture.</param>
		/// <param name="drawCoordinates">The draw coordinates.</param>
		private void DrawTiledTextureRegion(IDrawingService drawingService, Texture2D texture, Vector2 drawCoordinates)
		{
			var horizontalRepeats = (int)(this.DisplayArea.Width / this.TextureBox.Width);
			var verticalRepeats = (int)(this.DisplayArea.Height / this.TextureBox.Height);
			var remainderX = (int)(this.DisplayArea.Width % this.TextureBox.Width);
			var remainderY = (int)(this.DisplayArea.Height % this.TextureBox.Height);

			for (int x = 0; x < horizontalRepeats; x++)
				for (int y = 0; y < verticalRepeats; y++)
				{
					var repeatOffset = new Vector2 { X = x * this.TextureBox.Width, Y = y * this.TextureBox.Height };
					drawingService.Draw(texture, repeatOffset + drawCoordinates, this.TextureBox, Color.White);
				}

			if (0 < remainderX)
			{
				var remainderBox = new Rectangle { X = this.TextureBox.X, Y = this.TextureBox.Y, Width = remainderX, Height = this.TextureBox.Height };

				for (int y = 0; y < verticalRepeats; y++)
				{
					var repeatOffset = new Vector2 { X = horizontalRepeats * this.TextureBox.Width, Y = y * this.TextureBox.Height };
					drawingService.Draw(texture, repeatOffset + drawCoordinates, remainderBox, Color.White);
				}
			}

			if (0 < remainderY)
			{
				var remainderBox = new Rectangle { X = this.TextureBox.X, Y = this.TextureBox.Y, Width = this.TextureBox.Width, Height = remainderY };

				for (int x = 0; x < horizontalRepeats; x++)
				{
					var repeatOffset = new Vector2 { X = x * this.TextureBox.Width, Y = verticalRepeats * this.TextureBox.Height };
					drawingService.Draw(texture, repeatOffset + drawCoordinates, remainderBox, Color.White);
				}
			}

			if ((0 < remainderX) &&
				(0 < remainderY))
			{
				var remainderBox = new Rectangle { X = this.TextureBox.X, Y = this.TextureBox.Y, Width = remainderX, Height = remainderY };
				var repeatOffset = new Vector2 { X = horizontalRepeats * this.TextureBox.Width, Y = verticalRepeats * this.TextureBox.Height };
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
				TextureBox = this.TextureBox,
				DisplayArea = subAreaModel
			};

			return result;
		}
	}
}
