using Engine.DiskModels.Drawing.Abstract;
using Engine.DiskModels.Physics;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class IndependentGraphicModel : BaseDiskModel
	{
		[JsonPropertyName("position")]
		public PositionModel Position { get; set; }

		[JsonPropertyName("graphic")]
		public GraphicBaseModel Graphic { get; set; }
	}
}
