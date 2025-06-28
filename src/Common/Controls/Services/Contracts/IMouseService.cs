using Engine.Core.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.Services.Contracts
{
	/// <summary>
	/// Represents a mouse service.
	/// </summary>
	public interface IMouseService : INeedInitialization
	{
		/// <summary>
		/// Processes the mouse state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServiceContainer">The game service.</param>
		public void ProcessMouseState(GameTime gameTime, GameServiceContainer gameServiceContainer);
	}
}
