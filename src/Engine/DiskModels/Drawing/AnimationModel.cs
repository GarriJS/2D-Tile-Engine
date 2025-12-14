using Engine.DiskModels.Drawing.Abstract;
using Engine.DiskModels.Physics;
using Engine.Physics.Models.SubAreas;
using System.Linq;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class AnimationModel :  GraphicBaseModel
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
		public ImageBaseModel[] Frames { get; set; }

		override public SubArea GetDimensions()
		{
			return this.Frames.FirstOrDefault()?.GetDimensions();
		}

		override public void SetDrawDimensions(SubAreaModel dimensions)
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

		override public bool Equals(object obj)
		{ 
			return base.Equals(obj);
		}
	}
}
