using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models.Contracts;

namespace Common.Tiling.Models.Contracts
{
	/// <summary>
	/// Represents a tile.
	/// </summary>
	public interface IAmATile : IHaveAnImage, IHaveArea
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
