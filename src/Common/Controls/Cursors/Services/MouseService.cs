using Common.Controls.Cursors.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.Cursors.Services
{
    /// <summary>
    /// Represents a mouse service.
    /// </summary>
    /// <remarks>
    /// Initializes the mouse service.
    /// </remarks>
    /// <param name="gameServices">The game services.</param>
    public class MouseService(GameServiceContainer gameServices) : IMouseService
    {
        private readonly GameServiceContainer _gameServices = gameServices;

        /// <summary>
        /// Performs initialization.
        /// </summary>
        public void Initialize()
        {
            //ControlManager.ControlStateUpdated += this.ProcessMouseState;
        }

        /// <summary>
        /// Processes the mouse state.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        public void ProcessMouseState(GameTime gameTime, GameServiceContainer gameServices)
        {

        }
    }
}
