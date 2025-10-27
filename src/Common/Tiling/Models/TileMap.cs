using Common.DiskModels.Tiling;
using Common.Tiling.Models.Contracts;
using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Comparers;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a mapTile map.
	/// </summary>
	public class TileMap : IAmDrawable, IHaveArea, ICanBeSerialized<TileMapModel>
	{
		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the mapTile map name.
		/// </summary>
		public string TileMapName { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }

		/// <summary>
		/// Gets or sets the mapTile map layer.
		/// </summary>
		public Dictionary<int, TileMapLayer> TileMapLayers { get; set; } = [];

		/// <summary>
		/// Adds the mapTile. 
		/// </summary>
		/// <param name="layer">The layer of the mapTile.</param>
		/// <param name="tile">The mapTile.</param>
		public void AddTile(int layer, IAmATile tile)
		{
			if (true == this.TileMapLayers.TryGetValue(layer, out var tileMapLayer))
			{
				tileMapLayer.AddTile(tile);

				return;
			}

			tileMapLayer = new TileMapLayer
			{
				Layer = layer,
			};

			tileMapLayer.AddTile(tile);
			this.TileMapLayers[layer] = tileMapLayer;
		}

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			foreach (var tileMapLayer in this.TileMapLayers.Values)
			{
				tileMapLayer.Draw(gameTime, gameServices, this.Position);
			}
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public TileMapModel ToModel()
		{
			var tileMapLayerModels = this.TileMapLayers.Values.Select(e => e.ToModel())
															  .ToArray();
			var mapTileModels = tileMapLayerModels.SelectMany(e => e.Tiles)
											   .ToArray(); 
			var uniqueImages = new Dictionary<SimpleImageModel, int>(new ImageModelComparer());
			var tileImageMappings = new Dictionary<int, SimpleImageModel>();
			var nextId = 1;

			foreach (var mapTile in mapTileModels)
			{
				if (mapTile is TileModel tileModel)
				{
					if (false == uniqueImages.TryGetValue(tileModel.Graphic, out var imageId))
					{
						imageId = nextId++;
						uniqueImages[tileModel.Graphic] = imageId;
						tileImageMappings[imageId] = tileModel.Graphic;
					}

					tileModel.GraphicId = imageId;
				}
			}

			return new TileMapModel
			{
				TileMapName = this.TileMapName,
				TileMapLayers = tileMapLayerModels,
				Images = tileImageMappings,
			};
		}
	}
}
