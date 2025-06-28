using Engine.Core.Initialization;
using Engine.Core.Initialization.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
		public bool LaunchInDebugMode { get; set; }

		/// <summary>
		/// Gets or sets the debug sprite font name.
		/// </summary>
		public string DebugSpriteFontName { get; set; }

		/// <summary>
		/// Initializes a new instance of the engine.
		/// </summary>
		public Engine()
		{
			this._graphics = new GraphicsDeviceManager(this);
			this.Content.RootDirectory = "Content";
			this.Window.AllowUserResizing = true;
			this.IsMouseVisible = false;
		}

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
		/// Adds the function provider.
		/// </summary>
		/// <param name="functionProvider">The function provider.</param>
		public void AddFunctionProvider<TDelegate>(Func<GameServiceContainer, Dictionary<string, TDelegate>> functionProvider)
			where TDelegate : Delegate
		{
			this.FunctionProviders.Add(services =>
				functionProvider(services).ToDictionary(kvp => kvp.Key, kvp => (Delegate)kvp.Value)
			);
		}
	}
}
