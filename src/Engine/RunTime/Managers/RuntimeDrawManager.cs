using Engine.Drawables.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.RunTime.Managers
{
	/// <summary>
	/// Represents a runtime draw manager.
	/// </summary>
	/// <remarks>
	/// Creates a new instance of the runtime draw manager.
	/// </remarks>
	/// <param name="game">The game.</param>
	public class RuntimeDrawManager(Game game) : DrawableGameComponent(game), IRuntimeDrawService
	{
		/// <summary>
		/// Gets or sets the active drawables.
		/// </summary>
		private SortedDictionary<int, List<ICanBeDrawn>> ActiveDrawables { get; set; } = [];

		/// <summary>
		/// Gets or sets the overlaid active drawables.
		/// </summary>
		private SortedDictionary<int, List<ICanBeDrawn>> OverlaidActiveDrawables { get; set; } = [];

		/// <summary>
		/// Initializes the runtime draw manager.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
		}

		/// <summary>
		/// Adds the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void AddDrawable(ICanBeDrawn drawable)
		{
			if (true == this.ActiveDrawables.TryGetValue(drawable.DrawLayer, out var layerList))
			{
				layerList.Add(drawable);
			}
			else
			{
				layerList = [drawable];
				this.ActiveDrawables.Add(drawable.DrawLayer, layerList);
			}
		}

		/// <summary>
		/// Adds the overlaid drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void AddOverlaidDrawable(ICanBeDrawn drawable)
		{
			if (true == this.OverlaidActiveDrawables.TryGetValue(drawable.DrawLayer, out var layerList))
			{
				layerList.Add(drawable);
			}
			else
			{
				layerList = [drawable];
				this.OverlaidActiveDrawables.Add(drawable.DrawLayer, layerList);
			}
		}

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawable(ICanBeDrawn drawable)
		{
			if (true == this.ActiveDrawables.TryGetValue(drawable.DrawLayer, out var layerList))
			{
				layerList.Remove(drawable);
			}
		}

		/// <summary>
		/// Removes the overlaid drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveOverlaidDrawable(ICanBeDrawn drawable)
		{
			if (true == this.OverlaidActiveDrawables.TryGetValue(drawable.DrawLayer, out var layerList))
			{
				layerList.Remove(drawable);
			}
		}

		/// <summary>
		/// Changes the drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeDrawableLayer(int layer, ICanBeDrawn drawable)
		{ 
			this.RemoveDrawable(drawable);
			drawable.DrawLayer = layer;
			this.AddDrawable(drawable);
		}

		/// <summary>
		/// Changes the overlaid drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeOverlaidDrawableLayer(int layer, ICanBeDrawn drawable)
		{
			this.RemoveOverlaidDrawable(drawable);
			drawable.DrawLayer = layer;
			this.AddOverlaidDrawable(drawable);
		}

		/// <summary>
		/// Draws the active drawables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Draw(GameTime gameTime)
		{
			var drawingService = this.Game.Services.GetService<IDrawingService>();
			drawingService.BeginDraw();

			foreach (var layer in this.ActiveDrawables.Values)
			{
				foreach (var drawable in layer)
				{
					drawable.Draw(gameTime, this.Game.Services);
				}
			}

			drawingService.EndDraw();
			drawingService.BeginDraw();

			foreach (var layer in this.OverlaidActiveDrawables.Values)
			{
				foreach (var drawable in layer)
				{
					drawable.Draw(gameTime, this.Game.Services);
				}
			}

			base.Draw(gameTime);
			drawingService.EndDraw();
		}
	}
}
