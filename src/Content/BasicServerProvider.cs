namespace Content
{
	/// <summary>
	/// Represents a basic service provider.
	/// </summary>
	internal class BasicServerProvider : IServiceProvider
	{
		/// <summary>
		/// Gets the service.
		/// </summary>
		/// <param name="serviceType">The service type.</param>
		/// <returns>Always throws Not Supported Exception.</returns>
		/// <exception cref="NotSupportedException">This service provider does not provide services.</exception>
		public object? GetService(Type serviceType)
		{
			throw new NotSupportedException();
		}
	}
}
