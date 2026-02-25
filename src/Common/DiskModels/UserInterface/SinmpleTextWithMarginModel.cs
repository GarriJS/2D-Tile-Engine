using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class SinmpleTextWithMarginModel : SimpleTextModel
	{
		[JsonPropertyName("margin")]
		public UiMarginModel Margin { get; set; }
	}
}
