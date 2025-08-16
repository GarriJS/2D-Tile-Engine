using Engine.DiskModels.Physics;
using Engine.Physics.Models;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Services
{
	/// <summary>
	/// Represents a position service.
	/// </summary>
	/// <remarks>
	/// Initializes the position service.
	/// </remarks>
	/// <param name="gameService">The game services.</param>
	public class PositionService(GameServiceContainer gameService) : IPositionService
	{
		private readonly GameServiceContainer _gameServices = gameService;

		/// <summary>
		/// Gets the position from the model.
		/// </summary>
		/// <param name="positionModel">The position model.</param>
		/// <returns>The position.</returns>
		public Position GetPositionFromModel(PositionModel positionModel)
		{
			var vector = new Vector2(positionModel.X, positionModel.Y);

			return new Position
			{
				Coordinates = vector
			};
		}
	}
}
