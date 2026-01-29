namespace Engine.Core.Contracts
{
	/// <summary>
	/// Represents something that need configuration.
	/// </summary>
	public interface IDoConfiguration
	{
		/// <summary>
		/// Configures the service.
		/// </summary>
		public void ConfigureService();
	}
}
