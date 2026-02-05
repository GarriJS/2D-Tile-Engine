using Engine.RunTime.Constants;
using Engine.RunTime.Models;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.RunTime.Managers
{
	/// <summary>
	/// Represents a runtime overlaid draw manager.
	/// </summary>
	/// <remarks>
	/// Creates a new instance of the runtime overlaid draw manager.
	/// </remarks>
	/// <param name="game">The game.</param>
	sealed public class RuntimeOverlaidDrawManager(Game game) : DrawableGameComponent(game), IRuntimeOverlaidDrawService
	{
		/// <summary>
		/// The sub renders.
		/// </summary>
		readonly public List<IAmPreRenderable> _subRenders = [];

		/// <summary>
		/// The run time collection.
		/// </summary>
		readonly public RunTimeCollection<IAmDrawable> _runTimeCollection = new()
		{
			CurrentKey = null,
			KeyFunction = drawable => drawable.DrawLayer
		};

		/// <summary>
		/// Initializes the runtime draw manager.
		/// </summary>
		override public void Initialize()
		{
			this.UpdateOrder = ManagerOrderConstants.UnusedOrder;
			this.DrawOrder = ManagerOrderConstants.OverlaidDrawMangerDrawOrder;
			base.Initialize();
		}

		/// <summary>
		/// Adds the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void AddDrawable(IAmDrawable drawable)
		{
			this._runTimeCollection.AddModel(drawable);

			if (drawable is IAmPreRenderable subRender)
			{
				var preRenderService = this.Game.Services.GetService<IPreRenderService>();
				preRenderService.AddOverlaidPrerender(subRender);
			}
		}

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawable(IAmDrawable drawable)
		{
			this._runTimeCollection.RemoveModel(drawable);

			if (drawable is IAmPreRenderable subRender)
			{
				var preRenderService = this.Game.Services.GetService<IPreRenderService>();
				preRenderService.RemoveOverlaidPrerender(subRender);
			}
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
		/// Draws the active drawables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		override public void Draw(GameTime gameTime)
		{
			var drawingService = this.Game.Services.GetService<IDrawingService>();
			drawingService.BeginDraw();

			foreach (var kvp in this._runTimeCollection._activeModels)
			{
				this._runTimeCollection.CurrentKey = kvp.Key;

				foreach (var drawable in kvp.Value)
				{
					if (true == this._runTimeCollection._pendingRemovals.Contains(drawable))
						continue;

					drawable.Draw(gameTime, this.Game.Services);
				}

				this._runTimeCollection.ResolvePendingModels();
				this._runTimeCollection.CurrentKey = null;
			}

			base.Draw(gameTime);
			drawingService.EndDraw();
			this._runTimeCollection.ResolvePendingLists();
		}
	}
}
