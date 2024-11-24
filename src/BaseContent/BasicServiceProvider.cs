using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace BaseContent
{
	/// <summary>
	/// Represents a basic service provider.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the basic service provider.
	/// </remarks>
	/// <param name="graphicsDeviceManager">The graphics device manager.</param>
	public class BasicServiceProvider(GraphicsDeviceManager graphicsDeviceManager) : IServiceProvider
	{
		private readonly IGraphicsDeviceService _graphicsDeviceService = graphicsDeviceManager;

		/// <summary>
		/// Gets the service type.
		/// </summary>
		/// <param name="serviceType">The service type.</param>
		/// <returns>The service.</returns>
		public object GetService(Type serviceType)
		{
			if (serviceType == typeof(IGraphicsDeviceService))
			{
				return this._graphicsDeviceService;
			}

			return null;
		}
	}
}
