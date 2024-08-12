using Game.Drawing.Services.Contracts;
using Game.RunTime.Models.Contracts;
using Game.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game.RunTime.Managers
{
    /// <summary>
    /// Represents a run time update manager.
    /// </summary>
    public class RuntimeUpdateManager : DrawableGameComponent, IRuntimeUpdateService
	{
		/// <summary>
		/// Gets or sets the active sorted updateable.
		/// </summary>
		private SortedDictionary<int, List<ICanBeUpdated>> ActiveSortedUpdateables { get; set; }

		/// <summary>
		/// Initializes a new instance of the run time update manager.
		/// </summary>
		/// <param name="game"></param>
		public RuntimeUpdateManager(Microsoft.Xna.Framework.Game game) : base(game)
		{

		}

		/// <summary>
		/// Initializes the runtime update manager.
		/// </summary>
		public override void Initialize()
		{
			this.ActiveSortedUpdateables = new SortedDictionary<int, List<ICanBeUpdated>>();
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
				layerList = new List<ICanBeUpdated>() { updateable };
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
		/// Draws the active updateables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Draw(GameTime gameTime)
		{
			var updateService = this.Game.Services.GetService<IUpdateService>();

			foreach (var layer in this.ActiveSortedUpdateables.Values)
			{
				foreach (var updateable in layer)
				{
					updateService.Update(gameTime, updateable);
				}
			}

			base.Draw(gameTime);
		}
	}
}
