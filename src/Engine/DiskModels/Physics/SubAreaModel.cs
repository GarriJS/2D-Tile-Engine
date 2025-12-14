using System.Text.Json.Serialization;

namespace Engine.DiskModels.Physics
{
	public class SubAreaModel : BaseDiskModel
	{
		[JsonPropertyName("width")]
		public float Width { get; set; }

		[JsonPropertyName("height")]
		public float Height { get; set; }

		override public bool Equals(object obj)
		{
			if (obj is not SubAreaModel subAreaModel)
			{
				return false;
			}

			if ((this.Width != subAreaModel.Width) ||
				(this.Height != subAreaModel.Height))
			{
				return false;
			}

			return true;
		}
	}
}
