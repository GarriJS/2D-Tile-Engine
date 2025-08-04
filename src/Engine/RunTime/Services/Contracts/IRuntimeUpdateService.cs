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
		public void AddUpdateable(IAmUpdateable updateable);

		/// <summary>
		/// Removes the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void RemoveUpdateable(IAmUpdateable updateable);

		/// <summary>
		/// Changes the updateable update order.
		/// </summary>
		/// <param name="newUpdateOrder">The new update order.</param>
		/// <param name="updateable">The updateable.</param>
		public void ChangeUpdateableOrder(int newUpdateOrder, IAmUpdateable updateable);
	}
}
