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
		/// <param name="updateable">The updateable.</param>
		public void AddUpdateable(ICanBeUpdated updateable);

		/// <summary>
		/// Removes the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void RemoveUpdateable(ICanBeUpdated updateable);

		/// <summary>
		/// Changes the updateable update order.
		/// </summary>
		/// <param name="updateOrder">The update order.</param>
		/// <param name="updateable">The updateable.</param>
		public void ChangeUpdateableLayer(int updateOrder, ICanBeUpdated updateable);
	}
}
