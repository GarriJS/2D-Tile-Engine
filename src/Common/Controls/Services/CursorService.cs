using Common.Controls.Models;
using Common.Controls.Models.Constants;
using Common.Controls.Services.Contracts;
using Common.UI.Services.Contracts;
using Engine.Controls.Services.Contracts;
using Engine.Core.Textures.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Common.Controls.Services
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
			var textureService = this._gameServices.GetService<ITextureService>();
			var runTimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

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
			this.Cursors.Add(cursor.CursorName, cursor);
			this.SetActiveCursor(cursor);
		}

		/// <summary>
		/// Gets the hover cursor.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <returns>The hover cursor.</returns>
		public HoverCursor GetHoverCursor(string textureName = "mouse")
		{
			var textureService = this._gameServices.GetService<ITextureService>();

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
			this.DisableAllCursors();
			cursor.IsActive = true;
			this.ActiveCursor = cursor;
		}

		/// <summary>
		/// Disables all cursors.
		/// </summary>
		public void DisableAllCursors()
		{
			if (true != this.Cursors?.Any())
			{
				return;
			}

			foreach (var cursor in this.Cursors.Values)
			{
				cursor.IsActive = false;
				cursor.TrailingCursors.Clear();
				cursor.HoverCursor?.TrailingCursors?.Clear();
			}
		}

		/// <summary>
		/// Updates the cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="gameTime">The game time.</param>
		public void BasicCursorUpdater(Cursor cursor, GameTime gameTime)
		{
			var controlService = this._gameServices.GetService<IControlService>();
			var uiService = this._gameServices.GetService<IUserInterfaceService>();

			if (null == controlService.ControlState)
			{
				return;
			}

			cursor.Position.Coordinates = controlService.ControlState.MouseState.Position.ToVector2();
			var uiElementWithLocation = uiService.GetUiElementAtScreenLocation(cursor.Position.Coordinates);

			if (null == uiElementWithLocation)
			{
				return;
			}

			if (controlService.ControlState.MouseState.LeftButton == ButtonState.Pressed &&
				controlService.PriorControlState.MouseState.LeftButton != ButtonState.Pressed)
			{
				uiElementWithLocation.Element.RaisePressEvent(uiElementWithLocation.Location);
			}
			else
			{
				uiElementWithLocation.Element.RaiseHoverEvent(uiElementWithLocation.Location);
			}
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
