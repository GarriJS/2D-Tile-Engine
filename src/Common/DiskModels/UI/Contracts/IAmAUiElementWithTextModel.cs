using Engine.DiskModels.Drawing;
using System.Runtime.Serialization;

namespace Common.DiskModels.UI.Contracts
{
	public interface IAmAUiElementWithTextModel : IAmAUiElementModel
	{
		[DataMember(Name = "graphicText", Order = 10)]
		public GraphicalTextModel GraphicText { get; set; }
	}
}
