using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Drawing
{
	[DataContract(Name = "fillImage")]
	public class FillImageModel : ImageModel
	{
		[DataMember(Name = "fillBox", Order = 3)]
		public Vector2 FillBox { get; set; }
	}
}
