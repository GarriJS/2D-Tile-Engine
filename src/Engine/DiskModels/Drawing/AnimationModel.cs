using Engine.DiskModels.Drawing.Contracts;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Drawing
{
	[DataContract(Name = "animation")]
	public class AnimationModel : IAmAGraphicModel
	{
		[DataMember(Name = "textureName", Order = 1)]
		public string TextureName { get => this.Frames[this.CurrentFrameIndex].TextureName; }

		[DataMember(Name = "textureBox", Order = 2)]
		public Rectangle TextureBox { get => this.Frames[this.CurrentFrameIndex].TextureBox; }

		[DataMember(Name = "currentFrameIndex", Order = 3)]
		public int CurrentFrameIndex { get; set; }

		[DataMember(Name = "frameDuration", Order = 4)]
		public int? FrameDuration { get; set; }

		[DataMember(Name = "frameMinDuration", Order = 5)]
		public int? FrameMinDuration { get; set; }

		[DataMember(Name = "frameMaxDuration", Order = 6)]
		public int? FrameMaxDuration { get; set; }

		[DataMember(Name = "frames", Order = 7)]
		public ImageModel[] Frames { get; set; }
	}
}
