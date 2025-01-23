using Engine.Core.Initialization;
using Engine.Core.Initialization.Models;
using Microsoft.Xna.Framework;
using System;

namespace Engine
{
	/// <summary>
	/// Represent a game 1.
	/// </summary>
	public partial class Game1
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
		/// Adds the external model type map provider.
		/// </summary>
		/// <param name="externalModelTypeMapProviders">The external model type map providers.</param>
		public void AddExternalModelTypeMapProvider(Func<(Type typeIn, Type typeOut)[]> externalModelTypeMapProviders)
		{
			this.ExternalModelTypeMapProviders.Add(externalModelTypeMapProviders);
		}

		/// <summary>
		/// Adds the external model processor map provider.
		/// </summary>
		/// <param name="externalModelProcessorMapProvider">The external model processor map provider.</param>
		public void AddModelProcessingMapProvider(Func<GameServiceContainer, (Type typeIn, Delegate)[]> externalModelProcessorMapProvider)
		{
			this.ExternalModelProcessorMapProviders.Add(externalModelProcessorMapProvider);
		}
	}
}
