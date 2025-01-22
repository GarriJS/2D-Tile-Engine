using DiscModels.Engine.Physics.Contracts;
using System.Runtime.Serialization;

namespace DiscModels.Common.Tiling.Contracts
{
    public interface IAmATileModel
    {
        [DataMember(Name = "row", Order = 1)]
        public int Row { get; set; }

        [DataMember(Name = "column", Order = 2)]
        public int Column { get; set; }

        [DataMember(Name = "area", Order = 3)]
        public IAmAAreaModel Area { get; set; }
    }
}
