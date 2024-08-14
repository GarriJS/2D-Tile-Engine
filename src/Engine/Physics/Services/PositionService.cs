using DiscModels.Engine.Physics;
using Engine.Physics.Models;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Services
{
	/// <summary>
	/// Represents a position service.
	/// </summary>
	public class PositionService : IPositionService
	{
		private readonly GameServiceContainer _gameServiceContainer;

		/// <summary>
		/// Initializes the position service.
		/// </summary>
		/// <param name="gameService">The game services.</param>
		public PositionService(GameServiceContainer gameService)
		{
			this._gameServiceContainer = gameService;
		}

		/// <summary>
		/// Gets the position.
		/// </summary>
		/// <param name="positionModel">The position model.</param>
		/// <returns>The position.</returns>
		public Position GetPosition(PositionModel positionModel)
		{
			var vector = new Vector2(positionModel.X, positionModel.Y);

			return new Position
			{
				Coordinates = vector
			};
		}
	}
}
