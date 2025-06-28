using Common.Controls.Models;
using Common.Controls.Models.Constants;
using Common.Controls.Services.Contracts;
using Common.UI.Services.Contracts;
using Engine.Controls.Services;
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

			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				uiElementWithLocation.Element.RaisePressEvent(uiElementWithLocation.Location);
			}
			else
			{
				uiElementWithLocation.Element.RaiseHoverEvent(uiElementWithLocation.Location);
			}
		}
	}
}
