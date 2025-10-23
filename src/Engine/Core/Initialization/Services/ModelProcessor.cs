using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Core.Initialization.Services
{
	/// <summary>
	/// Represents a model processor
	/// </summary>
	internal static class ModelProcessor
	{
		/// <summary>
		/// Gets or sets the model processing mappings.
		/// </summary>
		internal static Dictionary<Type, Delegate> ModelProcessingMappings { get; set; } = [];

		/// <summary>
		/// Processes the initial models.
		/// </summary>
		/// <param name="initialModelsProvider">The initial models provider.</param>
		/// <param name="gameService">The game services.</param>
		internal static void ProcessInitialModels(Func<GameServiceContainer, IList<object>> initialModelsProvider, GameServiceContainer gameService)
		{
			var initialModels = initialModelsProvider?.Invoke(gameService);

			if (true != initialModels?.Any())
			{
				return;
			}

			foreach (var model in initialModels)
			{
				if (null == model)
				{ 
					continue;
				}

				if (false == ModelProcessingMappings.TryGetValue(model.GetType(), out var func))
				{
					// LOGGING
					continue;
				}

				_ = func.DynamicInvoke(model);
			}
		}
	}
}
