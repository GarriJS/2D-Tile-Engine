using Common.Controls.Constants;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Engine.Controls.Models;
using Engine.Controls.Services.Contracts;
using Engine.Core.Textures.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

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
		/// Gets the active cursor.
		/// </summary>
		public Cursor ActiveCursor { get; private set; }

		/// <summary>
		/// Gets the cursors.
		/// </summary>
		public Dictionary<string, Cursor> Cursors { get; private set; } = [];

		/// <summary>
		/// Loads the content.
		/// </summary>
		public void LoadContent()
		{
			var textureService = _gameServices.GetService<ITextureService>();
			var runTimeDrawService = _gameServices.GetService<IRuntimeDrawService>();
			var runTimeUpdateService = _gameServices.GetService<IRuntimeUpdateService>();

			if (false == textureService.TryGetTexture("mouse", out var cursorTexture))
			{
				cursorTexture = textureService.DebugTexture;
			}

			var position = new Position
			{
				Coordinates = default
			};

			var cursor = new Cursor
			{
				IsActive = true,
				CursorName = CommonCursorNames.PrimaryCursorName,
				TextureName = cursorTexture.Name,
				Offset = default,
				CursorUpdater = this.BasicCursorUpdater,
				TextureBox = new Rectangle(0, 0, 18, 28),
				Texture = cursorTexture,
				Position = position,
				DrawLayer = 1,
				TrailingCursors = []
			};

			runTimeDrawService.AddOverlaidDrawable(cursor);
			Cursors.Add(cursor.CursorName, cursor);
			SetActiveCursor(cursor);
		}

		/// <summary>
		/// Gets the hover cursor.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <returns>The hover cursor.</returns>
		public HoverCursor GetHoverCursor(string textureName = "mouse")
		{
			var textureService = _gameServices.GetService<ITextureService>();

			if (false == textureService.TryGetTexture("mouse", out var cursorTexture))
			{
				cursorTexture = textureService.DebugTexture;
			}

			var position = new Position
			{
				Coordinates = default
			};

			return new HoverCursor
			{
				IsActive = false,
				CursorName = CommonCursorNames.PrimaryCursorName,
				TextureName = cursorTexture.Name,
				Offset = default,
				TextureBox = new Rectangle(0, 0, 18, 28),
				Texture = cursorTexture,
				TrailingCursors = []
			};
		}

		/// <summary>
		/// Sets the active cursor.
		/// </summary>
		/// <param name="cursor"></param>
		public void SetActiveCursor(Cursor cursor)
		{
			DisableAllCursors();
			cursor.IsActive = true;
			ActiveCursor = cursor;
		}

		/// <summary>
		/// Disables all cursors.
		/// </summary>
		public void DisableAllCursors()
		{
			if (true != Cursors?.Any())
			{
				return;
			}

			foreach (var cursor in Cursors.Values)
			{
				cursor.IsActive = false;
				cursor.TrailingCursors.Clear();
				cursor.HoverCursor?.TrailingCursors?.Clear();
			}
		}

		/// <summary>
		/// Process the cursor and control state.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		public void ProcessCursorControlState(Cursor cursor, ControlState controlState, ControlState priorControlState)
		{
			var uiService = _gameServices.GetService<IUserInterfaceService>();
			var uiObject = uiService.GetUiObjectAtScreenLocation(cursor.Position.Coordinates);

			if (null == uiObject)
			{
				return;
			}

			switch (uiObject)
			{
				case UiElementWithLocation uiElementWithLocation:
					if (controlState.MouseState.LeftButton == ButtonState.Pressed &&
						priorControlState.MouseState.LeftButton != ButtonState.Pressed)
					{
						uiElementWithLocation.Element.RaisePressEvent(uiElementWithLocation.Location);

						return;
					}
					else
					{
						uiElementWithLocation.Element.RaiseHoverEvent(uiElementWithLocation.Location);

						return;
					}
				case UiRow uiRow:

					break;
				case UiZone uiZone:
					uiZone.RaiseHoverEvent(uiZone.Position.Coordinates);

					return;
			}
		}

		/// <summary>
		/// Updates the cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="gameTime">The game time.</param>
		public void BasicCursorUpdater(Cursor cursor, GameTime gameTime)
		{
			var controlService = _gameServices.GetService<IControlService>();
			var uiService = _gameServices.GetService<IUserInterfaceService>();

			if (null == controlService.ControlState)
			{
				return;
			}

			cursor.Position.Coordinates = controlService.ControlState.MouseState.Position.ToVector2();
			ProcessCursorControlState(cursor, controlService.ControlState, controlService.PriorControlState);
		}

		/// <summary>
		/// Updates the spritesheet button trailing cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="trailingCursor">The trailing cursor.</param>
		/// <param name="gameTime">The game time.</param>
		//public void BasicTrailingCursorUpdater(Cursor cursor, TrailingCursor trailingCursor, GameTime gameTime)
		//{

		//}
	}
}
