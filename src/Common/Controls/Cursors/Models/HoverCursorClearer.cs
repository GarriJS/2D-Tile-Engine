using Common.Controls.Cursors.Services.Contracts;
using Engine.Controls.Services.Contracts;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.Cursors.Models
{
	/// <summary>
	/// Represents a hover cursor clearer.
	/// </summary>
	public class HoverCursorClearer : IAmUpdateable
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

			var activeCursor = true == this.HoverCursorActive ?
							   cursorService.PrimaryHoverCursor :
							   cursorService.PrimaryCursor;

			if (null == activeCursor)
			{ 
				return;
			}

			var hoverResult = cursorService.ProcessCursorControlState(activeCursor, controlService.ControlState, controlService.PriorControlState);

			if ((null == hoverResult) ||
				(null == hoverResult.BaseHoverConfig?.HoverCursor))
			{
				if (true == this.HoverCursorActive)
				{
					cursorService.ClearHoverCursors();
					cursorService.PrimaryCursor.Position.Coordinates = controlService.ControlState.MousePosition;
					this.HoverCursorActive = false;
				}

				return;
			}
			
			if (activeCursor == hoverResult.BaseHoverConfig.HoverCursor)
			{
				return;
			}
			else
			{
				cursorService.SetPrimaryHoverCursor(hoverResult.BaseHoverConfig.HoverCursor);
				cursorService.PrimaryHoverCursor.Position.Coordinates = controlService.ControlState.MousePosition;
				this.HoverCursorActive = true;

				return;
			}
		}
	}
}
