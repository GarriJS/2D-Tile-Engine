using Engine.DiskModels.Physics.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;

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
	}
}
