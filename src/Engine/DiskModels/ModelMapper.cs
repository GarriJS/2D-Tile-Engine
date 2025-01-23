using Engine.Core.Initialization;
using Engine.DiskModels.Engine.Drawing;
using Engine.DiskModels.Engine.Physics;
using Engine.DiskModels.Engine.UI;
using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Services.Contracts;
using Engine.UI.Models;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Engine.DiskModels
{
	/// <summary>
	/// Represents a model mappings.
	/// </summary>
	internal static class ModelMapper
	{
		/// <summary>
		/// Loads the engine model type mappings.
		/// </summary>
		internal static void LoadEngineModelTypeMappings()
		{
			LoadModelTypeMappings(GetModelTypeMappings);
		}

		/// <summary>
		/// Loads the model type mappings.
		/// </summary>
		internal static void LoadModelTypeMappings(Func<(Type typeIn, Type typeOut)[]> modelTypeMapProvider)
		{
			var modelTypeMappings = modelTypeMapProvider?.Invoke();

			if (true != modelTypeMappings?.Any())
			{
				return;
			}

			foreach (var (typeIn, typeOut) in modelTypeMappings)
			{
				ModelsProcessor.ModelTypeMappings.Add(typeIn, typeOut);
			}
		}

		/// <summary>
		/// Loads the engine model type mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		internal static void LoadEngineModelProcessingMappings(GameServiceContainer gameServices)
		{
			LoadModelProcessingMappings(gameServices, GetModelProcessingMappings);
		}

		/// <summary>
		/// Loads the model processing mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		internal static void LoadModelProcessingMappings(GameServiceContainer gameServices, Func<GameServiceContainer, (Type typeIn, Delegate)[]> modelProcessorMapProvider)
		{
			var modelProcessingMappings = modelProcessorMapProvider.Invoke(gameServices);

			foreach (var (typeIn, processor) in modelProcessingMappings)
			{ 
				ModelsProcessor.ModelProcessingMappings.Add(typeIn, processor);
			}
		}

		/// <summary>
		/// Gets the model type mappings.
		/// </summary>
		/// <returns>The model type mappings.</returns>
		private static (Type typeIn, Type typeOut)[] GetModelTypeMappings()
		{
			return
			[
				(typeof(SpriteModel), typeof(Sprite)),
				(typeof(AreaCollectionModel), typeof(AreaCollection)),
				(typeof(OffsetAreaModel), typeof(OffsetArea)),
				(typeof(SimpleAreaModel), typeof(SimpleArea)),
				(typeof(PositionModel), typeof(Position)),
				(typeof(UiGroupModel), typeof(UiGroup)),
			];
		}

		/// <summary>
		/// Gets the model processing mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The model processing mappings.</returns>
		private static (Type typeIn, Delegate)[] GetModelProcessingMappings(GameServiceContainer gameServices)
		{
			var animationService = gameServices.GetService<IAnimationService>();
			var spriteService = gameServices.GetService<ISpriteService>();
			var areaService = gameServices.GetService<IAreaService>();
			var positionService = gameServices.GetService<IPositionService>();
			var uiService = gameServices.GetService<IUserInterfaceService>();

			return
			[
				(typeof(SpriteModel), spriteService.GetSprite),
				(typeof(AreaCollectionModel), areaService.GetArea<AreaCollection>),
				(typeof(OffsetAreaModel), areaService.GetArea<OffsetArea>),
				(typeof(SimpleAreaModel), areaService.GetArea<SimpleArea>),
				(typeof(PositionModel), positionService.GetPosition),
				(typeof(UiGroupModel), uiService.GetUiGroup)
			];
		}
	}
}
