using Common.DiskModels.Common.Tiling.Contracts;
using Engine.DiskModels.Drawing;
using System.Runtime.Serialization;

namespace Common.DiskModels.Common.Tiling
{
	[DataContract(Name = "tile")]
    public class TileModel : IAmATileModel
    {
        [DataMember(Name = "row", Order = 1)]
        public int Row { get; set; }

        [DataMember(Name = "column", Order = 2)]
        public int Column { get; set; }

        [DataMember(Name = "image", Order = 3)]
        public ImageModel Image { get; set; }
    }
}
