using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Engine.Signals.Models.Contracts;
using Engine.Signals.Services.Contracts;
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
		/// <param name="layer">The layer.</param>
		/// <param name="updateable">The updateable.</param>
		public void AddUpdateable(int layer, ICanBeUpdated updateable)
		{
			if (true == this.ActiveSortedUpdateables.TryGetValue(layer, out var layerList))
			{
				layerList.Add(updateable);
			}
			else
			{
				layerList = [updateable];
				this.ActiveSortedUpdateables.Add(layer, layerList);
			}
		}

		/// <summary>
		/// Removes the updateable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="updateable">The updateable.</param>
		public void RemoveUpdateable(int layer, ICanBeUpdated updateable)
		{
			if (true == this.ActiveSortedUpdateables.TryGetValue(layer, out var layerList))
			{
				layerList.Remove(updateable);
			}
		}

		/// <summary>
		/// Changes the updateable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="updateable">The updateable.</param>
		public void ChangeUpdateableLayer(int layer, ICanBeUpdated updateable)
		{
			this.RemoveUpdateable(layer, updateable);
			this.AddUpdateable(layer, updateable);
		}

		/// <summary>
		/// Updates the active updateables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Update(GameTime gameTime)
		{
			var updateService = this.Game.Services.GetService<IUpdateService>();
			var signalService = this.Game.Services.GetService<ISignalService>();

			foreach (var layer in this.ActiveSortedUpdateables.Values)
			{
				foreach (var updateable in layer)
				{
					updateService.Update(gameTime, updateable);

					if (updateable is IEmitSignals emitter)
					{ 
						signalService.ProcessSignalResponses(emitter);
					}
				}
			}

			base.Update(gameTime);
		}
	}
}
