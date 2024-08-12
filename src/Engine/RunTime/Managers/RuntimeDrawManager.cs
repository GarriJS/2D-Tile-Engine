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
		/// Gets or sets the active sorted drawables.
		/// </summary>
		private SortedDictionary<int, List<IAmDrawable>> ActiveSortedDrawData { get; set; }
		
		/// <summary>
		/// Creates a new instance of the runtime draw manager.
		/// </summary>
		/// <param name="game">The game.</param>
		public RuntimeDrawManager(Game game) : base(game) 
		{ 
		
		}

		/// <summary>
		/// Initializes the runtime draw manager.
		/// </summary>
		public override void Initialize()
		{
			this.ActiveSortedDrawData = new SortedDictionary<int, List<IAmDrawable>>();
			base.Initialize();
		}

		/// <summary>
		/// Adds the drawable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void AddDrawData(int layer, IAmDrawable drawable)
		{
			if (true == this.ActiveSortedDrawData.TryGetValue(layer, out var layerList))
			{
				layerList.Add(drawable);
			}
			else
			{
				layerList = new List<IAmDrawable>() { drawable };
				this.ActiveSortedDrawData.Add(layer, layerList);
			}
		}

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawData(int layer, IAmDrawable drawable)
		{
			if (true == this.ActiveSortedDrawData.TryGetValue(layer, out var layerList))
			{
				layerList.Remove(drawable);
			}
		}

		/// <summary>
		/// Changes the drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeDrawDataLayer(int layer, IAmDrawable drawable)
		{ 
			this.RemoveDrawData(layer, drawable);
			this.AddDrawData(layer, drawable);
		}

		/// <summary>
		/// Draws the active drawables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Draw(GameTime gameTime)
		{
			var drawingService = this.Game.Services.GetService<IDrawingService>();

			foreach (var layer in this.ActiveSortedDrawData.Values)
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
