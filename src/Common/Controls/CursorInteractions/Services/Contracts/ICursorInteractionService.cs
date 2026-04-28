using Common.Controls.CursorInteractions.Models;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteractions.Services.Contracts
{
	/// <summary>
	/// Represents a cursor interaction service.
	/// </summary>   
	public interface ICursorInteractionService
	{
		/// <summary>
		/// Gets the hover configuration.
		/// </summary>
		/// <param name="area">The area.</param>
		/// <param name="clickArea">The clickable area.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="clickOffset">The clickable offset.</param>
		/// <returns>The hover configuration.</returns>
		public CursorConfiguration GetCursorConfiguration(SubArea area, SubArea clickArea = null, Vector2 offset = default, Vector2 clickOffset = default);
	}
}