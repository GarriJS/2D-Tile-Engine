using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Drawing
{
	[DataContract(Name = "tiledImage")]
	public class TiledImageModel : ImageModel
	{
		[DataMember(Name = "fillBox", Order = 3)]
		public Vector2 FillBox { get; set; }
	}
}
