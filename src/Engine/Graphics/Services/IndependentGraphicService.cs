using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.Services
{
	/// <summary>
	/// Represents a independent graphic service.
	/// </summary>
	/// <remarks>
	/// Initializes the independent graphic service.
	/// </remarks>
	/// <param name="gameService">The game services.</param>
	public class IndependentGraphicService(GameServiceContainer gameService) : IIndependentGraphicService
	{
		private readonly GameServiceContainer _gameServices = gameService;

		/// <summary>
		/// Gets the independent graphic from the image.
		/// </summary>
		/// <param name="independentGraphicModel">The independent graphic model.</param>
		/// <param name="drawLayer">The draw layer.</param>
		/// <returns>The independent graphic.</returns>
		public IndependentGraphic GetIndependentGraphicFromModel(IndependentGraphicModel independentGraphicModel, int drawLayer = 0)
		{
			var positionService = this._gameServices.GetService<IPositionService>();

			IAmAGraphic graphic = null;

			if (independentGraphicModel.Graphic is AnimationModel animationModel)
			{
				var animationService = this._gameServices.GetService<IAnimationService>();

				graphic = animationService.GetAnimationFromModel(animationModel);
			}
			else if (independentGraphicModel.Graphic is SimpleImageModel imageModel)
			{ 
				var imageService = this._gameServices.GetService<IImageService>();

				graphic = imageService.GetImageFromModel(imageModel);	
			}

			var position = positionService.GetPositionFromModel(independentGraphicModel.Position);

			return new IndependentGraphic
			{
				DrawLayer = drawLayer,
				Position = position,
				Graphic = graphic
			};
		}
	}
}
