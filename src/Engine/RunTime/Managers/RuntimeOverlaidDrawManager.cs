using Engine.RunTime.Constants;
using Engine.RunTime.Models;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.RunTime.Managers
{
	/// <summary>
	/// Represents a runtime overlaid draw manager.
	/// </summary>
	/// <remarks>
	/// Creates a new instance of the runtime overlaid draw manager.
	/// </remarks>
	/// <param name="game">The game.</param>
	public class RuntimeOverlaidDrawManager(Game game) : DrawableGameComponent(game), IRuntimeOverlaidDrawService
	{
		/// <summary>
		/// Gets the run time collection.
		/// </summary>
		public RunTimeCollection<IAmDrawable> RunTimeCollection { get; private set; }

		/// <summary>
		/// Initializes the runtime draw manager.
		/// </summary>
		public override void Initialize()
		{
			this.UpdateOrder = ManagerOrderConstants.UnusedOrder;
			this.DrawOrder = ManagerOrderConstants.OverlaidDrawMangerDrawOrder;
			this.RunTimeCollection = new RunTimeCollection<IAmDrawable>
			{
				KeyFunction = this.GetKey
			};
			base.Initialize();
		}

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		/// <returns>The key.</returns>
		private int GetKey(IAmDrawable drawable)
		{
			var key = drawable.DrawLayer;

			return key;
		}

		/// <summary>
		/// Adds the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void AddDrawable(IAmDrawable drawable)
		{
			this.RunTimeCollection.AddModel(drawable);
		}

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawable(IAmDrawable drawable)
		{
			this.RunTimeCollection.RemoveModel(drawable);
		}

		/// <summary>
		/// Changes the drawable draw layer.
		/// </summary>
		/// <param name="newDrawOrder">The new draw layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeDrawableLayer(int newDrawOrder, IAmDrawable drawable)
		{
			this.RemoveDrawable(drawable);
			drawable.DrawLayer = newDrawOrder;
			this.AddDrawable(drawable);
		}

		/// <summary>
		/// Updates the active drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Draw(GameTime gameTime)
		{
			var drawingService = this.Game.Services.GetService<IDrawingService>();
			drawingService.BeginDraw();

			foreach (var kvp in this.RunTimeCollection.ActiveModels)
			{
				this.RunTimeCollection.CurrentKey = kvp.Key;

				foreach (var drawable in kvp.Value)
				{
					if (true == this.RunTimeCollection.PendingRemovals.Contains(drawable))
						continue;

					drawable.Draw(gameTime, this.Game.Services);
				}

				this.RunTimeCollection.ResolvePendingModels();
				this.RunTimeCollection.CurrentKey = null;
			}

			base.Draw(gameTime);
			drawingService.EndDraw();
			this.RunTimeCollection.ResolvePendingLists();
		}
	}
}
