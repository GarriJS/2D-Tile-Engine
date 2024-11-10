using Engine.Signals.Models;
using Engine.Signals.Models.Contracts;
using Engine.UI.Models;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.UI.Services
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
	/// <param name="gameServices">The game services.</param>
	public class UserInterfaceService(GameServiceContainer gameServices) : IUserInterfaceService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		private List<UserInterfaceElement> UserInterfaceElements = [];

		public UserInterfaceElement GetUserInterfaceElement()
		{ 
			var uiElement = new UserInterfaceElement();
			this.UserInterfaceElements.Add(uiElement);

			return uiElement;
		}

		/// <summary>
		/// Processes the signal.
		/// </summary>
		/// <param name="receiver">The receiver.</param>
		/// <param name="signal">The signal.</param>
		/// <param name="allowSignalResponses">A value indicating whether to allow for signal responses or not.</param>
		public void ProcessSignal(IReceiveSignals receiver, Signal signal, bool allowSignalResponses = true)
		{

		}
	}
}
