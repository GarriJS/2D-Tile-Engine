using Engine.RunTime.Models.Contracts;

namespace Engine.RunTime.Services.Contracts
{
    /// <summary>
    /// Represents a runtime update manager.
    /// </summary>
    public interface IRuntimeUpdateService
	{
		/// <summary>
		/// Adds the updateable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="updateable">The updateable.</param>
		public void AddUpdateable(int layer, ICanBeUpdated updateable);

		/// <summary>
		/// Removes the updateable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="updateable">The updateable.</param>
		public void RemoveUpdateable(int layer, ICanBeUpdated updateable);

		/// <summary>
		/// Changes the updateable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="updateable">The updateable.</param>
		public void ChangeUpdateableLayer(int layer, ICanBeUpdated updateable);
	}
}
