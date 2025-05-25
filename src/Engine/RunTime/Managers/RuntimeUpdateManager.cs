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
		/// Gets or sets the active sorted updateable.
		/// </summary>
		private SortedDictionary<int, List<ICanBeUpdated>> ActiveSortedUpdateables { get; set; } = [];

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
		public void AddUpdateable(ICanBeUpdated updateable)
		{
			if (true == this.ActiveSortedUpdateables.TryGetValue(updateable.UpdateOrder, out var orderList))
			{
				orderList.Add(updateable);
			}
			else
			{
				orderList = [updateable];
				this.ActiveSortedUpdateables.Add(updateable.UpdateOrder, orderList);
			}
		}

		/// <summary>
		/// Removes the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void RemoveUpdateable(ICanBeUpdated updateable)
		{
			if (true == this.ActiveSortedUpdateables.TryGetValue(updateable.UpdateOrder, out var orderList))
			{
				orderList.Remove(updateable);
			}
		}

		/// <summary>
		/// Changes the updateable update order.
		/// </summary>
		/// <param name="updateOrder">The update order.</param>
		/// <param name="updateable">The updateable.</param>
		public void ChangeUpdateableLayer(int updateOrder, ICanBeUpdated updateable)
		{
			this.RemoveUpdateable(updateable);
			updateable.UpdateOrder = updateOrder;
			this.AddUpdateable(updateable);
		}

		/// <summary>
		/// Updates the active updateables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Update(GameTime gameTime)
		{
			var updateService = this.Game.Services.GetService<IUpdateService>();

			foreach (var layer in this.ActiveSortedUpdateables.Values)
			{
				foreach (var updateable in layer)
				{
					updateable.Update(gameTime, this.Game.Services);
				}
			}

			base.Update(gameTime);
		}
	}
}
