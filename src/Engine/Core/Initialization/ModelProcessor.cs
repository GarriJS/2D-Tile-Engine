using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Core.Initialization
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

			foreach (var model in initialModels ?? [])
			{
				if (model is null)
					continue;

				if (false == ModelProcessingMappings.TryGetValue(model.GetType(), out var func))
					continue;

				_ = func.DynamicInvoke(model);
			}
		}

		/// <summary>
		/// Tries to invokes the model.
		/// </summary>
		/// <param name="model">The initial model.</param>
		/// <param name="result">The result of invoking the model.</param>
		/// <returns>A value indicating whether the model processor was found for the provided model.</returns>
		public static bool TryInvokeModel(object model, out object result)
		{
			if (model is null)
			{
				result = null;

				return false;
			}

			var modelProcessor = GetModelProcessorsForType(model.GetType());

			if (modelProcessor is null)
			{
				result = null;

				return false;
			}

			result = modelProcessor.DynamicInvoke(model);

			return true;
		}

		/// <summary>
		/// Tries to invoke the model to the explicit type.
		/// </summary>
		/// <typeparam name="Tresult">The desired result type.</typeparam>
		/// <param name="model">The initial model.</param>
		/// <param name="result">The result of invoking the model..</param>
		/// <returns>A value indicating whether the model processor was found for the provided model to the desired type.</returns>
		public static bool TryInvokeModel<Tresult>(object model, out Tresult result)
		{
			if ((true == TryInvokeModel(model, out var typelessResult)) &&
				(typelessResult is Tresult typedResult))
			{
				result = typedResult;

				return true;
			}

			result = default;

			return false;
		}

		/// <summary>
		/// Tries to invoke the model to the explicit type or throws.
		/// </summary>
		/// <typeparam name="Tresult">The desired result type.</typeparam>
		/// <param name="model">The initial model.</param>
		/// <param name="result">The result of invoking the model..</param>
		/// <returns>A value indicating whether the model processor was found for the provided model to the desired type.</returns>
		/// <exception cref="Exception">Thrown when the desired result type can't be produced from the provided model.</exception>
		public static bool TryInvokeModelOrThrow<Tresult>(object model, out Tresult result)
		{
			if ((true == TryInvokeModel(model, out var typelessResult)) &&
				(typelessResult is Tresult typedResult))
			{
				result = typedResult;

				return true;
			}

			throw new Exception("Could not produced desired type from the provided model.");
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
