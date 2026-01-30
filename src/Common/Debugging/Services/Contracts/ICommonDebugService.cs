using Engine.Core.Contracts;

namespace Common.Debugging.Services.Contracts
{
	/// <summary>
	/// Represents a common debug service.
	/// </summary>
	public interface ICommonDebugService : IPostLoadInitialize
	{
		/// <summary>
		/// Sets the performance rate counter activity.
		/// </summary>
		/// <param name="enable">A value indicating whether to enable the performance counters.</param>
		public void SetScreenAreaIndicatorsEnabled(bool enable);
	}
}
