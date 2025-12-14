using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.Controls;
using Common.DiskModels.Tiling;
using Common.Tiling.Models;
using Common.Tiling.Services.Contracts;
using Engine.Core.Constants;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Physics;
using Engine.Graphics.Enum;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Common.Tiling.Services
{
    /// <summary>
    /// Represents a tile service.
    /// </summary>
    /// <remarks>
    /// Initializes the tile service.
    /// </remarks>
    /// <param name="gameServices">The game services.</param>
    public class TileService(GameServiceContainer gameServices) : ITileService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Loads the content.
		/// </summary>
		public void LoadContent()
		{
			var cursorService = this._gameServices.GetService<ICursorService>();

			var cursorModel = new CursorModel
			{
				CursorName = CommonCursorNames.TileGridCursorName,
				AboveUi = false,
				Offset = default,
				Graphic = new SimpleImageModel
				{
					TextureName = "tile_grid_dark",
					TextureRegion = new TextureRegionModel
					{
						TextureRegionType = TextureRegionType.Simple,
						TextureBox = new Rectangle
						{
							Width = 160,
							Height = 160
						}
					}
				},
				CursorUpdaterName = CommonCursorUpdatersNames.TileGridCursorUpdater
			};

			_ = cursorService.GetCursor(cursorModel, addCursor: true);
		}

		/// <summary>
		/// Updates the tile map grid cursor position.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="gameTime">The game time.</param>
		public void TileGridCursorUpdater(Cursor cursor, GameTime gameTime)
		{
			var localTileLocation = this.GetLocalTileCoordinates(cursor.Position.Coordinates);
			cursor.Offset = new Vector2
			{
				X = localTileLocation.X - cursor.Position.X - ((cursor.Graphic.Dimensions.Width / 2) - (TileConstants.TILE_SIZE / 2)),
				Y = localTileLocation.Y - cursor.Position.Y - ((cursor.Graphic.Dimensions.Height / 2) - (TileConstants.TILE_SIZE / 2))
			};
		}

		/// <summary>
		/// Gets the local tile map coordinates.
		/// </summary>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="gridOffset">The grid offset.</param>
		/// <returns>The local tile map coordinates.</returns>
		public Vector2 GetLocalTileCoordinates(Vector2 coordinates, int gridOffset = 0)
		{
			var col = (int)coordinates.X / TileConstants.TILE_SIZE;
			var row = (int)coordinates.Y / TileConstants.TILE_SIZE;

			var result = new Vector2
			{
				X = (col + gridOffset) * TileConstants.TILE_SIZE,
				Y = (row + gridOffset) * TileConstants.TILE_SIZE
			};
				
			return result;
		}

		/// <summary>
		/// Gets the tile map from the model.
		/// </summary>
		/// <param name="tileMapModel">The tile map model.</param>
		/// <returns>The tile map.</returns>
		public TileMap GetTileMapFromModel(TileMapModel tileMapModel)
		{
			var areaService = this._gameServices.GetService<IAreaService>();

			var areaModel = new AreaModel
			{
				Position = new PositionModel
				{
					X = 0,
					Y = 0,
				},
				Width = 0,
				Height = 0,
			};

			var area = areaService.GetAreaFromModel(areaModel);

			var tileMap = new TileMap
			{
				TileMapName = tileMapModel.TileMapName,
				Area = area,
				TileMapLayers = []
			};

			if (null == tileMapModel.TileMapLayers)
			{
				return tileMap;
			}

			var mapTileModels = tileMapModel.TileMapLayers.SelectMany(e => e.Tiles)
													      .ToArray();

			foreach (var mapTile in mapTileModels)
			{
				if ((mapTile is TileModel tileModel) &&
					(true == tileMapModel.Graphics.TryGetValue(tileModel.GraphicId, out var tileImage)))
				{
					tileModel.Graphic = tileImage;
				}
			}

			foreach (var tileMapLayerModel in tileMapModel.TileMapLayers)
			{
				var tileMapLayer = this.GetTileMapLayerFromModel(tileMapLayerModel);

				if (null == tileMapLayer)
				{
					continue;
				}

				tileMap.TileMapLayers.Add(tileMapLayer.Layer, tileMapLayer);
			}

			return tileMap;
		}

		/// <summary>
		/// Gets the tile map layer from the model.
		/// </summary>
		/// <param name="tileMapLayerModel">The tile map layer model.</param>
		/// <returns>The tile map layer.</returns>
		public TileMapLayer GetTileMapLayerFromModel(TileMapLayerModel tileMapLayerModel)
		{
			var tileMapLayer = new TileMapLayer
			{
				Layer = tileMapLayerModel.Layer,
				Tiles = []
			};

			if (null == tileMapLayerModel.Tiles)
			{
				return tileMapLayer;
			}

			foreach (var tileModel in tileMapLayerModel.Tiles)
			{
				var tile = this.GetTileFromModel(tileModel);

				if (null == tile)
				{
					continue;
				}

				tileMapLayer.Tiles.Add((tile.Row, tile.Column), tile);
			}

			return tileMapLayer;
		}

		/// <summary>
		/// Gets the tile from the model.
		/// </summary>
		/// <param name="tileModel">The tile model.</param>
		/// <returns>The tile.</returns>
		public Tile GetTileFromModel(TileModel tileModel)
		{
			var graphicService = this._gameServices.GetService<IGraphicService>();

			var graphic = graphicService.GetGraphicFromModel(tileModel.Graphic);
			var tile = new Tile
			{
				Row = tileModel.Row,
				Column = tileModel.Column,
				Graphic = graphic,
				Area = TileConstants.TILE_AREA
			};

			return tile;
		}
	}
}
