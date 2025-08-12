using Engine.DiskModels.Drawing;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Common.DiskModels.Controls
{
	[DataContract(Name = "cursor")]
	public class CursorModel : ImageModel
	{
		[DataMember(Name = "cursorName", Order = 3)]
		public string CursorName { get; set; }

		[DataMember(Name = "aboveUi", Order = 4)]
		public bool AboveUi { get; set; }

		[DataMember(Name = "offset", Order = 5)]
		public Vector2 Offset { get; set; }

		[DataMember(Name = "cursorUpdaterName", Order = 6)]
		public string CursorUpdaterName { get; set; }
	}
}
