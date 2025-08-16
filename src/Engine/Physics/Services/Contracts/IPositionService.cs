using Engine.DiskModels.Physics;
using Engine.Physics.Models;

namespace Engine.Physics.Services.Contracts
{
	/// <summary>
	/// Represents a position service.
	/// </summary>
	public interface IPositionService
	{
		/// <summary>
		/// Gets the position from the model.
		/// </summary>
		/// <param name="positionModel">The position model.</param>
		/// <returns>The position.</returns>
		public Position GetPositionFromModel(PositionModel positionModel);
	}
}
