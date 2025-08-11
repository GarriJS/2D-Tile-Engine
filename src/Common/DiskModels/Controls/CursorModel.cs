using Engine.DiskModels.Drawing;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Common.DiskModels.Controls
{
	[DataContract(Name = "cursor")]
	public class CursorModel : ImageModel
	{
		[DataMember(Name = "cursorName", Order = 2)]
		public string CursorName { get; set; }

		[DataMember(Name = "offset", Order = 3)]
		public Vector2 Offset { get; set; }

		[DataMember(Name = "cursorUpdaterName", Order = 4)]
		public string CursorUpdaterName { get; set; }
	}
}
