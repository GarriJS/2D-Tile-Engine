using System.Runtime.Serialization;

namespace Engine.DiscModels.Engine.Drawing
{
	[DataContract(Name = "triggeredAnimationModel")]
	public class TriggeredAnimationModel : AnimationModel
	{
		[DataMember(Name = "restingFrameIndex", Order = 6)]
		public int RestingFrameIndex { get; set; }
	}
}
