using Engine.Drawing.Models.Contracts;
using Engine.Drawing.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.RunTime.Managers
{
	/// <summary>
	/// Represents a runtime draw manager.
	/// </summary>
	public class RuntimeDrawManager : DrawableGameComponent, IRuntimeDrawService
	{
		/// <summary>
		/// Gets or sets the active drawables.
		/// </summary>
		private SortedDictionary<int, List<IAmDrawable>> ActiveDrawables { get; set; }
		
		/// <summary>
		/// Gets or sets the overlaid active drawables.
		/// </summary>
		private SortedDictionary<int, List<IAmDrawable>> OverlaidActiveDrawables { get; set; }

		/// <summary>
		/// Creates a new instance of the runtime draw manager.
		/// </summary>
		/// <param name="game">The game.</param>
		public RuntimeDrawManager(Game game) : base(game) 
		{
			this.ActiveDrawables = new SortedDictionary<int, List<IAmDrawable>>();
			this.OverlaidActiveDrawables = new SortedDictionary<int, List<IAmDrawable>>();
		}

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
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void AddDrawable(int layer, IAmDrawable drawable)
		{
			if (true == this.ActiveDrawables.TryGetValue(layer, out var layerList))
			{
				layerList.Add(drawable);
			}
			else
			{
				layerList = new List<IAmDrawable>() { drawable };
				this.ActiveDrawables.Add(layer, layerList);
			}
		}

		/// <summary>
		/// Adds the overlaid drawable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void AddOverlaidDrawable(int layer, IAmDrawable drawable)
		{
			if (true == this.OverlaidActiveDrawables.TryGetValue(layer, out var layerList))
			{
				layerList.Add(drawable);
			}
			else
			{
				layerList = new List<IAmDrawable>() { drawable };
				this.OverlaidActiveDrawables.Add(layer, layerList);
			}
		}

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawable(int layer, IAmDrawable drawable)
		{
			if (true == this.ActiveDrawables.TryGetValue(layer, out var layerList))
			{
				layerList.Remove(drawable);
			}
		}

		/// <summary>
		/// Removes the overlaid drawable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void RemoveOverlaidDrawable(int layer, IAmDrawable drawable)
		{
			if (true == this.OverlaidActiveDrawables.TryGetValue(layer, out var layerList))
			{
				layerList.Remove(drawable);
			}
		}

		/// <summary>
		/// Changes the drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeDrawableLayer(int layer, IAmDrawable drawable)
		{ 
			this.RemoveDrawable(layer, drawable);
			this.AddDrawable(layer, drawable);
		}

		/// <summary>
		/// Changes the overlaid drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeOverlaidDrawableLayer(int layer, IAmDrawable drawable)
		{
			this.RemoveOverlaidDrawable(layer, drawable);
			this.AddOverlaidDrawable(layer, drawable);
		}

		/// <summary>
		/// Draws the active drawables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Draw(GameTime gameTime)
		{
			var drawingService = this.Game.Services.GetService<IDrawingService>();

			foreach (var layer in this.ActiveDrawables.Values)
			{
				foreach (var drawable in layer)
				{ 
					drawingService.Draw(gameTime, drawable);
				}
			}

			foreach (var layer in this.OverlaidActiveDrawables.Values)
			{
				foreach (var drawable in layer)
				{
					drawingService.Draw(gameTime, drawable);
				}
			}

			base.Draw(gameTime);
		}
	}
}
