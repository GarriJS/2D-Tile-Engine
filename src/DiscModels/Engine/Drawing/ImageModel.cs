using System.Runtime.Serialization;

namespace DiscModels.Engine.Drawing
{
	[DataContract(Name = "image")]
	public class ImageModel
	{
		[DataMember(Name = "textureName", Order = 1)]
		public string TextureName { get; set; }
	}
}
