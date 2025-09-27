using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.Common.Tiling;
using Common.DiskModels.Common.Tiling.Contracts;
using Common.DiskModels.Controls;
using Common.Tiling.Models;
using Common.Tiling.Models.Contracts;
using Common.Tiling.Services.Contracts;
using Engine.Core.Constants;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;

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
				TextureBox = new Rectangle
				{ 
					X = 0, 
					Y = 0,
					Width = 160,
					Height = 160
				},
				AboveUi = false,
				TextureName = "tile_grid",
				Offset = default,
				CursorUpdaterName = CommonCursorUpdatersNames.TileGridCursorUpdater
			};

			_ = cursorService.GetCursor(cursorModel, addCursor: true);
		}

		/// <summary>
		/// Updates the tile grid cursor position.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="gameTime">The game time.</param>
		public void TileGridCursorUpdater(Cursor cursor, GameTime gameTime)
		{
			var localTileLocation = this.GetLocalTileCoordinates(cursor.Position.Coordinates);
			cursor.Offset = new Vector2
			{
				X = localTileLocation.X - cursor.Position.X - ((cursor.Image.Texture.Width / 2) - (TileConstants.TILE_SIZE / 2)),
				Y = localTileLocation.Y - cursor.Position.Y - ((cursor.Image.Texture.Height / 2) - (TileConstants.TILE_SIZE / 2))
			};
		}

		/// <summary>
		/// Gets the local tile coordinates.
		/// </summary>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="gridOffset">The grid offset.</param>
		/// <returns>The local tile coordinates.</returns>
		public Vector2 GetLocalTileCoordinates(Vector2 coordinates, int gridOffset = 0)
		{
			var col = (int)coordinates.X / TileConstants.TILE_SIZE;
			var row = (int)coordinates.Y / TileConstants.TILE_SIZE;

			return new Vector2
			{
				X = (col + gridOffset) * TileConstants.TILE_SIZE,
				Y = (row + gridOffset) * TileConstants.TILE_SIZE
			};
		}

		/// <summary>
		/// Gets the tile map.
		/// </summary>
		/// <param name="tileMapModel">The tile map model.</param>
		/// <returns>The tile map.</returns>
		public TileMap GetTileMap(TileMapModel tileMapModel)
		{
			var tileMap = new TileMap
			{
				TileMapName = tileMapModel.TileMapName,
				TileMapLayers = []
			};

			if (null == tileMapModel.TileMapLayers)
			{
				return tileMap;
			}

			foreach (var tileMapLayerModel in tileMapModel.TileMapLayers)
			{
				var tileMapLayer = this.GetTileMapLayer(tileMapLayerModel);

				if (null == tileMapLayer)
				{
					continue;
				}

				tileMap.TileMapLayers.Add(tileMapLayer.Layer, tileMapLayer);
			}

			return tileMap;
		}

		/// <summary>
		/// Gets the tile map layer.
		/// </summary>
		/// <param name="tileMapLayerModel">The tile map layer model.</param>
		/// <returns>The tile map layer.</returns>
		public TileMapLayer GetTileMapLayer(TileMapLayerModel tileMapLayerModel)
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
				var tile = this.GetTile(tileModel);

				if (null == tile)
				{
					continue;
				}

				tileMapLayer.Tiles.Add((tile.Row, tile.Column), tile);
			}

			return tileMapLayer;
		}

		/// <summary>
		/// Gets the tile.
		/// </summary>
		/// <param name="tileModel">The tile model.</param>
		/// <returns>The tile.</returns>
		public IAmATile GetTile(IAmATileModel tileModel)
		{
			var areaService = this._gameServices.GetService<IAreaService>();
			var imageService = this._gameServices.GetService<IImageService>();

			var area = areaService.GetAreaFromModel(tileModel.Area);

			if (tileModel is AnimatedTileModel animatedTileModel)
			{
				var animationService = this._gameServices.GetService<IAnimationService>();
				var animation = animationService.GetAnimationFromModel(animatedTileModel.Animation);
				var animatedTile = new AnimatedTile
				{
					Row = tileModel.Row,
					Column = tileModel.Column,
					Animation = animation,
					Area = area,
				};

				return animatedTile;
			}

			var basicTileModel = tileModel as TileModel;
			var sprite = imageService.GetImageFromModel(basicTileModel.Sprite);
			var tile = new Tile
			{
				Row = tileModel.Row,
				Column = tileModel.Column,
				Image = sprite,
				Area = area,
			};

			return tile;
		}
	}
}
