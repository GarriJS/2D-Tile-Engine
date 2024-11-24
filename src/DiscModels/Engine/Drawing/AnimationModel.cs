using System.Runtime.Serialization;

namespace DiscModels.Engine.Drawing
{
	[DataContract(Name = "animation")]
	public class AnimationModel
	{
		[DataMember(Name = "currentFrameIndex", Order = 1)]
		public int CurrentFrameIndex { get; set; }

		[DataMember(Name = "frameDuration", Order = 2)]
		public int? FrameDuration { get; set; }

		[DataMember(Name = "frameMinDuration", Order = 3)]
		public int? FrameMinDuration { get; set; }

		[DataMember(Name = "frameMaxDuration", Order = 4)]
		public int? FrameMaxDuration { get; set; }

		[DataMember(Name = "frames", Order = 5)]
		public required SpriteModel[] Frames { get; set; }
	}
}
