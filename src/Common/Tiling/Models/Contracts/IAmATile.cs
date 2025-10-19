using Common.DiskModel.Tiling.Contracts;
using Engine.Core.Files.Models.Contract;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models.Contracts;
using Engine.RunTime.Models.Contracts;

namespace Common.Tiling.Models.Contracts
{
	/// <summary>
	/// Represents a tile.
	/// </summary>
	public interface IAmATile : IAmSubDrawable, IHaveAGraphic, IHaveASubArea, ICanBeSerialized<IAmATileModel>
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
