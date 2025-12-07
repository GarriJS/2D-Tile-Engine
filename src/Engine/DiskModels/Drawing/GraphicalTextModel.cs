using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class GraphicalTextModel
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("textColor")]
		public Color TextColor { get; set; }

		[JsonPropertyName("fontName")]
		public string FontName { get; set; }
	}
}
