using Common.DiskModels.Tiling;
using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Drawing.Abstract;
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
	/// Represents a tileModel map.
	/// </summary>
	sealed public class TileMap : IAmDrawable, IHaveArea, ICanBeSerialized<TileMapModel>
	{
		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		required public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the tileModel map name.
		/// </summary>
		required public string TileMapName { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		required public IAmAArea Area { get; set; }

		/// <summary>
		/// The tile map layers.
		/// </summary>
		readonly public Dictionary<int, TileMapLayer> _tileMapLayers = [];

		/// <summary>
		/// Adds the tile. 
		/// </summary>
		/// <param name="layer">The layer of the tile.</param>
		/// <param name="tile">The tile.</param>
		public void AddTile(int layer, Tile tile)
		{
			if (true == this._tileMapLayers.TryGetValue(layer, out var tileMapLayer))
			{
				tileMapLayer.AddTile(tile);

				return;
			}

			tileMapLayer = new TileMapLayer
			{
				Layer = layer
			};
			tileMapLayer.AddTile(tile);
			this._tileMapLayers[layer] = tileMapLayer;
		}

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			foreach (var tileMapLayer in this._tileMapLayers.Values)
				tileMapLayer.Draw(gameTime, gameServices, this.Position.Coordinates, Color.White);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public TileMapModel ToModel()
		{
			var tileMapLayerModels = this._tileMapLayers.Values.Select(e => e.ToModel())
															  .ToArray();
			var tileModels = tileMapLayerModels.SelectMany(e => e.Tiles)
											   .ToArray();
			var uniqueImages = new Dictionary<ImageBaseModel, int>(new ImageModelComparer());
			var tileImageMappings = new Dictionary<int, GraphicBaseModel>();
			var nextId = 1;

			foreach (var tileModel in tileModels)
				if (tileModel.Graphic is ImageBaseModel imageModel)
				{
					if (false == uniqueImages.TryGetValue(imageModel, out var imageId))
					{
						imageId = nextId++;
						uniqueImages[imageModel] = imageId;
						tileImageMappings[imageId] = imageModel;
					}

					tileModel.GraphicId = imageId;
				}

			var result = new TileMapModel
			{
				TileMapName = this.TileMapName,
				TileMapLayers = tileMapLayerModels,
				Graphics = tileImageMappings,
			};

			return result;
		}
	}
}
