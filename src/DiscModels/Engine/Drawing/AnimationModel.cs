using System.Runtime.Serialization;

namespace DiskModels.Engine.Drawing
{
	[DataContract(Name = "animation")]
	public class AnimationModel
	{
		[DataMember(Name = "currentFrameIndex", Order = 2)]
		public int CurrentFrameIndex { get; set; }

		[DataMember(Name = "frameDuration", Order = 3)]
		public int? FrameDuration { get; set; }

		[DataMember(Name = "frameMinDuration", Order = 4)]
		public int? FrameMinDuration { get; set; }

		[DataMember(Name = "frameMaxDuration", Order = 5)]
		public int? FrameMaxDuration { get; set; }

		[DataMember(Name = "frames", Order = 6)]
		public SpriteModel[] Frames { get; set; }
	}
}
