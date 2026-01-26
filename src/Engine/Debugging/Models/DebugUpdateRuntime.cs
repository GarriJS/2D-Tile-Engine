using Engine.Debugging.Models.Contracts;
using Engine.RunTime.Models;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Debugging.Models
{
	/// <summary>
	/// Represents a debug draw runtime.
	/// </summary>
	public class DebugUpdateRuntime : IAmUpdateable
	{
		/// <summary>
		/// Gets or sets the update order.
		/// </summary>
		public int UpdateOrder { get; set; }

		/// <summary>
		/// Gets the run time collection.
		/// </summary>
		public RunTimeCollection<IAmDebugUpdateable> RunTimeCollection { get; private set; }

		/// <summary>
		/// Initializes a new instance of the debug update runtime.
		/// </summary>
		public DebugUpdateRuntime()
		{
			this.RunTimeCollection = new RunTimeCollection<IAmDebugUpdateable>
			{
				KeyFunction = updateable => updateable.UpdateOrder
			};
		}

		/// <summary>
		/// Adds the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void AddUpdateable(IAmDebugUpdateable updateable)
		{
			this.RunTimeCollection.AddModel(updateable);
		}

		/// <summary>
		/// Removes the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void RemoveUpdateable(IAmDebugUpdateable updateable)
		{
			this.RunTimeCollection.RemoveModel(updateable);
		}

		/// <summary>
		/// Changes the updateable update order.
		/// </summary>
		/// <param name="newUpdateOrder">The new update order.</param>
		/// <param name="updateable">The updateable.</param>
		public void ChangeUpdateableOrder(int newUpdateOrder, IAmDebugUpdateable updateable)
		{
			this.RemoveUpdateable(updateable);
			updateable.UpdateOrder = newUpdateOrder;
			this.AddUpdateable(updateable);
		}

		/// <summary>
		/// Updates the active updateables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Update(GameTime gameTime, GameServiceContainer gameServices)
		{
			foreach (var kvp in this.RunTimeCollection.ActiveModels)
			{
				this.RunTimeCollection.CurrentKey = kvp.Key;

				foreach (var updateable in kvp.Value)
				{
					if (true == this.RunTimeCollection.PendingRemovals.Contains(updateable))
						continue;

					updateable.UpdateDebug(gameTime, gameServices);
				}

				this.RunTimeCollection.ResolvePendingModels();
				this.RunTimeCollection.CurrentKey = null;
			}

			this.RunTimeCollection.ResolvePendingLists();
		}
	}
}
