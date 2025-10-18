using Engine.DiskModels.Drawing;
using System.Runtime.Serialization;

namespace Common.DiskModels.UI.Contracts
{
	public interface IAmAUiElementWithTextModel : IAmAUiElementModel
	{
		[DataMember(Name = "text", Order = 12)]
		public GraphicalTextModel Text { get; set; }
	}
}
