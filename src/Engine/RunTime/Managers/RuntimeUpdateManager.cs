using Engine.RunTime.Constants;
using Engine.RunTime.Models;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.RunTime.Managers
{
	/// <summary>
	/// Represents a run time update manager.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the run time update manager.
	/// </remarks>
	/// <param name="game">The game.</param>
	sealed public class RuntimeUpdateManager(Game game) : GameComponent(game), IRuntimeUpdateService
	{
		/// <summary>
		/// The run time collection.
		/// </summary>
		readonly public RunTimeCollection<IAmUpdateable> _runTimeCollection = new()
		{
			CurrentKey = null,
			KeyFunction = updateable => updateable.UpdateOrder
		};

		/// <summary>
		/// Initializes the runtime update manager.
		/// </summary>
		override public void Initialize()
		{
			this.UpdateOrder = ManagerOrderConstants.UpdateManagerUpdateOrder;
			base.Initialize();
		}

		/// <summary>
		/// Adds the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void AddUpdateable(IAmUpdateable updateable)
		{
			this._runTimeCollection.AddModel(updateable);
		}

		/// <summary>
		/// Removes the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void RemoveUpdateable(IAmUpdateable updateable)
		{
			this._runTimeCollection.RemoveModel(updateable);
		}

		/// <summary>
		/// Changes the updateable update order.
		/// </summary>
		/// <param name="newUpdateOrder">The new update order.</param>
		/// <param name="updateable">The updateable.</param>
		public void ChangeUpdateableOrder(int newUpdateOrder, IAmUpdateable updateable)
		{
			this.RemoveUpdateable(updateable);
			updateable.UpdateOrder = newUpdateOrder;
			this.AddUpdateable(updateable);
		}

		/// <summary>
		/// Updates the active updateables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		override public void Update(GameTime gameTime)
		{
			foreach (var kvp in this._runTimeCollection._activeModels)
			{
				this._runTimeCollection.CurrentKey = kvp.Key;

				foreach (var updateable in kvp.Value)
				{
					if (true == this._runTimeCollection._pendingRemovals.Contains(updateable))
						continue;

					updateable.Update(gameTime, this.Game.Services);
				}

				this._runTimeCollection.ResolvePendingModels();
				this._runTimeCollection.CurrentKey = null;
			}

			base.Update(gameTime);
			this._runTimeCollection.ResolvePendingLists();
		}
	}
}
