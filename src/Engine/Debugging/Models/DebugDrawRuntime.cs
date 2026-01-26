using Engine.Debugging.Models.Contracts;
using Engine.RunTime.Models;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Debugging.Models
{
	/// <summary>
	/// Represents a debug draw runtime.
	/// </summary>
	public class DebugDrawRuntime : IAmDrawable
	{
		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets the run time collection.
		/// </summary>
		public RunTimeCollection<IAmDebugDrawable> RunTimeCollection { get; private set; }

		/// <summary>
		/// Initializes a new instance of the debug draw runtime.
		/// </summary>
		public DebugDrawRuntime()
		{
			this.RunTimeCollection = new RunTimeCollection<IAmDebugDrawable>
			{
				KeyFunction = drawable => drawable.DrawLayer
			};
		}

		/// <summary>
		/// Adds the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void AddDrawable(IAmDebugDrawable drawable)
		{
			this.RunTimeCollection.AddModel(drawable);
		}

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawable(IAmDebugDrawable drawable)
		{
			this.RunTimeCollection.RemoveModel(drawable);
		}

		/// <summary>
		/// Changes the drawable draw layer.
		/// </summary>
		/// <param name="newDrawOrder">The new draw layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeDrawableLayer(int newDrawOrder, IAmDebugDrawable drawable)
		{
			RemoveDrawable(drawable);
			drawable.DrawLayer = newDrawOrder;
			AddDrawable(drawable);
		}

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			foreach (var kvp in RunTimeCollection.ActiveModels)
			{
				this.RunTimeCollection.CurrentKey = kvp.Key;

				foreach (var drawable in kvp.Value)
				{
					if (true == this.RunTimeCollection.PendingRemovals.Contains(drawable))
						continue;

					drawable.DrawDebug(gameTime, gameServices);
				}

				this.RunTimeCollection.ResolvePendingModels();
				this.RunTimeCollection.CurrentKey = null;
			}

			this.RunTimeCollection.ResolvePendingLists();
		}
	}
}
