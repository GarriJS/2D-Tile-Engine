using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.RunTime.Services.Contracts
{
    /// <summary>
    /// Represents a update service.
    /// </summary>
    public interface IUpdateService
    {
        /// <summary>
        /// Updates the updateable.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="updateable">The updateable.</param>
        public void Update(GameTime gameTime, IAmUpdateable updateable);
    }
}
