using Engine.DiskModels;
using Engine.DiskModels.Drawing.Contracts;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Controls
{
	public class CursorModel : BaseDiskModel
	{
		[JsonPropertyName("cursorName")]
		public string CursorName { get; set; }

		[JsonPropertyName("aboveUi")]
		public bool AboveUi { get; set; }

		[JsonPropertyName("offset")]
		public Vector2 Offset { get; set; }

		[JsonPropertyName("graphic")]
		public IAmAGraphicModel Graphic {get; set; }

		[JsonPropertyName("cursorUpdaterName")]
		public string CursorUpdaterName { get; set; }
	}
}
