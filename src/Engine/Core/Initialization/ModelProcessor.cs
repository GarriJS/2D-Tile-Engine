using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Core.Initialization
{
	/// <summary>
	/// Represents a model processor
	/// </summary>
	public static class ModelProcessor
	{
		/// <summary>
		/// Gets or sets the model processing mappings.
		/// </summary>
		public static Dictionary<Type, Delegate> ModelProcessingMappings { get; set; } = [];

		/// <summary>
		/// Processes the initial models.
		/// </summary>
		/// <param name="initialModels">The initial models.</param>
		internal static void ProcessInitialModels(IList<object> initialModels)
		{
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
					continue;
				}

				var result = func.DynamicInvoke(model);
			}
		}
	}
}
