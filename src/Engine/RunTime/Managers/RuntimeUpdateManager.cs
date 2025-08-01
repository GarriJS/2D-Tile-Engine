using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.RunTime.Managers
{
	/// <summary>
	/// Represents a run time update manager.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the run time update manager.
	/// </remarks>
	/// <param name="game">The game.</param>
	public class RuntimeUpdateManager(Game game) : GameComponent(game), IRuntimeUpdateService
	{
		/// <summary>
		/// Represents a updateable container.
		/// </summary>
		private class UpdateableContainer
		{
			/// <summary>
			/// Gets or sets a value indicating whether the updateable is being removed.
			/// </summary>
			public bool IsBeingRemoved { get; set; }

			/// <summary>
			/// Gets or sets the updateable.
			/// </summary>
			public IAmUpdateable Updateable { get; set; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the update is in progress.
		/// </summary>
		public bool UpdateInProgress { get; set; } = false;

		/// <summary>
		/// Gets or sets the active updateable.
		/// </summary>
		private SortedDictionary<int, List<UpdateableContainer>> ActiveUpdateables { get; set; } = [];

		/// <summary>
		/// Gets or sets the container mappings.
		/// </summary>
		private Dictionary<IAmUpdateable, UpdateableContainer> ContainerMappings { get; set; } = [];

		/// <summary>
		/// Gets or sets the pending adds.
		/// </summary>
		private HashSet<IAmUpdateable> PendingAdds { get; set; } = [];

		/// <summary>
		/// Gets or sets the pending removals.
		/// </summary>
		private SortedDictionary<int, HashSet<UpdateableContainer>> PendingRemovals { get; set; } = [];

		/// <summary>
		/// Initializes the runtime update manager.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
		}

		/// <summary>
		/// Adds the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void AddUpdateable(IAmUpdateable updateable)
		{
			if (true == this.ContainerMappings.TryGetValue(updateable, out var mappedContainer))
			{
				this.RemovePendingRemoval(mappedContainer);

				return;
			}

			if (true == this.UpdateInProgress)
			{
				if (false == this.PendingAdds.Contains(updateable))
				{
					this.PendingAdds.Add(updateable);
				}

				return;
			}

			var container = new UpdateableContainer
			{
				IsBeingRemoved = false,
				Updateable = updateable
			};

			this.ContainerMappings.Add(updateable, container);

			if (true == this.ActiveUpdateables.TryGetValue(updateable.UpdateOrder, out var orderList))
			{
				orderList.Add(container);
			}
			else
			{
				orderList = [container];
				this.ActiveUpdateables.Add(updateable.UpdateOrder, orderList);
			}
		}

		/// <summary>
		/// Adds the pending removal.
		/// </summary>
		/// <param name="container">The container.</param>
		private void AddPendingRemoval(UpdateableContainer container)
		{
			container.IsBeingRemoved = true;

			if (true == this.PendingRemovals.TryGetValue(container.Updateable.UpdateOrder, out var orderList))
			{
				orderList.Add(container);
			}
			else
			{
				orderList = [container];
				this.PendingRemovals.Add(container.Updateable.UpdateOrder, orderList);
			}
		}

		/// <summary>
		/// Removes the pending removal.
		/// </summary>
		/// <param name="container">The container.</param>
		private void RemovePendingRemoval(UpdateableContainer container)
		{
			container.IsBeingRemoved = false;

			if (false == this.PendingRemovals.TryGetValue(container.Updateable.UpdateOrder, out var removeOrderList))
			{
				return;
			}

			removeOrderList.Remove(container);

			if (0 == removeOrderList.Count)
			{
				this.PendingRemovals.Remove(container.Updateable.UpdateOrder);
			}
		}

		/// <summary>
		/// Removes the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void RemoveUpdateable(IAmUpdateable updateable)
		{
			this.PendingAdds.Remove(updateable);

			if ((false == this.ContainerMappings.TryGetValue(updateable, out var container)) ||
				(false == this.ActiveUpdateables.TryGetValue(container.Updateable.UpdateOrder, out var orderList)))
			{
				return;
			}

			if (false == this.UpdateInProgress)
			{
				this.ContainerMappings.Remove(updateable);
				orderList.Remove(container);

				if (0 == orderList.Count)
				{
					this.ActiveUpdateables.Remove(container.Updateable.UpdateOrder);
				}

				return;
			}

			if (true == container.IsBeingRemoved)
			{
				return;
			}

			this.AddPendingRemoval(container);
		}

		/// <summary>
		/// Changes the updateable update order.
		/// </summary>
		/// <param name="updateOrder">The update order.</param>
		/// <param name="updateable">The updateable.</param>
		public void ChangeUpdateableLayer(int updateOrder, IAmUpdateable updateable)
		{
			this.RemoveUpdateable(updateable);
			updateable.UpdateOrder = updateOrder;
			this.AddUpdateable(updateable);
		}

		/// <summary>
		/// Cleans up the pending removals.
		/// </summary>
		private void CleanUpPendingRemovals()
		{
			foreach (var kvp in this.PendingRemovals)
			{
				if (false == this.ActiveUpdateables.TryGetValue(kvp.Key, out var correspondingList))
				{
					continue;
				}

				foreach (var removingContainer in kvp.Value)
				{
					this.ContainerMappings.Remove(removingContainer.Updateable);
					correspondingList.Remove(removingContainer);
				}
			}

			this.PendingRemovals.Clear();
		}

		/// <summary>
		/// Updates the active updateables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Update(GameTime gameTime)
		{
			foreach (var pendingAdd in this.PendingAdds)
			{
				this.AddUpdateable(pendingAdd);
			}

			this.PendingAdds.Clear();
			this.UpdateInProgress = true;

			foreach (var layer in this.ActiveUpdateables.Values)
			{
				foreach (var container in layer)
				{
					if (true == container.IsBeingRemoved)
					{
						continue;
					}

					container.Updateable.Update(gameTime, this.Game.Services);
				}
			}

			this.UpdateInProgress = false;
			this.CleanUpPendingRemovals();
			base.Update(gameTime);
		}
	}
}
