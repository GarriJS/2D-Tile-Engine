using System.Runtime.Serialization;

namespace Engine.DiskModels.UI.Contracts
{
	public interface IAmAUiElementWithTextModel : IAmAUiElementModel
	{
		[DataMember(Name = "text", Order = 7)]
		public string Text { get; set; }
	}
}
