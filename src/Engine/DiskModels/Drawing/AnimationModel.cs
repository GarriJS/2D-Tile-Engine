using Engine.DiskModels.Drawing.Contracts;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class AnimationModel : IAmAGraphicModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get => this.Frames[this.CurrentFrameIndex].TextureName; }

		[JsonPropertyName("textureBox")]
		public Rectangle TextureBox { get => this.Frames[this.CurrentFrameIndex].TextureBox; }

		[JsonPropertyName("currentFrameIndex")]
		public int CurrentFrameIndex { get; set; }

		[JsonPropertyName("frameDuration")]
		public int? FrameDuration { get; set; }

		[JsonPropertyName("frameMinDuration")]
		public int? FrameMinDuration { get; set; }

		[JsonPropertyName("frameMaxDuration")]
		public int? FrameMaxDuration { get; set; }

		[JsonPropertyName("frames")]
		public ImageModel[] Frames { get; set; }
	}
}
