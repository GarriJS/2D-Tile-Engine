using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public struct UiMarginModel
	{
		[JsonPropertyName("topMargin")]
		public float TopMargin { get; set; }

		[JsonPropertyName("bottomMargin")]
		public float BottomMargin { get; set; }

		[JsonPropertyName("leftMargin")]
		public float LeftMargin { get; set; }

		[JsonPropertyName("rightMargin")]
		public float RightMargin { get; set; }
	}
}
