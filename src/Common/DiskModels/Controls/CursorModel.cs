using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Contracts;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Controls
{
	public class CursorModel : IAmAImageModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get; set; }

		[JsonPropertyName("textureRegion")]
		public TextureRegionModel TextureRegion { get; set; }

		[JsonPropertyName("cursorName")]
		public string CursorName { get; set; }

		[JsonPropertyName("aboveUi")]
		public bool AboveUi { get; set; }

		[JsonPropertyName("offset")]
		public Vector2 Offset { get; set; }

		[JsonPropertyName("cursorUpdaterName")]
		public string CursorUpdaterName { get; set; }
	}
}
