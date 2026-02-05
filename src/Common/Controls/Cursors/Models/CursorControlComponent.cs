using BaseContent.BaseContentConstants.Controls;
using Common.Controls.CursorInteraction.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.Controls.Models.Contracts;
using Common.Debugging.Services.Contracts;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Engine.Controls.Models;
using Engine.Core.State.Contracts;
using Engine.Debugging.Services;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Common.Controls.Cursors.Models
{
	/// <summary>
	/// Represents a cursor control Block.
	/// </summary>
	/// <remarks>
	/// Initializes a cursor control Block.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class CursorControlComponent(GameServiceContainer gameServices) : IAmACursorControlContextComponent
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets a value indicating whether hover cursors are active.
		/// </summary>
		public bool HoverCursorActive = false;

		/// <summary>
		/// Gets or sets the cursor position.
		/// </summary>
		public Position CursorPosition { get; set; }

		/// <summary>
		/// Gets or sets the primary cursor.
		/// </summary>
		public Cursor PrimaryCursor { get; set; }

		/// <summary>
		/// Gets or sets the primary hover cursor.
		/// </summary>
		public Cursor PrimaryHoverCursor { get; set; }

		/// <summary>
		/// Gets or sets the secondary cursors.
		/// </summary>
		public List<Cursor> SecondaryCursors { get; set; } = [];

		/// <summary>
		/// Gets or sets the secondary cursors.
		/// </summary>
		public List<Cursor> SecondaryHoverCursors { get; set; } = [];

		/// <summary>
		/// Consumes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		public void ConsumeControlState(GameTime gameTime, ControlState controlState, ControlState priorControlState)
		{
			var cursorService = this._gameServices.GetService<ICursorService>();

			var hoverState = cursorService.GetCursorHoverState();
			this.ConsumeControlState(gameTime, controlState, priorControlState, hoverState);
		}

		/// <summary>
		/// Consumes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		/// <param name="hoverState">The hover state.</param>
		public void ConsumeControlState(GameTime gameTime, ControlState controlState, ControlState priorControlState, HoverState hoverState)
		{
			var cursorService = this._gameServices.GetService<ICursorService>();
			var commonDebugService = this._gameServices.GetService<ICommonDebugService>();
			var gameStateService = this._gameServices.GetService<IGameStateService>();
			commonDebugService.ClearDebugUserInterfaceZones();

			if (null == controlState)
				return;

			this.CursorPosition.Coordinates = controlState.MousePosition;

			if (null != hoverState?.TopHoverCursor)
			{
				this.SetHoverState();
				this.SetPrimaryHoverCursor(hoverState.TopHoverCursor, clearSecondaryCursors: false);

				foreach (var secondaryCursor in this.SecondaryHoverCursors)
					this.AddSecondaryHoverCursor(secondaryCursor, false);
			}
			else
			{
				this.ClearHoverState();
				this.SetPrimaryCursor(this.PrimaryCursor, clearSecondaryCursors: false);

				foreach (var secondaryCursor in this.SecondaryCursors)
					this.AddSecondaryCursor(secondaryCursor, false);
			}

			this.UpdateActiveCursors(gameTime);

			if (null == hoverState?.HoverObjectLocation)
				return;

			var uiObject = hoverState.HoverObjectLocation.Subject;
			var elementLocation = hoverState.HoverObjectLocation.Location;

			switch (uiObject)
			{
				case IAmAUiElement uiElement:

					var elementCursorInteraction = new CursorInteraction<IAmAUiElement>
					{
						CursorLocation = cursorService.CursorControlComponent.CursorPosition.Coordinates,
						ElementLocation = elementLocation,
						Element = uiElement
					};

					if (true == controlState.ActionNameIsFresh(BaseControlNames.LeftClick))
					{
						uiElement.RaisePressEvent(elementCursorInteraction);
					}
					else
					{
						uiElement.RaiseHoverEvent(elementCursorInteraction);
					}

					break;

				case UiRow uiRowWithLocation:

					var rowCursorInteraction = new CursorInteraction<UiRow>
					{
						CursorLocation = cursorService.CursorControlComponent.CursorPosition.Coordinates,
						ElementLocation = elementLocation,
						Element = uiRowWithLocation
					};
					uiRowWithLocation.RaiseHoverEvent(rowCursorInteraction);

					break;

				case UiZone uiZone:

					var zoneCursorInteraction = new CursorInteraction<UiZone>
					{
						CursorLocation = cursorService.CursorControlComponent.CursorPosition.Coordinates,
						ElementLocation = elementLocation,
						Element = uiZone
					};
					uiZone.RaiseHoverEvent(zoneCursorInteraction);

					break;
			}

			gameStateService.CheckGameStateFlagValue(DebugService.DebugFlagName, out var isDebugMode);

			if ((true == isDebugMode) &&
				(true == hoverState.HoveredObjects.TryGetValue(typeof(UiZone), out var value)) &&
				(value is UiZone hoveredUiZone))
				commonDebugService.AddDebugUserInterfaceZone(hoveredUiZone);

			if ((null != hoverState.BottomScrollable?.ScrollState) &&
				(false == hoverState.BottomScrollable.ScrollState.DisableScrolling) &&
				(0 != controlState.MouseVerticalScrollDelta))
				hoverState.BottomScrollable.ScrollState.Scroll(controlState.MouseVerticalScrollDelta);
		}

		/// <summary>
		/// Updates the active cursors.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		private void UpdateActiveCursors(GameTime gameTime)
		{
			if (false == this.HoverCursorActive)
			{
				this.PrimaryCursor.Update(gameTime, this._gameServices);

				foreach (var cursor in this.SecondaryCursors)
					cursor.Update(gameTime, this._gameServices);
			}
			else
			{
				this.PrimaryHoverCursor.Update(gameTime, this._gameServices);

				foreach (var hoverCursor in this.SecondaryHoverCursors)
					hoverCursor.Update(gameTime, this._gameServices);
			}
		}

		/// <summary>
		/// Sets the primary cursor.
		/// </summary>
		/// <param name="cursor">The cursor</param>
		/// <param name="maintainHoverState">A value indicating whether to maintain the hover state.</param>
		/// <param name="clearSecondaryCursors">A value indicating whether to clear the secondary cursors.</param>
		public void SetPrimaryCursor(Cursor cursor, bool maintainHoverState = false, bool clearSecondaryCursors = true)
		{
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (true == clearSecondaryCursors)
			{
				foreach (var secondaryCursor in this.SecondaryCursors)
					runTimeOverlaidDrawService.RemoveDrawable(secondaryCursor);

				this.SecondaryCursors.Clear();
			}

			if ((true == this.HoverCursorActive) &&
				(true == maintainHoverState))
			{
				this.PrimaryCursor = cursor;

				return;
			}

			this.HoverCursorActive = false;

			if (null != this.PrimaryCursor)
				runTimeOverlaidDrawService.RemoveDrawable(this.PrimaryCursor);

			this.PrimaryCursor = cursor;
			runTimeOverlaidDrawService.AddDrawable(this.PrimaryCursor);
		}

		/// <summary>
		/// Adds the secondary cursors.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="disableExisting">A value indicating whether to disable existing secondary cursors.</param>
		public void AddSecondaryCursor(Cursor cursor, bool disableExisting)
		{
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (this.PrimaryCursor == cursor)
				return;

			if (true == this.SecondaryCursors.Contains(cursor))
			{
				if (false == this.HoverCursorActive)
					runTimeOverlaidDrawService.AddDrawable(cursor);

				return;
			}

			if (true == disableExisting)
			{
				foreach (var secondaryCursor in this.SecondaryCursors)
					runTimeOverlaidDrawService.RemoveDrawable(secondaryCursor);

				this.SecondaryCursors.Clear();
			}

			this.SecondaryCursors.Add(cursor);

			if (false == this.HoverCursorActive)
				runTimeOverlaidDrawService.AddDrawable(cursor);
		}

		/// <summary>
		/// Sets the primary hover cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="maintainHoverState">A value indicating whether to maintain the hover state.</param>
		/// <param name="clearSecondaryCursors">A value indicating whether to clear the secondary cursors.</param>
		public void SetPrimaryHoverCursor(Cursor cursor, bool maintainHoverState = false, bool clearSecondaryCursors = true)
		{
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (true == clearSecondaryCursors)
			{
				foreach (var secondaryCursor in this.SecondaryHoverCursors)
					runTimeOverlaidDrawService.RemoveDrawable(secondaryCursor);

				this.SecondaryCursors.Clear();
			}

			if ((false == this.HoverCursorActive) &&
				(true == maintainHoverState))
			{
				this.PrimaryHoverCursor = cursor;

				return;
			}

			this.HoverCursorActive = true;

			if (null != this.PrimaryHoverCursor)
				runTimeOverlaidDrawService.RemoveDrawable(this.PrimaryHoverCursor);

			this.PrimaryHoverCursor = cursor;
			runTimeOverlaidDrawService.AddDrawable(this.PrimaryHoverCursor);
		}

		/// <summary>
		/// Adds the secondary cursors.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="disableExisting">A value indicating whether to disable existing secondary hover cursors.</param>
		public void AddSecondaryHoverCursor(Cursor cursor, bool disableExisting)
		{
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (this.PrimaryHoverCursor == cursor)
				return;

			if (true == this.SecondaryHoverCursors.Contains(cursor))
			{
				if (true == this.HoverCursorActive)
					runTimeOverlaidDrawService.AddDrawable(cursor);

				return;
			}

			if (true == disableExisting)
			{
				foreach (var secondaryCursor in this.SecondaryHoverCursors)
					runTimeOverlaidDrawService.RemoveDrawable(secondaryCursor);

				this.SecondaryHoverCursors.Clear();
			}

			this.SecondaryHoverCursors.Add(cursor);

			if (true == this.HoverCursorActive)
				runTimeOverlaidDrawService.AddDrawable(cursor);
		}

		/// <summary>
		/// Clears the hover state.
		/// </summary>
		private void ClearHoverState()
		{
			if (false == this.HoverCursorActive)
				return;

			this.HoverCursorActive = false;
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (null != this.PrimaryHoverCursor)
				runTimeOverlaidDrawService.RemoveDrawable(this.PrimaryHoverCursor);

			foreach (var secondaryCursor in this.SecondaryHoverCursors)
				runTimeOverlaidDrawService.RemoveDrawable(secondaryCursor);
		}

		/// <summary>
		/// Sets the hover state.
		/// </summary>
		private void SetHoverState()
		{
			if (true == this.HoverCursorActive)
				return;

			this.HoverCursorActive = true;
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (null != this.PrimaryCursor)
				runTimeOverlaidDrawService.RemoveDrawable(this.PrimaryCursor);

			foreach (var secondaryCursor in this.SecondaryCursors)
				runTimeOverlaidDrawService.RemoveDrawable(secondaryCursor);
		}
	}
}
