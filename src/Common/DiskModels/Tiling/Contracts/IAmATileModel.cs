using System.Text.Json.Serialization;

namespace Common.DiskModel.Tiling.Contracts
{
	public interface IAmATileModel
    {
        [JsonPropertyName("row")]
        public int Row { get; set; }

        [JsonPropertyName("column")]
        public int Column { get; set; }
    }
}
