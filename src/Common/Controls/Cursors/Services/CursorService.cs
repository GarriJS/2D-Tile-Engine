using Common.Controls.CursorInteraction.Models;
using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.Core.Constants;
using Common.DiskModels.Controls;
using Common.UserInterface.Services.Contracts;
using Engine.Core.Initialization.Services.Contracts;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Physics;
using Engine.Graphics.Enum;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Services.Contracts;
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
	sealed public class CursorService(GameServiceContainer gameServices) : ICursorService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets the cursor control Block.
		/// </summary>
		public CursorControlComponent CursorControlComponent { get; set; }

		/// <summary>
		/// The cursors.
		/// </summary>
		readonly public Dictionary<string, Cursor> _cursors = [];

		/// <summary>
		/// Gets the cursors.
		/// </summary>
		public Dictionary<string, Cursor> Cursors { get => this._cursors; }

		/// <summary>
		/// Loads the content.
		/// </summary>
		public void LoadContent()
		{
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
				AboveUi = true,
				Graphic = new SimpleImageModel
				{
					TextureName = "mouse",
					TextureRegion = new TextureRegionModel
					{
						TextureRegionType = TextureRegionType.Simple,
						TextureBox = new Rectangle
						{
							Width = 18,
							Height = 28
						},
						DisplayArea = new SubAreaModel
						{ 
							Width = 18,
							Height = 28
						}
					}
				}
			};
			var cursor = this.GetCursorFromModel(cursorModel, addCursor: true);
			this.CursorControlComponent.SetPrimaryCursor(cursor);
		}

		/// <summary>
		/// Gets the cursor from the model.
		/// </summary>
		/// <param name="cursorModel">The cursor model.</param>
		/// <param name="addCursor">A value indicating whether to add the cursors.</param>
		/// <param name="drawLayerOffset">The draw layer offset.</param>
		/// <returns>The cursor.</returns>
		public Cursor GetCursorFromModel(CursorModel cursorModel, bool addCursor = false, byte drawLayerOffset = 0)
		{
			if (cursorModel is null)
				return null;

			var graphicSerivce = this._gameServices.GetService<IGraphicService>();
			var functionService = this._gameServices.GetService<IFunctionService>();

			if (false == functionService.TryGetFunction<Action<Cursor, GameTime>>(cursorModel.CursorUpdaterName, out var cursorUpdater))
				cursorUpdater = null;

			var graphic = graphicSerivce.GetGraphicFromModel(cursorModel.Graphic);
			var cursor = new Cursor
			{
				DrawLayer = (cursorModel.AboveUi ? RunTimeConstants.BaseAboveUiCursorDrawLayer : RunTimeConstants.BaseBelowUiCursorDrawLayer) + drawLayerOffset,
				UpdateOrder = RunTimeConstants.BaseCursorUpdateOrder,
				CursorName = cursorModel.CursorName,
				Offset = cursorModel.Offset,
				Position = this.CursorControlComponent.CursorPosition,
				Graphic = graphic,
				CursorUpdater = cursorUpdater
			};

			if (true == addCursor)
				this._cursors.Add(cursor.CursorName, cursor);

			return cursor;
		}

		/// <summary>
		/// Gets the cursor hover state.
		/// </summary>
		/// <returns>The hover state.</returns>
		public HoverState GetCursorHoverState()
		{
			var uiLocationService = this._gameServices.GetService<IUiLocationService>();
			var uiObject = uiLocationService.GetUiObjectAtScreenLocation(this.CursorControlComponent.CursorPosition.Coordinates);

			return uiObject;
		}
	}
}
