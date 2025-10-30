using Common.DiskModels.Tiling.Converters;
using Engine.DiskModels.Monogame.Converters;
using System.Text.Json;

namespace Common.DiskModels.Tiling.Options
{
	static public class TilingOptions
	{
		static private readonly JsonSerializerOptions _tileMapOptions = new()
		{
			Converters = { new RectangleJsonConverter(), new TileModelConverter() },
			WriteIndented = true
		};

		static public JsonSerializerOptions TileMapOptions { get => _tileMapOptions; }
	}
}
