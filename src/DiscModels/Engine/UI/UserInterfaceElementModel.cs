using DiscModels.Engine.Drawing;
using DiscModels.Engine.Physics.Contracts;
using System.Runtime.Serialization;

namespace DiscModels.Engine.UI
{
	public class UserInterfaceElementModel
	{
		[DataMember(Name = "userInterfaceElementName", Order = 1)]
		public string UserInterfaceElementName { get; set; }

		[DataMember(Name = "area", Order = 2)]
		public IAmAAreaModel Area { get; set; }

		[DataMember(Name = "sprite", Order = 3)]
		public SpriteModel Sprite { get; set; }
	}
}
