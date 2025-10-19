using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents a fill image.
	/// </summary>
	public class FillImage : Image
	{
		/// <summary>
		/// Gets or sets the fill box.
		/// </summary>
		public Vector2 FillBox { get; set; }

		/// <summary>
		/// Sets the draw dimensions.
		/// </summary>
		/// <param name="dimensions">The dimensions.</param>
		override public void SetDrawDimensions(Vector2 dimensions)
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
		override public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();

			drawingService.Draw(this.Texture, position.Coordinates + offset, this.TextureBox, this.FillBox, Color.White);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		override public IAmAGraphicModel ToModel()
		{
			return new FillImageModel
			{
				TextureName = this.TextureName,
				TextureBox = this.TextureBox,
				FillBox = this.FillBox
			};
		}
	}
}
