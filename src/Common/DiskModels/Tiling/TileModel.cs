using Common.DiscModels.Common.Tiling.Contracts;
using Engine.DiscModels.Engine.Drawing;
using Engine.DiscModels.Engine.Physics.Contracts;
using System.Runtime.Serialization;

namespace Common.DiscModels.Common.Tiling
{
	[DataContract(Name = "tile")]
    public class TileModel : IAmATileModel
    {
        [DataMember(Name = "row", Order = 1)]
        public int Row { get; set; }

        [DataMember(Name = "column", Order = 2)]
        public int Column { get; set; }

        [DataMember(Name = "area", Order = 3)]
        public IAmAAreaModel Area { get; set; }

        [DataMember(Name = "sprite", Order = 4)]
        public SpriteModel Sprite { get; set; }
    }
}
