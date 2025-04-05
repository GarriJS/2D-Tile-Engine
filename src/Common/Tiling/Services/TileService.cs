using Common.Tiling.Models;
using Common.Tiling.Models.Contracts;
using Common.Tiling.Services.Contracts;
using Common.DiskModels.Common.Tiling;
using Common.DiskModels.Common.Tiling.Contracts;
using Engine.Core.Constants;
using Engine.Drawables.Services.Contracts;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;
using Engine.Physics.Models;

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
		/// Gets the local tile coordinates.
		/// </summary>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="gridOffset">The grid offset.</param>
		/// <returns>The local tile coordinates.</returns>
		public Vector2 GetLocalTileCoordinates(Vector2 coordinates, int gridOffset = 0)
		{
			var col = (int)coordinates.X / TileConstants.TILE_SIZE;
			var row = (int)coordinates.Y / TileConstants.TILE_SIZE;

			return new Vector2((col + gridOffset) * TileConstants.TILE_SIZE, (row + gridOffset) * TileConstants.TILE_SIZE);
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
			var spriteService = this._gameServices.GetService<ISpriteService>();
			var tilePosition = new Position
			{
				Coordinates = new Vector2(tileModel.Column * TileConstants.TILE_SIZE, tileModel.Row * TileConstants.TILE_SIZE)
			};

			var area = areaService.GetArea(tileModel.Area, tilePosition);

			if (tileModel is AnimatedTileModel animatedTileModel)
			{
				var animationService = this._gameServices.GetService<IAnimationService>();
				var animation = animationService.GetAnimation(animatedTileModel.Animation, TileConstants.TILE_SIZE, TileConstants.TILE_SIZE);
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
			var sprite = spriteService.GetSprite(basicTileModel.Sprite);
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
