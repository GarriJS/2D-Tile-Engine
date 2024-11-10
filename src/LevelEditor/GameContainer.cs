using Engine;
using Microsoft.Xna.Framework;

namespace LevelEditor
{
	/// <summary>
	/// The game container.
	/// </summary>
	internal static class GameContainer
	{
		/// <summary>
		/// Gets or sets the game.
		/// </summary>
		internal static Game1 Game { get; set; }

		/// <summary>
		/// Gets the game services.
		/// </summary>
		internal static GameServiceContainer GameService { get => Game.Services; }
	}
}
