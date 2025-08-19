using Common.Controls.Cursors.Services.Contracts;
using Engine.Controls.Services.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.Cursors.Models
{
	/// <summary>
	/// Represents a cursor state monitor.
	/// </summary>
	public class CursorStateMonitor : IAmUpdateable
	{
		/// <summary>
		/// Gets or sets a value indicating whether hover cursors are active.
		/// </summary>
		public bool HoverCursorActive = false;
		
		/// <summary>
		/// Gets or sets the update order.
		/// </summary>
		public int UpdateOrder { get; set; }

		/// <summary>
		/// Gets or sets the cursor position.
		/// </summary>
		public Position CursorPosition { get; set; }

		/// <summary>
		/// Updates the updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Update(GameTime gameTime, GameServiceContainer gameServices)
		{
			var cursorService = gameServices.GetService<ICursorService>();
			var controlService = gameServices.GetService<IControlService>();

			if ((null == controlService.ControlState) ||
				(null == controlService.PriorControlState))
			{
				return;
			}

			this.CursorPosition.Coordinates = controlService.ControlState.MousePosition;
			var activeCursor = true == this.HoverCursorActive
				? cursorService.PrimaryHoverCursor
				: cursorService.PrimaryCursor;

			if (null == activeCursor)
			{ 
				return;
			}

			var hoverResult = cursorService.ProcessCursorControlState(activeCursor, controlService.ControlState, controlService.PriorControlState);

			if (null == hoverResult?.TopHoverCursorConfiguration?.HoverCursor)
			{
				if (true == this.HoverCursorActive)
				{
					cursorService.ClearHoverCursors();
					this.HoverCursorActive = false;
				}

				return;
			}

			cursorService.SetPrimaryHoverCursor(hoverResult.TopHoverCursorConfiguration.HoverCursor);

			if (activeCursor == hoverResult.TopHoverCursorConfiguration.HoverCursor)
			{
				return;
			}
			else
			{
				this.HoverCursorActive = true;

				return;
			}
		}
	}
}
