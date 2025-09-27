using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models.Contracts;
using Engine.RunTime.Models.Contracts;

namespace Common.Tiling.Models.Contracts
{
	/// <summary>
	/// Represents a tile.
	/// </summary>
	public interface IAmATile : IAmSubDrawable, IHaveAnImage, IHaveASubArea
	{
		/// <summary>
		/// Gets the row.
		/// </summary>
		public int Row { get; }

		/// <summary>
		/// Gets the columns.
		/// </summary>
		public int Column { get; }
	}
}
