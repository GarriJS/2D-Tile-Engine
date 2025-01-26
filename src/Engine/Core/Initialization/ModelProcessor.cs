using Engine.DiskModels.Engine.UI;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Core.Initialization
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

		internal static void ProcessInitialUiModels(IList<UiGroupModel> initialUiModels, GameServiceContainer gameServices)
		{
			if (true != initialUiModels?.Any())
			{
				return;
			}

			var uiService = gameServices.GetService<IUserInterfaceService>();

			foreach (var model in initialUiModels)
			{ 
				var uiGroup = uiService.GetUiGroup(model);

				if (null != uiGroup)
				{ 
					uiService.UserInterfaceGroups.Add(uiGroup);
				}

				if (true == model.IsVisible)
				{
					uiService.ToggleUserInterfaceGroupVisibility(uiGroup.VisibilityGroupId);	
				}
			}
		}
	}
}
