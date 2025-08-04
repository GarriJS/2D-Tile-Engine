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
	public class RuntimeUpdateManager(Game game) : GameComponent(game), IRuntimeUpdateService
	{
		/// <summary>
		/// Gets or sets the run time collections.
		/// </summary>
		private RunTimeCollection<IAmUpdateable> RunTimeCollection { get; set; } 

		/// <summary>
		/// Initializes the runtime update manager.
		/// </summary>
		public override void Initialize()
		{
			this.RunTimeCollection = new RunTimeCollection<IAmUpdateable>
			{
				KeyFunction = this.GetKey
			};

			base.Initialize();
		}

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		/// <returns>The key.</returns>
		private int GetKey(IAmUpdateable updateable)
		{
			return updateable.UpdateOrder;
		}

		/// <summary>
		/// Adds the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void AddUpdateable(IAmUpdateable updateable)
		{ 
			this.RunTimeCollection.AddModel(updateable);
		}

		/// <summary>
		/// Removes the updateable.
		/// </summary>
		/// <param name="updateable">The updateable.</param>
		public void RemoveUpdateable(IAmUpdateable updateable)
		{ 
			this.RunTimeCollection.RemoveModel(updateable);
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
		public override void Update(GameTime gameTime)
		{
			foreach (var kvp in this.RunTimeCollection.ActiveModels)
			{ 
				this.RunTimeCollection.CurrentKey = kvp.Key;

				foreach (var updateable in kvp.Value)
				{ 
					if (true == this.RunTimeCollection.PendingRemovals.Contains(updateable))
					{
						continue;
					}

					updateable.Update(gameTime, this.Game.Services);
				}

				foreach (var updateable in this.RunTimeCollection.PendingAdds)
				{
					updateable.Update(gameTime, this.Game.Services);
				}

				this.RunTimeCollection.ResolvePendingModels();
				this.RunTimeCollection.CurrentKey = null;
			}
			
			base.Update(gameTime);
		}
	}
}
