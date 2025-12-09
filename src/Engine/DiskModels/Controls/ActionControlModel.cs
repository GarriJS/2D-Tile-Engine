using System.Text.Json.Serialization;

namespace Engine.DiskModels.Controls
{
	public class ActionControlModel : BaseDiskModel
	{
		[JsonPropertyName("actionName")]
		public string ActionName { get; set; }

		[JsonPropertyName("controlKeys")]
		public int[] ControlKeys { get; set; }

		[JsonPropertyName("controlMouseButtons")]
		public int[] ControlMouseButtons { get; set; }
	}
}
