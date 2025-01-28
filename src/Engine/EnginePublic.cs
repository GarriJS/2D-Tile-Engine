using Engine.Core.Initialization;
using Engine.Core.Initialization.Models;
using Engine.DiskModels.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine
{
	/// <summary>
	/// Represent a engine.
	/// </summary>
	public partial class Engine
	{
		/// <summary>
		/// Sets the loading instructions.
		/// </summary>
		/// <param name="loadingInstructions">The loading instructions.</param>
		public void SetLoadingInstructions(LoadingInstructions loadingInstructions)
		{
			LoadingInstructionsContainer.LoadingInstructions = loadingInstructions;
		}

		/// <summary>
		/// Adds the external services provider.
		/// </summary>
		/// <param name="externalServiceProvider">The external service provider.</param>
		public void AddExternalServiceProvider(Func<Game, (Type type, object provider)[]> externalServiceProvider)
		{
			this.ExternalServiceProviders.Add(externalServiceProvider);
		}

		/// <summary>
		/// Adds the external model processor map provider.
		/// </summary>
		/// <param name="externalModelProcessorMapProvider">The external model processor map provider.</param>
		public void AddModelProcessingMapProvider(Func<GameServiceContainer, (Type typeIn, Delegate)[]> externalModelProcessorMapProvider)
		{
			this.ExternalModelProcessorMapProviders.Add(externalModelProcessorMapProvider);
		}

		/// <summary>
		/// Adds the initial models.
		/// </summary>
		/// <param name="initialModelsProvider">The initial models provider.</param>
		public void AddInitialModels(Func<GameServiceContainer, IList<object>> initialModelsProvider)
		{ 
			this.InitialModelsProviders.Add(initialModelsProvider);
		}

		/// <summary>
		/// Sets the initial user interface.
		/// </summary>
		/// <param name="initialUiModelsProvider">The initial user interface models provider.</param>
		public void AddInitialUserInterface(Func<GameServiceContainer, IList<UiGroupModel>> initialUiModelsProvider)
		{ 
			this.InitialUiModelsProviders.Add(initialUiModelsProvider);
		}
	}
}
