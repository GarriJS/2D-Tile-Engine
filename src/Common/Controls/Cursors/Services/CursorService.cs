using Common.Controls.Constants;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Engine.Controls.Models;
using Engine.Core.Textures.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Common.Controls.Cursors.Services
{
	/// <summary>
	/// Represents a cursors service.
	/// </summary>
	/// <remarks>
	/// Initializes the cursor service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class CursorService(GameServiceContainer gameServices) : ICursorService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the cursor position.
		/// </summary>
		public Position CursorPosition { get; private set; }

		/// <summary>
		/// Gets the primary cursor.
		/// </summary>
		public Cursor PrimaryCursor { get; private set; }

		/// <summary>
		/// Gets or sets the primary hover cursor.
		/// </summary>
		public Cursor PrimaryHoverCursor { get; private set; }

		/// <summary>
		/// Gets the secondary cursors.
		/// </summary>
		public List<Cursor> SecondaryCursors { get; private set; } = [];

		/// <summary>
		/// Gets the secondary cursors.
		/// </summary>
		public List<Cursor> SecondaryHoverCursors { get; private set; } = [];

		/// <summary>
		/// Gets the cursors.
		/// </summary>
		public Dictionary<string, Cursor> Cursors { get; private set; } = [];

		/// <summary>
		/// Loads the content.
		/// </summary>
		public void LoadContent()
		{
			var textureService = this._gameServices.GetService<ITextureService>();
			var runTimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			this.CursorPosition = new Position
			{
				Coordinates = default
			};

			if (false == textureService.TryGetTexture("mouse", out var cursorTexture))
			{
				cursorTexture = textureService.DebugTexture;
			}

			var cursor = new Cursor
			{
				CursorName = CommonCursorNames.PrimaryCursorName,
				TextureName = cursorTexture.Name,
				Offset = default,
				CursorUpdater = this.BasicCursorUpdater,
				TextureBox = new Rectangle(0, 0, 18, 28),
				Texture = cursorTexture,
				Position = this.CursorPosition,
				DrawLayer = 1
			};

			this.Cursors.Add(cursor.CursorName, cursor);
			this.SetPrimaryCursor(cursor);

			var hoverCursorMonitor = new CursorStateMonitor
			{
				CursorPosition = this.CursorPosition,
				UpdateOrder = -100
			};

			runTimeUpdateService.AddUpdateable(hoverCursorMonitor);
		}

		/// <summary>
		/// Sets the primary cursor.
		/// </summary>
		/// <param name="cursor"></param>
		public void SetPrimaryCursor(Cursor cursor)
		{
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			this.DisableAllNonHoverCursors();
			this.PrimaryCursor = cursor;

			if (null == this.PrimaryHoverCursor)
			{
				runTimeOverlaidDrawService.AddDrawable(cursor);
				runTimeUpdateService.AddUpdateable(cursor);
			}
			else
			{
				runTimeOverlaidDrawService.AddDrawable(this.PrimaryHoverCursor);
				runTimeUpdateService.AddUpdateable(this.PrimaryHoverCursor);
			}
		}

		/// <summary>
		/// Adds the secondary cursors.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="disableExisting">A value indicating whether to disable existing secondary hover cursors.</param>
		public void AddSecondaryCursor(Cursor cursor, bool disableExisting)
		{
			if ((this.PrimaryCursor == cursor) ||
				(true == this.SecondaryCursors.Contains(cursor)))
			{
				return;
			}

			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			if (true == disableExisting)
			{
				foreach (var secondaryCursor in this.SecondaryCursors)
				{
					runTimeOverlaidDrawService.RemoveDrawable(secondaryCursor);
					runTimeUpdateService.RemoveUpdateable(secondaryCursor);
				}

				this.SecondaryCursors.Clear();
			}

			this.SecondaryCursors.Add(cursor);

			if (null == this.PrimaryHoverCursor)
			{
				runTimeOverlaidDrawService.AddDrawable(cursor);
				runTimeUpdateService.AddUpdateable(cursor);
			}
		}

		/// <summary>
		/// Sets the primary hover cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		public void SetPrimaryHoverCursor(Cursor cursor)
		{
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			if (null != this.PrimaryCursor)
			{
				runTimeOverlaidDrawService.RemoveDrawable(this.PrimaryCursor);
				runTimeUpdateService.RemoveUpdateable(this.PrimaryCursor);
			}

			foreach (var secondaryCursor in this.SecondaryCursors)
			{
				runTimeOverlaidDrawService.RemoveDrawable(secondaryCursor);
				runTimeUpdateService.RemoveUpdateable(secondaryCursor);
			}

			this.DisableAllHoverCursors(disableSecondaryHoverCursors: false);
			this.PrimaryHoverCursor = cursor;
			runTimeOverlaidDrawService.AddDrawable(cursor);
			runTimeUpdateService.AddUpdateable(cursor);

			foreach (var secondaryHoverCursor in this.SecondaryHoverCursors)
			{
				runTimeOverlaidDrawService.AddDrawable(secondaryHoverCursor);
				runTimeUpdateService.AddUpdateable(secondaryHoverCursor);
			}
		}

		/// <summary>
		/// Adds the secondary cursors.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="disableExisting">A value indicating whether to disable existing secondary hover cursors.</param>
		public void AddSecondaryHoverCursor(Cursor cursor, bool disableExisting)
		{
			if ((this.PrimaryHoverCursor == cursor) ||
				(true == this.SecondaryHoverCursors.Contains(cursor)))
			{
				return;
			}

			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			if (true == disableExisting)
			{
				foreach (var secondaryCursor in this.SecondaryHoverCursors)
				{
					runTimeOverlaidDrawService.RemoveDrawable(secondaryCursor);
					runTimeUpdateService.RemoveUpdateable(secondaryCursor);
				}

				this.SecondaryHoverCursors.Clear();
			}

			this.SecondaryHoverCursors.Add(cursor);
			runTimeOverlaidDrawService.AddDrawable(cursor);
			runTimeUpdateService.AddUpdateable(cursor);
		}

		/// <summary>
		/// Disables all non hover cursors.
		/// </summary>
		public void DisableAllNonHoverCursors()
		{
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			if (null != this.PrimaryCursor)
			{
				runTimeOverlaidDrawService.RemoveDrawable(this.PrimaryCursor);
				runTimeUpdateService.RemoveUpdateable(this.PrimaryCursor);
				this.PrimaryCursor = null;
			}

			if (0 == this.SecondaryCursors.Count)
			{
				return;
			}

			foreach (var cursor in this.SecondaryCursors)
			{
				runTimeOverlaidDrawService.RemoveDrawable(cursor);
				runTimeUpdateService.RemoveUpdateable(cursor);
			}

			this.SecondaryCursors.Clear();
		}

		/// <summary>
		/// Disables all hover cursors.
		/// </summary>
		/// <param name="disableSecondaryHoverCursors">A value indicating whether to disable secondary hover cursors.</param>
		public void DisableAllHoverCursors(bool disableSecondaryHoverCursors)
		{
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			if (null != this.PrimaryHoverCursor)
			{
				runTimeOverlaidDrawService.RemoveDrawable(this.PrimaryHoverCursor);
				runTimeUpdateService.RemoveUpdateable(this.PrimaryHoverCursor);
				this.PrimaryHoverCursor = null;
			}

			if ((false == disableSecondaryHoverCursors) ||
				(0 == this.SecondaryHoverCursors.Count))
			{
				return;
			}

			foreach (var cursor in this.SecondaryHoverCursors)
			{
				runTimeOverlaidDrawService.RemoveDrawable(cursor);
				runTimeUpdateService.RemoveUpdateable(cursor);
			}

			this.SecondaryHoverCursors.Clear();
		}

		/// <summary>
		/// Clears the hover cursors.
		/// </summary>
		public void ClearHoverCursors()
		{
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			if (null != this.PrimaryHoverCursor)
			{
				runTimeOverlaidDrawService.RemoveDrawable(this.PrimaryHoverCursor);
				runTimeUpdateService.RemoveUpdateable(this.PrimaryHoverCursor);
			}

			foreach (var cursor in this.SecondaryHoverCursors)
			{
				runTimeOverlaidDrawService.RemoveDrawable(cursor);
				runTimeUpdateService.RemoveUpdateable(cursor);
			}

			if (null != this.PrimaryCursor)
			{
				runTimeOverlaidDrawService.AddDrawable(this.PrimaryCursor);
				runTimeUpdateService.AddUpdateable(this.PrimaryCursor);
			}

			foreach (var cursor in this.SecondaryCursors)
			{
				runTimeOverlaidDrawService.AddDrawable(cursor);
				runTimeUpdateService.AddUpdateable(cursor);
			}
		}

		/// <summary>
		/// Process the cursor and control state.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		/// <returns>The object the cursor is over.</returns>
		public IHaveAHoverConfiguration ProcessCursorControlState(Cursor cursor, ControlState controlState, ControlState priorControlState)
		{
			var uiService = this._gameServices.GetService<IUserInterfaceService>();
			var uiObject = uiService.GetUiObjectAtScreenLocation(cursor.Position.Coordinates);

			if (null == uiObject)
			{
				return null;
			}

			switch (uiObject)
			{
				case UiElementWithLocation uiElementWithLocation:

					if ((ButtonState.Pressed == controlState.MouseState.LeftButton) &&
						(ButtonState.Pressed == priorControlState.MouseState.LeftButton))
					{
						uiElementWithLocation.Element.RaisePressEvent(uiElementWithLocation.Location, cursor.Position.Coordinates);
					}
					else
					{
						uiElementWithLocation.Element.RaiseHoverEvent(uiElementWithLocation.Location);
					}

					return uiElementWithLocation.Element;

				case UiRow uiRow:

					return null;

				case UiZone uiZone:

					uiZone.RaiseHoverEvent(uiZone.Position.Coordinates);

					return uiZone;
			}

			return null;
		}

		/// <summary>
		/// Updates the cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="gameTime">The game time.</param>
		public void BasicCursorUpdater(Cursor cursor, GameTime gameTime)
		{
			// cursor position is updated in the cursor state monitor
		}
	}
}
