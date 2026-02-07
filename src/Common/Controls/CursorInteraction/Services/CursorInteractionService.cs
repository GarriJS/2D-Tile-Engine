using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteraction.Services
{
	/// <summary>
	/// Represents a cursor interaction service.
	/// </summary>   
	/// <remarks>
	/// Initializes the cursor interaction service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	sealed public class CursorInteractionService(GameServiceContainer gameServices) : ICursorInteractionService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the hover configuration.
		/// </summary>
		/// <typeparam name="T">The parent type.</typeparam>
		/// <param name="area">The area.</param>
		/// <param name="clickArea">The clickable area.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="clickOffset">The clickable offset.</param>
		/// <returns>The hover configuration.</returns>
		public CursorConfiguration<T> GetCursorConfiguration<T>(SubArea area, SubArea clickArea, Vector2 offset = default, Vector2 clickOffset = default)
		{
			var result = new CursorConfiguration<T>
			{
				Area = area,
				ClickArea = clickArea,
				Offset = offset,
				ClickOffset = clickOffset
			};

			return result;
		}
	}
}
