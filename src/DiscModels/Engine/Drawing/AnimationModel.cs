using System.Runtime.Serialization;

namespace DiskModels.Engine.Drawing
{
	[DataContract(Name = "animation")]
	public class AnimationModel
	{
		[DataMember(Name = "currentFrameIndex", Order = 2)]
		public int CurrentFrameIndex { get; set; }

		[DataMember(Name = "frameConstantDuration", Order = 3)]
		public int? FrameConstantDuration { get; set; }

		[DataMember(Name = "frameMinDuration", Order = 4)]
		public int? FrameMinDuration { get; set; }

		[DataMember(Name = "frameMaxDuration", Order = 5)]
		public int? FrameMaxDuration { get; set; }

		[DataMember(Name = "sprites", Order = 6)]
		public SpriteModel[] Sprites { get; set; }
	}
}
