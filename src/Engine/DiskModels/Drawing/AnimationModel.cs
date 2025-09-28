using Engine.DiskModels.Drawing.Contracts;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Drawing
{
	[DataContract(Name = "animation")]
	public class AnimationModel : IAmAGraphicModel
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
		public ImageModel[] Frames { get; set; }
	}
}
