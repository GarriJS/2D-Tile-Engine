using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface.Contracts
{
	public interface IAmAUiElementWithTextModel : IAmAUiElementModel
	{
		[JsonPropertyName("text")]
		public SimpleTextModel Text { get; set; }
	}
}
