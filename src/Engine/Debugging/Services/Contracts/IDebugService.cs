using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Debugging.Services.Contracts
{
	/// <summary>
	/// Represents a debug service.
	/// </summary>
	public interface IDebugService
	{
		/// <summary>
		/// Gets the debug draw layer.
		/// </summary>
		public int DebugDrawLayer { get; }

		/// <summary>
		/// Gets the debug update layer.
		/// </summary>
		public int DebugUpdateOrder { get; }

		/// <summary>
		/// Toggles the screen area indicators.
		/// </summary>
		public void ToggleScreenAreaIndicators();

		/// <summary>
		/// Toggles the performance rate counter.
		/// </summary>
		public void TogglePerformanceRateCounter();

	}
}
