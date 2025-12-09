using System.Text.Json.Serialization;

namespace Engine.DiskModels
{
	public class BaseDiskModel
	{
		[JsonPropertyName("type")]
		public string Type => GetType().Name;
	}
}
