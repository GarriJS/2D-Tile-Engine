﻿using Engine.Core.Initialization.Services;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.Services
{
	/// <summary>
	/// Represents a graphic service.
	/// </summary>
	/// <param name="gameService"></param>
	public class GraphicService(GameServiceContainer gameService) : IGraphicService
	{
		private readonly GameServiceContainer _gameServices = gameService;

		/// <summary>
		/// Gets the graphic from the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>The graphic.</returns>
		public IAmAGraphic GetGraphicFromModel(IAmAGraphicModel model)
		{
			var modelType = model.GetType();

			if (modelType == typeof(IAmAGraphicModel))
			{
				// LOGGING

				return null;
			}

			var modelProcessor = ModelProcessor.GetModelProcessorsForType(model.GetType());
			var result = modelProcessor?.DynamicInvoke(model);

			if (null == result)
			{
				// LOGGIGN

				return null;
			}

			return result as IAmAGraphic;
		}

		/// <summary>
		/// Gets the texture region from the model.
		/// </summary>
		/// <param name="textureRegionModel">The texture region model.</param>
		/// <returns>The texture region.</returns>
		public TextureRegion GetTextureRegionFromModel(TextureRegionModel textureRegionModel)
		{
			var areaService = this._gameServices.GetService<IAreaService>();

			SubArea displayArea = null;

			if (null != textureRegionModel.DisplayArea)
			{
				displayArea = areaService.GetSubAreaFromModel(textureRegionModel.DisplayArea);
			}
			else
			{
				displayArea = new SubArea { Width = textureRegionModel.TextureBox.Width, Height = textureRegionModel.TextureBox.Height };
			}

			var result = new TextureRegion
			{
				TextureRegionType = textureRegionModel.TextureRegionType,
				TextureBox = textureRegionModel.TextureBox,
				DisplayArea = displayArea
			};

			return result;
		}
	}
}
