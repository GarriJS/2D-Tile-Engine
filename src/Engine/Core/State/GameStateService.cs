using Engine.Core.State.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Core.State
{
	/// <summary>
	/// Represents a game state service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the game state service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class GameStateService(GameServiceContainer gameServices) : IGameStateService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets a value describing whether the engine should launch in debug mode.
		/// </summary>
		public bool InDebugMode { get; set; }

		/// <summary>
		/// Gets or sets the game state flags.
		/// </summary>
		private Dictionary<string, bool> GameStateFlags { get; set; } = [];

		/// <summary>
		/// Creates the game state flag.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="initialValue">The initial value.</param>
		/// <returns>A value indicating whether the flag name already exists.</returns>
		public bool CreateGameStateFlag(string flagName, bool initialValue)
		{
			if (true == this.GameStateFlags.ContainsKey(flagName))
			{ 
				// LOGGING

				return false;
			}

			this.GameStateFlags[flagName] = initialValue;

			return true;
		}
		
	
		
	
	
	}
}
