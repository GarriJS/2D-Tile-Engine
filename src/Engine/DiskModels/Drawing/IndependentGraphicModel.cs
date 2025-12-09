using Engine.DiskModels.Drawing.Contracts;
using Engine.DiskModels.Physics;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class IndependentGraphicModel : BaseDiskModel
	{
		[JsonPropertyName("position")]
		public PositionModel Position { get; set; }

		[JsonPropertyName("graphic")]
		public IAmAGraphicModel Graphic { get; set; }
	}
}
