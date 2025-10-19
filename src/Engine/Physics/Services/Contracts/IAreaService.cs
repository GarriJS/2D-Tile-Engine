using Engine.DiskModels.Physics;
using Engine.DiskModels.Physics.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.Physics.Models.SubAreas;

namespace Engine.Physics.Services.Contracts
{
	/// <summary>
	/// Represents a area service.
	/// </summary>
	public interface IAreaService
	{
		/// <summary>
		/// Gets the area from the model.
		/// </summary>
		/// <param name="areaModel">The area model.</param>
		/// <returns>The area.</returns>
		public IAmAArea GetAreaFromModel(IAmAAreaModel areaModel);

		/// <summary>
		/// Gets the area from the model.
		/// </summary>
		/// <param name="areaModel">The area model.</param>
		/// <param name="position">The position</param>
		/// <returns>The area.</returns>
		public T GetAreaFromModel<T>(IAmAAreaModel areaModel, Position position = null) where T : IAmAArea;

		/// <summary>
		/// Gets the sub area.
		/// </summary>
		/// <param name="subAreaModel">The sub area model.</param>
		/// <returns>The sub area.</returns>
		public SubArea GetSubArea(SubAreaModel subAreaModel);

		/// <summary>
		/// Gets the offset sub area.
		/// </summary>
		/// <param name="offsetSubAreaModel">The off set sub area model.</param>
		/// <returns>The offset sub area.</returns>
		public OffsetSubArea GetOffSetSubArea(OffsetSubAreaModel offsetSubAreaModel);
	}
}
