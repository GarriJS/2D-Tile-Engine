using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents a independent graphic.
	/// </summary>
	public class IndependentGraphic : IHaveAGraphic, IAmDrawable, ICanBeSerialized<IndependentGraphicModel>
	{
		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets the graphic.
		/// </summary>
		public IAmAGraphic Graphic { get; set; }

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			this.Graphic.Draw(gameTime, gameServices, this.Position, Color.White);
		}

		/// <summary>
		/// Disposes of the draw data texture.
		/// </summary>
		public void Dispose()
		{
			this.Graphic?.Dispose();
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public IndependentGraphicModel ToModel()
		{ 
			var positionModel = this.Position.ToModel();
			var graphicModel = this.Graphic.ToModel();

			return new IndependentGraphicModel
			{
				Position = positionModel,
				Graphic = graphicModel
			};
		}
	}
}
