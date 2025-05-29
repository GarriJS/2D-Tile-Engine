using Engine.Core.Initialization;
using Engine.Core.Initialization.Models;
using Engine.DiskModels.UI;
using Engine.UI.Models.Contracts;
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
		/// Gets or sets a value describing whether the engine should launch in debug mode.
		/// </summary>
		public bool LaunchDebugMode { get; set; }

		/// <summary>
		/// Gets or sets the debug sprite font name.
		/// </summary>
		public string DebugSpriteFontName { get; set; }

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
		public void AddInitialModelsProvider(Func<GameServiceContainer, IList<object>> initialModelsProvider)
		{
			this.InitialModelsProviders.Add(initialModelsProvider);
		}

		/// <summary>
		/// Adds the user interface button hover events.
		/// </summary>
		/// <param name="hoverEventProcessors">The button hover event processors provider.</param>
		public void AddUiElementHoverEventProcessorsProvider(Func<GameServiceContainer, Dictionary<string, Action<IAmAUiElement, Vector2>>> hoverEventProcessors)
		{
			this.UiElementHoverEventProcessorsProviders.Add(hoverEventProcessors);
		}

		/// <summary>
		/// Adds the user interface button press events.
		/// </summary>
		/// <param name="pressEventProcessors">The button press event processors provider.</param>
		public void AddUiElementPressEventProcessorsProvider(Func<GameServiceContainer, Dictionary<string, Action<IAmAUiElement, Vector2>>> pressEventProcessors)
		{
			this.UiElementPressEventProcessorsProviders.Add(pressEventProcessors);
		}

		/// <summary>
		/// Adds the user interface button click events.
		/// </summary>
		/// <param name="clickEventProcessors">The button click event processors provider.</param>
		public void AddUiElementClickEventProcessorsProvider(Func<GameServiceContainer, Dictionary<string, Action<IAmAUiElement, Vector2>>> clickEventProcessors)
		{
			this.UiElementClickEventProcessorsProviders.Add(clickEventProcessors);
		}

		/// <summary>
		/// Adds the initial user interface.
		/// </summary>
		/// <param name="initialUiModelsProvider">The initial user interface models provider.</param>
		public void AddInitialUserInterfaceProvider(Func<GameServiceContainer, IList<UiGroupModel>> initialUiModelsProvider)
		{
			this.InitialUiModelsProviders.Add(initialUiModelsProvider);
		}
	}
}
