using Engine.Controls.Models;
using Engine.Controls.Services.Contracts;
using Engine.Core.Initialization.Services;
using Engine.DiskModels.Controls;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Abstract;
using Engine.DiskModels.Physics;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.SubAreas;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Engine.DiskModels
{
	/// <summary>
	/// Represents a model mappings.
	/// </summary>
	static internal class ModelMapper
	{
		/// <summary>
		/// Loads the engine model type mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		static internal void LoadEngineModelProcessingMappings(GameServiceContainer gameServices)
		{
			LoadModelProcessingMappings(gameServices, GetModelProcessingMappings);
		}

		/// <summary>
		/// Loads the model processing mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		static internal void LoadModelProcessingMappings(GameServiceContainer gameServices, Func<GameServiceContainer, (Type typeIn, Delegate)[]> modelProcessorMapProvider)
		{
			var modelProcessingMappings = modelProcessorMapProvider.Invoke(gameServices);

			foreach (var (typeIn, processor) in modelProcessingMappings)
			{ 
				ModelProcessor.ModelProcessingMappings.Add(typeIn, processor);
			}
		}

		/// <summary>
		/// Gets the model processing mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The model processing mappings.</returns>
		static private (Type type, Delegate factory)[] GetModelProcessingMappings(GameServiceContainer gameServices)
		{
			var positionService = gameServices.GetService<IPositionService>();
			var areaService = gameServices.GetService<IAreaService>();
			var graphicService = gameServices.GetService<IGraphicService>();
			var imageService = gameServices.GetService<IImageService>();
			var independentGraphicService = gameServices.GetService<IIndependentGraphicService>();
			var graphicTextService = gameServices.GetService<IGraphicTextService>();
			var animationService = gameServices.GetService<IAnimationService>();
			var actionControlService = gameServices.GetService<IActionControlServices>();

			(Type type, Delegate factory)[] result =
			[
				(typeof(PositionModel), positionService.GetPositionFromModel),
				(typeof(AreaModel), areaService.GetAreaFromModel<SimpleArea>),
				(typeof(OffsetAreaModel), areaService.GetAreaFromModel<OffsetArea>),
				(typeof(AreaCollectionModel), areaService.GetAreaFromModel<AreaCollection>),
				(typeof(SubAreaModel), areaService.GetSubAreaFromModel),
				(typeof(OffsetSubAreaModel), areaService.GetOffSetSubAreaFromModel),
				(typeof(GraphicBaseModel), graphicService.GetGraphicFromModel),
				(typeof(TextureRegionModel), graphicService.GetTextureRegionFromModel),
				(typeof(SimpleImageModel), imageService.GetImageFromModel<SimpleImage>),
				(typeof(CompositeImageModel), imageService.GetImageFromModel<CompositeImage>),
				(typeof(IndependentGraphicModel), independentGraphicService.GetIndependentGraphicFromModel),
				(typeof(GraphicalTextModel), graphicTextService.GetGraphicTextFromModel),
				(typeof(AnimationModel), animationService.GetAnimationFromModel),
				(typeof(TriggeredAnimationModel), animationService.GetAnimationFromModel),
				(typeof(ActionControlModel), actionControlService.GetActionControlFromModel),
			];

			return result;
		}
	}
}
