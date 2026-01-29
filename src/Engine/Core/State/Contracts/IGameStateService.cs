using System;

namespace Engine.Core.State.Contracts
{  
	/// <summary>
	/// Represents a game state service.
	/// </summary>
	public interface IGameStateService
	{
		/// <summary>
		/// Creates the game state flag.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="initialValue">The initial value.</param>
		/// <returns>A value indicating whether the flag name already exists.</returns>
		public bool CreateGameStateFlag(string flagName, bool initialValue);

		/// <summary>
		/// Checks the game state flag value.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="value">The value.</param>
		/// <returns>A value indicating whether the game state flag exists.</returns>
		public bool CheckGameStateFlagValue(string flagName, out bool value);
		
		/// <summary>
		/// Changes the game state flag state.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="newValue">The new value.</param>
		/// <returns>A value indicating whether the flags value was actually changed.</returns>
		public bool ChangeGameStateFlagState(string flagName, bool newValue);

		/// <summary>
		/// Subscribes to the flag.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="callback">The callback function.</param>
		public void SubscribeToFlag(string flagName, Action<bool> callback);

		/// <summary>
		/// Unsubscribes the flag.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		/// <param name="callback">The call back.</param>
		public void UnsubscribeToFlag(string flagName, Action<bool> callback);

		/// <summary>
		/// Clears the subscriptions.
		/// </summary>
		/// <param name="flagName">The flag name.</param>
		public void ClearSubsriptions(string flagName);
	}
}
