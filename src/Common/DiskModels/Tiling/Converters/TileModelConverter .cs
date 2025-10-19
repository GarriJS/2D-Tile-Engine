using Common.DiskModel.Tiling.Contracts;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling.Converters
{
	public class TileModelConverter : JsonConverter<IAmATileModel>
	{
		public override IAmATileModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using JsonDocument doc = JsonDocument.ParseValue(ref reader);
			JsonElement root = doc.RootElement;

			if (root.TryGetProperty("image", out _))
			{
				return JsonSerializer.Deserialize<TileModel>(root.GetRawText(), options);
			}
			else if (root.TryGetProperty("animation", out _))
			{
				return JsonSerializer.Deserialize<AnimatedTileModel>(root.GetRawText(), options);
			}
			else
			{
				throw new JsonException("Unknown tile member.");
			}
		}

		public override void Write(Utf8JsonWriter writer, IAmATileModel value, JsonSerializerOptions options)
		{
			JsonSerializer.Serialize(writer, value, value.GetType(), options);
		}
	}
}
