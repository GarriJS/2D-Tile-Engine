using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents a tiled image. 
	/// </summary>
	public class TiledImage : Image
	{
		/// <summary>
		/// Gets or sets the fill box.
		/// </summary>
		public Vector2 FillBox { get; set; }

		/// <summary>
		/// Sets the draw dimensions.
		/// </summary>
		/// <param name="dimensions">The dimensions.</param>
		public override void SetDrawDimensions(Vector2 dimensions)
		{
			this.FillBox = dimensions;
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public override void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();

			var horizontalRepeats = (int)this.FillBox.X / this.TextureBox.Width;
			var verticalRepeats = (int)this.FillBox.Y / this.TextureBox.Height;

			for (int x = 0; x < horizontalRepeats; x++)
			{
				for (int y = 0; y < verticalRepeats; y++)
				{
					var repeatOffset = new Vector2
					{
						X = x * this.TextureBox.Width,
						Y = y * this.TextureBox.Height,	
					};
					drawingService.Draw(this, position, repeatOffset + offset);
				}
			}

			var remainderX = (int)this.FillBox.X % this.TextureBox.Width;
			var remainderY = (int)this.FillBox.Y % this.TextureBox.Height;

			if (remainderX > 0)
			{
				for (int y = 0; y < verticalRepeats; y++)
				{
					var repeatOffset = new Vector2
					{
						X = horizontalRepeats * this.TextureBox.Width,
						Y = y * this.TextureBox.Height,
					};
					var repeatTextureBox = new Rectangle
					{
						X = this.TextureBox.X,
						Y = this.TextureBox.Y, 
						Width = remainderX,
						Height = this.TextureBox.Height 
					};
					drawingService.Draw(this.Texture, repeatOffset + offset, repeatTextureBox);
				}
			}

			if (remainderY > 0)
			{
				for (int x = 0; x < horizontalRepeats; x++)
				{
					var repeatOffset = new Vector2
					{
						X = x * this.TextureBox.Width,
						Y= verticalRepeats * this.TextureBox.Height
					};
					var repeatTextureBox = new Rectangle
					{
						X = this.TextureBox.X,
						Y = this.TextureBox.Y,
						Width = this.TextureBox.Width,
						Height = remainderY
					};
					drawingService.Draw(this.Texture, repeatOffset + offset, repeatTextureBox);
				}
			}

			if ((remainderX > 0) && 
				(remainderY > 0))
			{
				var repeatOffset = new Vector2
				{
					X = horizontalRepeats * this.TextureBox.Width,
					Y = verticalRepeats * this.TextureBox.Height
				};
				var repeatTextureBox = new Rectangle
				{
					X = this.TextureBox.X,
					Y = this.TextureBox.Y,
					Width = remainderX,
					Height = remainderY
				};
				drawingService.Draw(this.Texture, repeatOffset + offset, repeatTextureBox);
			}
		}
	}
}
