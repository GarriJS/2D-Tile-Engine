using DiscModels.Engine.Physics.Contracts;
using Engine.Physics.Models.Contracts;
using Engine.Physics.Models;

namespace Engine.Physics.Services.Contracts
{
	/// <summary>
	/// Represents a area service.
	/// </summary>
	public interface IAreaService
	{
		/// <summary>
		/// Gets the area.
		/// </summary>
		/// <param name="areaModel">The area model.</param>
		/// <param name="position">The position.</param>
		/// <returns>The area.</returns>
		public T GetArea<T>(IAmAAreaModel areaModel, Position position = null) where T : IAmAArea;
	}
}
