using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Drawing
{
	[DataContract(Name = "graphicalText")]
	public class GraphicalTextModel
	{
		[DataMember(Name = "text", Order = 1)]
		public string Text { get; set; }

		[DataMember(Name = "textColor", Order = 2)]
		public Color TextColor { get; set; }

		[DataMember(Name = "fontName", Order = 3)]
		public string FontName { get; set; }
	}
}
