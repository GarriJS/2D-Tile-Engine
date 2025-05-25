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
		/// Toggles the screen area indicators.
		/// </summary>
		public void ToggleScreenAreaIndicators();

		/// <summary>
		/// Gets the line texture.
		/// </summary>
		/// <param name="verticalLine">A value indicating whether the line texture will be vertical.</param>
		/// <param name="length">The length.</param>
		/// <param name="color">The color.</param>
		/// <returns>The line texture.</returns>
		public Texture2D GetLineTexture(bool verticalLine, int length, Color color);

		/// <summary>
		/// Toggles the performance rate counter.
		/// </summary>
		public void TogglePerformanceRateCounter();

	}
}
