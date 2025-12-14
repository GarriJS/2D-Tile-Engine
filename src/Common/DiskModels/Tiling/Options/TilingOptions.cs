using Engine.DiskModels;
using Engine.DiskModels.Monogame.Converters;
using System.Text.Json;

namespace Common.DiskModels.Tiling.Options
{
	static public class TilingOptions 
	{
		static private readonly JsonSerializerOptions _tileMapOptions = new()
		{
			Converters = { new RectangleJsonConverter() },
			TypeInfoResolver = new DiskModelTypeResolver(),
			WriteIndented = true
		};

		static public JsonSerializerOptions TileMapOptions { get => _tileMapOptions; }
	}
}
