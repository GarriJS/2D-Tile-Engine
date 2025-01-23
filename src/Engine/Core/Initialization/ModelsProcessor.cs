using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Core.Initialization
{
	public static class ModelsProcessor
	{
		/// <summary>
		/// Gets or sets the model type mappings.
		/// </summary>
		public static Dictionary<Type, Type> ModelTypeMappings { get; set; } = [];

		/// <summary>
		/// Gets or sets the model processing mappings.
		/// </summary>
		public static Dictionary<Type, Delegate> ModelProcessingMappings { get; set; } = [];

		internal static void ProcessInitialModels(IList<object> initialModels)
		{
			if (true != initialModels?.Any())
			{
				return;
			}

			foreach (var model in initialModels)
			{
				switch (model)
				{ 
					
				
				}
			}


		}
	}
}
