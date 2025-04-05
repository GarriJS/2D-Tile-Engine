using Engine.Core.Initialization;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Physics;
using Engine.DiskModels.UI;
using Engine.Drawables.Services.Contracts;
using Engine.Physics.Services.Contracts;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

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
				ModelProcessor.ModelProcessingMappings.Add(typeIn, processor);
			}
		}

		/// <summary>
		/// Gets the model processing mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The model processing mappings.</returns>
		private static (Type typeIn, Delegate)[] GetModelProcessingMappings(GameServiceContainer gameServices)
		{
			var spriteService = gameServices.GetService<ISpriteService>();
			var positionService = gameServices.GetService<IPositionService>();
			var uiService = gameServices.GetService<IUserInterfaceService>();

			return
			[
				(typeof(SpriteModel), spriteService.GetSprite),
				(typeof(PositionModel), positionService.GetPosition),
				(typeof(UiGroupModel), uiService.GetUiGroup)
			];
		}
	}
}
