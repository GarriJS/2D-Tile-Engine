using Common.Controls.CursorInteractions.Models;
using Common.Controls.CursorInteractions.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteractions.Services
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
		/// <param name="area">The area.</param>
		/// <param name="clickArea">The clickable area.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="clickOffset">The clickable offset.</param>
		/// <returns>The hover configuration.</returns>
		public CursorConfiguration GetCursorConfiguration(SubArea area, SubArea clickArea = null, Vector2 offset = default, Vector2 clickOffset = default)
		{
			var result = new CursorConfiguration
			{
				SubArea = area,
				ClickArea = clickArea,
				Offset = offset,
				ClickOffset = clickOffset
			};

			return result;
		}
	}
}
