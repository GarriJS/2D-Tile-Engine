using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class GraphicalTextWithMarginModel : GraphicalTextModel
	{
		[JsonPropertyName("margin")]
		public UiMarginModel Margin { get; set; }
	}
}
