using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing.Contracts
{
	public interface IAmAImageModel : IAmAGraphicModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get; }
	}
}
