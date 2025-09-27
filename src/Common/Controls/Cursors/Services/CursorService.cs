using Common.Controls.CursorInteraction.Models;
using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.Core.Constants;
using Common.DiskModels.Controls;
using Common.UserInterface.Services.Contracts;
using Engine.Core.Initialization.Contracts;
using Engine.Core.Textures.Contracts;
using Engine.DiskModels.Physics;
using Engine.Physics.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
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
		/// Gets or sets the cursor control component.
		/// </summary>
		public CursorControlComponent CursorControlComponent { get; set; }

		/// <summary>
		/// Gets the cursors.
		/// </summary>
		public Dictionary<string, Cursor> Cursors { get; private set; } = [];

		/// <summary>
		/// Loads the content.
		/// </summary>
		public void LoadContent()
		{
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();
			var positionService = this._gameServices.GetService<IPositionService>();

			var positionModel = new PositionModel
			{
				X = default,
				Y = default
			};

			var position = positionService.GetPositionFromModel(positionModel);
			this.CursorControlComponent = new CursorControlComponent(this._gameServices)
			{
				CursorPosition = position
			};

			var cursorModel = new CursorModel
			{
				CursorName = CommonCursorNames.BasicCursorName,
				TextureBox = new Rectangle
				{ 
					X = 0,
					Y = 0,
					Width = 18,
					Height = 28	
				},
				AboveUi = true,
				TextureName = "mouse",
				Offset = default
			};

			var cursor = this.GetCursor(cursorModel, addCursor: true);
			this.CursorControlComponent.SetPrimaryCursor(cursor);
		}

		/// <summary>
		/// Gets the cursor.
		/// </summary>
		/// <param name="cursorModel">The cursor model.</param>
		/// <param name="addCursor">A value indicating whether to add the cursors.</param>
		/// <param name="drawLayerOffset">The draw layer offset.</param>
		/// <returns>The cursor.</returns>
		public Cursor GetCursor(CursorModel cursorModel, bool addCursor = false, byte drawLayerOffset = 0)
		{
			if (null == cursorModel)
			{
				return null;
			}

			var textureService = this._gameServices.GetService<ITextureService>();
			var functionService = this._gameServices.GetService<IFunctionService>();

			if (false == textureService.TryGetTexture(cursorModel.TextureName, out var texture))
			{
				texture = textureService.DebugTexture;
			}

			if (false == functionService.TryGetFunction<Action<Cursor, GameTime>>(cursorModel.CursorUpdaterName, out var cursorUpdater))
			{ 
				cursorUpdater = null;
			}

			var cursor =  new Cursor
			{
				DrawLayer = (cursorModel.AboveUi ? RunTimeConstants.BaseAboveUiCursorDrawLayer : RunTimeConstants.BaseBelowUiCursorDrawLayer) + drawLayerOffset,
				UpdateOrder = RunTimeConstants.BaseCursorUpdateOrder,
				CursorName = cursorModel.CursorName,
				TextureName = cursorModel.TextureName,
				Offset = cursorModel.Offset,
				Position = this.CursorControlComponent.CursorPosition,
				TextureBox = cursorModel.TextureBox,
				Texture = texture,
				CursorUpdater = cursorUpdater
			};

			if (true == addCursor)
			{
				this.Cursors.Add(cursor.CursorName, cursor);
			}

			return cursor;
		}

		/// <summary>
		/// Gets the cursor hover state.
		/// </summary>
		/// <returns>The hover state.</returns>
		public HoverState GetCursorHoverState()
		{
			var uiService = this._gameServices.GetService<IUserInterfaceService>();

			var uiObject = uiService.GetUiObjectAtScreenLocation(this.CursorControlComponent.CursorPosition.Coordinates);

			return uiObject;
		}
	}
}
