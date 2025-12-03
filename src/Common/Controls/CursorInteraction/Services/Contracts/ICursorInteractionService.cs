using Common.Controls.CursorInteraction.Models;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteraction.Services.Contracts
{
	/// <summary>
	/// Represents a cursor interaction service.
	/// </summary>   
	public interface ICursorInteractionService
	{
		/// <summary>
		/// Gets the hover configuration.
		/// </summary>
		/// <typeparam name="T">The parent type.</typeparam>
		/// <param name="area">The area.</param>
		/// <param name="clickArea">The clickable area.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="clickOffset">The clickable offset.</param>
		/// <returns>The hover configuration.</returns>
		public CursorConfiguration<T> GetCursorConfiguration<T>(SubArea area, SubArea clickArea, Vector2 offset = default, Vector2 clickOffset = default);
	}
}
