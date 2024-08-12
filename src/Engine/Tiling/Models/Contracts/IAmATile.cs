using Engine.Physics.Models.Contracts;
using Game.Drawing.Models.Contracts;

namespace Engine.Tiling.Models.Contracts
{
	/// <summary>
	/// Represents a tile.
	/// </summary>
	public interface IAmATile : ICanBeDrawn, IHaveArea
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
