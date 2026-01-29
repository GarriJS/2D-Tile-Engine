using Engine.Core.Contracts;
using Engine.Debugging.Models;

namespace Engine.Debugging.Services.Contracts
{
	/// <summary>
	/// Represents a debug service.
	/// </summary>
	public interface IDebugService : IDoConfiguration, IPostLoadInitialize
	{
		//TODO move to another file
		/// <summary>
		/// Gets the debug draw layer.
		/// </summary>
		public const int DebugDrawLayer = 999;

		/// <summary>
		/// Gets the debug update layer.
		/// </summary>
		public const int DebugUpdateOrder = 999;

		/// <summary>
		/// Gets or sets the debug draw runtime.
		/// </summary>
		public DebugDrawRuntime DebugDrawRuntime { get; }

		/// <summary>
		/// Gets or sets the debug overlaid draw runtime.
		/// </summary>
		public DebugOverlaidDrawRuntime DebugOverlaidDrawRuntime { get; }

		/// <summary>
		/// Gets or sets the debug update runtime.
		/// </summary>
		public DebugUpdateRuntime DebugUpdateRuntime { get; }

		/// <summary>
		/// Toggles the screen area indicators.
		/// </summary>
		public void ToggleScreenAreaIndicators();

		/// <summary>
		/// Toggles the performance rate counter.
		/// </summary>
		/// <param name="enable">A value indicating whether to enable the performance counters.</param>
		public void TogglePerformanceRateCounter(bool enable);

	}
}
