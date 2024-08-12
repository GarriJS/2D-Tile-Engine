using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.RunTime.Services
{
	/// <summary>
	/// Represents a update service.
	/// </summary>
	public class UpdateService : IUpdateService
    {
        private readonly GameServiceContainer _gameServiceContainer;

        /// <summary>
        /// Initializes a new instance of the update service.
        /// </summary>
        /// <param name="gameServices"></param>
        public UpdateService(GameServiceContainer gameServices)
        {
            this._gameServiceContainer = gameServices;
        }

        /// <summary>
        /// Updates the updateable.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="updateable">The updateable.</param>
        public void Update(GameTime gameTime, ICanBeUpdated updateable)
        {
            throw new System.InvalidCastException();
        }
    }
}
