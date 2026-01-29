using Engine.Core.State.Contracts;
using Microsoft.Xna.Framework;
using System;
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
		/// The game state flags.
		/// </summary>
		readonly private Dictionary<string, bool> _gameStateFlags = new(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// The flag listeners.
		/// </summary>
		readonly private Dictionary<string, List<Action<bool>>> _flagListeners = [];

		/// <summary>
		/// Creates the game state flag.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="initialValue">The initial value.</param>
		/// <returns>A value indicating whether the flag name already exists.</returns>
		public bool CreateGameStateFlag(string flagName, bool initialValue)
		{
			if (true == this._gameStateFlags.ContainsKey(flagName))
			{
				// LOGGING

				return false;
			}

			this._gameStateFlags[flagName] = initialValue;

			return true;
		}

		/// <summary>
		/// Checks the game state flag value.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="value">The value.</param>
		/// <returns>A value indicating whether the game state flag exists.</returns>
		public bool CheckGameStateFlagValue(string flagName, out bool value)
		{
			var result = this._gameStateFlags.TryGetValue(flagName, out value);

			return result;
		}

		/// <summary>
		/// Changes the game state flag state.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="newValue">The new value.</param>
		/// <returns>A value indicating whether the flags value was actually changed.</returns>
		public bool ChangeGameStateFlagState(string flagName, bool newValue)
		{
			if (false == this._gameStateFlags.ContainsKey(flagName))
				return false;
			else if (this._gameStateFlags[flagName] == newValue)
			{
				// LOGGING

				return false;
			}

			this._gameStateFlags[flagName] = newValue;

			if (true == this._flagListeners.TryGetValue(flagName, out var listeners))
				foreach (var listener in listeners ?? [])
					listener.Invoke(newValue);

			return true;
		}

		/// <summary>
		/// Subscribes to the flag.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="callback">The callback function.</param>
		public void SubscribeToFlag(string flagName, Action<bool> callback)
		{
			if (false == this._flagListeners.ContainsKey(flagName))
				this._flagListeners[flagName] = [];

			this._flagListeners[flagName].Add(callback);
		}

		/// <summary>
		/// Unsubscribes the flag.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="callback">The call back.</param>
		public void UnsubscribeToFlag(string flagName, Action<bool> callback)
		{
			if (false == this._flagListeners.ContainsKey(flagName))
				return;

			this._flagListeners[flagName].Remove(callback);

			if (0 == this._flagListeners[flagName].Count)
				this._flagListeners.Remove(flagName);
		}

		/// <summary>
		/// Clears the subscriptions.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		public void ClearSubsriptions(string flagName)
		{
			if (false == this._flagListeners.ContainsKey(flagName))
				return;

			this._flagListeners[flagName].Clear();
			this._flagListeners.Remove(flagName);
		}
	}
}
