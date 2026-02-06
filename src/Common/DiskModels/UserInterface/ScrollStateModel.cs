using Common.UserInterface.Enums;
using Engine.DiskModels;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class ScrollStateModel : BaseDiskModel
	{
		[JsonPropertyName("disableScrolling")]
		public bool DisableScrolling { get; set; }

		[JsonPropertyName("drawScrollWheel")]
		public bool DrawScrollWheel { get; set; }

		[JsonPropertyName("verticalScrollOffset")]
		public float VerticalScrollOffset { get; set; }

		[JsonPropertyName("scrollSpeed")]
		public float ScrollSpeed { get; set; }

		[JsonPropertyName("maxVisibleHeight")]
		public float MaxVisibleHeight { get; set; }

		[JsonPropertyName("scrollBarWidth")]
		public int ScrollBarWidth { get; set; }

		[JsonPropertyName("scrollStateJustificationType")]
		public ScrollStateJustificationType ScrollStateJustificationType { get; set; }

		[JsonPropertyName("scrollBackgroundColor")]
		public Color ScrollBackgroundColor { get; set; }

		[JsonPropertyName("scrollNotchColor")]
		public Color ScrollNotchColor { get; set; }
	}
}
