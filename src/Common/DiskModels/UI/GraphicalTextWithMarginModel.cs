using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UI
{
	public class GraphicalTextWithMarginModel : GraphicalTextModel
	{
		[JsonPropertyName("margin")]
		public UiMarginModel Margin { get; set; }
	}
}
