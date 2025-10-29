using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Core.Initialization.Services
{
	/// <summary>
	/// Represents a model processor
	/// </summary>
	static public class ModelProcessor
	{
		/// <summary>
		/// Gets or sets the model processing mappings.
		/// </summary>
		static public Dictionary<Type, Delegate> ModelProcessingMappings { get; set; } = [];

		/// <summary>
		/// Processes the initial models.
		/// </summary>
		/// <param name="initialModelsProvider">The initial models provider.</param>
		/// <param name="gameService">The game services.</param>
		static public void ProcessInitialModels(Func<GameServiceContainer, IList<object>> initialModelsProvider, GameServiceContainer gameService)
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

		/// <summary>
		/// Gets the model processors for the given type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>A dictionary of the processors for the type.</returns>
		static public Delegate GetModelProcessorsForType(Type type)
		{
			var result = ModelProcessingMappings.FirstOrDefault(e => e.Key == type);

			return result.Value;
		}
	}
}
