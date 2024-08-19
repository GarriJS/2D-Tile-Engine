using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.RunTime.Services
{
	/// <summary>
	/// Represents a update service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the update service.
	/// </remarks>
	/// <param name="gameServices"></param>
	public class UpdateService(GameServiceContainer gameServices) : IUpdateService
    {
        private readonly GameServiceContainer _gameServiceContainer = gameServices;

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
