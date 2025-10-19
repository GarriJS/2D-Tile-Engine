using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UI.Contracts
{
	public interface IAmAUiElementWithTextModel : IAmAUiElementModel
	{
		[JsonPropertyName("text")]
		public GraphicalTextModel Text { get; set; }
	}
}
