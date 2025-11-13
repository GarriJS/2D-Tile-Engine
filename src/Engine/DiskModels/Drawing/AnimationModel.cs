using Engine.DiskModels.Drawing.Contracts;
using Engine.Physics.Models.SubAreas;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class AnimationModel : IAmAGraphicModel
	{
		[JsonPropertyName("currentFrameIndex")]
		public int CurrentFrameIndex { get; set; }

		[JsonPropertyName("frameDuration")]
		public int? FrameDuration { get; set; }

		[JsonPropertyName("frameMinDuration")]
		public int? FrameMinDuration { get; set; }

		[JsonPropertyName("frameMaxDuration")]
		public int? FrameMaxDuration { get; set; }

		[JsonPropertyName("frames")]
		public IAmAImageModel[] Frames { get; set; }

		public virtual SubArea GetDimensions()
		{
			return new SubArea();
		}
	}
}
