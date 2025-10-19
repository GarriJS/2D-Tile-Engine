using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class TriggeredAnimationModel : AnimationModel
	{
		[JsonPropertyName("restingFrameIndex")]
		public int RestingFrameIndex { get; set; }
	}
}
