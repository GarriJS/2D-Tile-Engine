using System.Text.Json.Serialization;

namespace Common.DiskModels.UI
{
	public struct UiPaddingModel
	{
		[JsonPropertyName("topPadding")]
		public float TopPadding { get; set; }

		[JsonPropertyName("bottomPadding")]
		public float BottomPadding { get; set; }

		[JsonPropertyName("leftPadding")]
		public float LeftPadding { get; set; }

		[JsonPropertyName("rightPadding")]
		public float RightPadding { get; set; }
	}
}
