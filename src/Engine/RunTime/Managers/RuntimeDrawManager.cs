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
		/// Gets or sets the active sorted draw data.
		/// </summary>
		private SortedDictionary<int, List<ICanBeDrawn>> ActiveSortedDrawData { get; set; }
		
		/// <summary>
		/// Creates a new instance of the runtime draw manager.
		/// </summary>
		/// <param name="game">The game.</param>
		public RuntimeDrawManager(Microsoft.Xna.Framework.Game game) : base(game) 
		{ 
		
		}

		/// <summary>
		/// Initializes the runtime draw manager.
		/// </summary>
		public override void Initialize()
		{
			this.ActiveSortedDrawData = new SortedDictionary<int, List<ICanBeDrawn>>();
			base.Initialize();
		}

		/// <summary>
		/// Adds the draw data.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void AddDrawData(int layer, ICanBeDrawn drawable)
		{
			if (true == this.ActiveSortedDrawData.TryGetValue(layer, out var layerList))
			{
				layerList.Add(drawable);
			}
			else
			{
				layerList = new List<ICanBeDrawn>() { drawable };
				this.ActiveSortedDrawData.Add(layer, layerList);
			}
		}

		/// <summary>
		/// Removes the draw data.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawData(int layer, ICanBeDrawn drawable)
		{
			if (true == this.ActiveSortedDrawData.TryGetValue(layer, out var layerList))
			{
				layerList.Remove(drawable);
			}
		}

		/// <summary>
		/// Changes the draw data layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeDrawDataLayer(int layer, ICanBeDrawn drawable)
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
