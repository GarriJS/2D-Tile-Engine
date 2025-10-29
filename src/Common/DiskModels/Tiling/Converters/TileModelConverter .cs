using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling.Converters
{
	public class TileModelConverter : JsonConverter<TileModel>
	{
		public override TileModel Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			using JsonDocument doc = JsonDocument.ParseValue(ref reader);
			JsonElement root = doc.RootElement;

			return JsonSerializer.Deserialize<TileModel>(root.GetRawText(), options);
		}

		public override void Write(Utf8JsonWriter writer, TileModel value, JsonSerializerOptions options)
		{
			JsonSerializer.Serialize(writer, value, value.GetType(), options);
		}
	}
}
