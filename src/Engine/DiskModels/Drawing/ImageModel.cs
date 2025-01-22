using System.Runtime.Serialization;

namespace Engine.DiscModels.Engine.Drawing
{
	[DataContract(Name = "image")]
	public class ImageModel
	{
		[DataMember(Name = "textureName", Order = 1)]
		public string TextureName { get; set; }
	}
}
