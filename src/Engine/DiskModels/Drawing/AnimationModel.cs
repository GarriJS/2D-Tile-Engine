using Engine.DiskModels.Drawing.Contracts;
using Engine.DiskModels.Physics;
using Engine.Physics.Models.SubAreas;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class AnimationModel : BaseDiskModel, IAmAGraphicModel
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

		public void SetDrawDimensions(SubAreaModel dimensions)
		{
			foreach (var frame in this.Frames ?? [])
			{
				var frameDimensions = new SubAreaModel
				{
					Width = dimensions.Width,
					Height = dimensions.Height,
				};

				frame.SetDrawDimensions(frameDimensions);	
			}
		}
	}
}
