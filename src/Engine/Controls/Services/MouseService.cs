using Engine.Controls.Models;
using Engine.Controls.Services.Contracts;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.Controls.Services
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
		/// Processes the mouse state.
		/// </summary>
		/// <param name="controlState">The control state.</param>
		public void ProcessMouseState(ControlState controlState = null)
		{
			var controlService = this._gameServices.GetService<IControlService>();
			var uiService = this._gameServices.GetService<IUserInterfaceService>();

			if ((null == controlState) &&
				(null == controlService.ControlState))
			{
				return;
			}
			else if (null == controlState)
            {
				controlState = controlService.ControlState;
			}

			var mousePosition = controlState.MouseState.Position.ToVector2();
			var uiElementWithLocation = uiService.GetUiElementAtScreenLocation(mousePosition);

			if (null != uiElementWithLocation)
			{
				if (Mouse.GetState().LeftButton == ButtonState.Pressed)
				{
					uiElementWithLocation.Element.RaisePressEvent(uiElementWithLocation.Location);
				}
				else
				{
					uiElementWithLocation.Element.RaiseHoverEvent(uiElementWithLocation.Location);
				}

				return;
			}
		}
	}
}
