using Microsoft.Xna.Framework;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Monogame.Converters
{
	public class RectangleJsonConverter : JsonConverter<Rectangle>
	{
		public override Rectangle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			int x = 0, y = 0, width = 0, height = 0;

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndObject)
					break;

				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					string propertyName = reader.GetString();
					reader.Read();

					switch (propertyName)
					{
						case "x": x = reader.GetInt32(); break;
						case "y": y = reader.GetInt32(); break;
						case "width": width = reader.GetInt32(); break;
						case "height": height = reader.GetInt32(); break;
					}
				}
			}

			return new Rectangle(x, y, width, height);
		}

		public override void Write(Utf8JsonWriter writer, Rectangle value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("x", value.X);
			writer.WriteNumber("y", value.Y);
			writer.WriteNumber("width", value.Width);
			writer.WriteNumber("height", value.Height);
			writer.WriteEndObject();
		}
	}
}
