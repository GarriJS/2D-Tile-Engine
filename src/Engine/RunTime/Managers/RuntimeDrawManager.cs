using Engine.RunTime.Models.Contracts;
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
		/// Represents a drawable container.
		/// </summary>
		private class DrawableContainer
		{
			/// <summary>
			/// Gets or sets a value indicating whether the drawable is being removed.
			/// </summary>
			public bool IsBeingRemoved { get; set; }

			/// <summary>
			/// Gets or sets the drawable.
			/// </summary>
			public IAmDrawable Drawable { get; set; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether the draw is in progress.
		/// </summary>
		public bool DrawInProgress { get; set; } = false;

		/// <summary>
		/// Gets or sets a value indicating whether the overlaid draw is in progress.
		/// </summary>
		public bool OverlaidDrawInProgress { get; set; } = false;

		/// <summary>
		/// Gets or sets the active drawables.
		/// </summary>
		private SortedDictionary<int, List<DrawableContainer>> ActiveDrawables { get; set; } = [];

		/// <summary>
		/// Gets or sets the container mappings.
		/// </summary>
		private Dictionary<IAmDrawable, DrawableContainer> ContainerMappings { get; set; } = [];

		/// <summary>
		/// Gets or sets the pending adds.
		/// </summary>
		private HashSet<IAmDrawable> PendingAdds { get; set; } = [];

		/// <summary>
		/// Gets or sets the pending removals.
		/// </summary>
		private SortedDictionary<int, HashSet<DrawableContainer>> PendingRemovals { get; set; } = [];

		/// <summary>
		/// Gets or sets the overlaid active drawables.
		/// </summary>
		private SortedDictionary<int, List<DrawableContainer>> OverlaidActiveDrawables { get; set; } = [];

		/// <summary>
		/// Gets or sets the overlaid container mappings.
		/// </summary>
		private Dictionary<IAmDrawable, DrawableContainer> OverlaidContainerMappings { get; set; } = [];

		/// <summary>
		/// Gets or sets the overlaid pending adds.
		/// </summary>
		private HashSet<IAmDrawable> OverlaidPendingAdds { get; set; } = [];

		/// <summary>
		/// Gets or sets the overlaid pending removals.
		/// </summary>
		private SortedDictionary<int, HashSet<DrawableContainer>> OverlaidPendingRemovals { get; set; } = [];

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
		public void AddDrawable(IAmDrawable drawable)
		{
			if (true == this.ContainerMappings.TryGetValue(drawable, out var mappedContainer))
			{
				this.RemovePendingRemoval(mappedContainer);

				return;
			}

			if (true == this.DrawInProgress)
			{
				if (false == this.PendingAdds.Contains(drawable))
				{
					this.PendingAdds.Add(drawable);
				}

				return;
			}

			var container = new DrawableContainer
			{
				IsBeingRemoved = false,
				Drawable = drawable
			};

			this.ContainerMappings.Add(drawable, container);

			if (true == this.ActiveDrawables.TryGetValue(drawable.DrawLayer, out var orderList))
			{
				orderList.Add(container);
			}
			else
			{
				orderList = [container];
				this.ActiveDrawables.Add(drawable.DrawLayer, orderList);
			}
		}

		/// <summary>
		/// Adds the overlaid drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void AddOverlaidDrawable(IAmDrawable drawable)
		{
			if (true == this.OverlaidContainerMappings.TryGetValue(drawable, out var mappedContainer))
			{
				this.RemoveOverlaidPendingRemoval(mappedContainer);

				return;
			}

			if (true == this.OverlaidDrawInProgress)
			{
				if (false == this.OverlaidPendingAdds.Contains(drawable))
				{
					this.OverlaidPendingAdds.Add(drawable);
				}

				return;
			}

			var container = new DrawableContainer
			{
				IsBeingRemoved = false,
				Drawable = drawable
			};

			this.OverlaidContainerMappings.Add(drawable, container);

			if (true == this.OverlaidActiveDrawables.TryGetValue(drawable.DrawLayer, out var orderList))
			{
				orderList.Add(container);
			}
			else
			{
				orderList = [container];
				this.OverlaidActiveDrawables.Add(drawable.DrawLayer, orderList);
			}
		}

		/// <summary>
		/// Adds the pending removal.
		/// </summary>
		/// <param name="container">The container.</param>
		private void AddPendingRemoval(DrawableContainer container)
		{
			container.IsBeingRemoved = true;

			if (true == this.PendingRemovals.TryGetValue(container.Drawable.DrawLayer, out var orderList))
			{
				orderList.Add(container);
			}
			else
			{
				orderList = [container];
				this.PendingRemovals.Add(container.Drawable.DrawLayer, orderList);
			}
		}

		/// <summary>
		/// Adds the overlaid pending removal.
		/// </summary>
		/// <param name="container">The container.</param>
		private void AddOverlaidPendingRemoval(DrawableContainer container)
		{
			container.IsBeingRemoved = true;

			if (true == this.OverlaidPendingRemovals.TryGetValue(container.Drawable.DrawLayer, out var orderList))
			{
				orderList.Add(container);
			}
			else
			{
				orderList = [container];
				this.OverlaidPendingRemovals.Add(container.Drawable.DrawLayer, orderList);
			}
		}

		/// <summary>
		/// Removes the pending removal.
		/// </summary>
		/// <param name="container">The container.</param>
		private void RemovePendingRemoval(DrawableContainer container)
		{
			container.IsBeingRemoved = false;

			if (false == this.PendingRemovals.TryGetValue(container.Drawable.DrawLayer, out var removeOrderList))
			{
				return;
			}

			removeOrderList.Remove(container);

			if (0 == removeOrderList.Count)
			{
				this.PendingRemovals.Remove(container.Drawable.DrawLayer);
			}
		}

		/// <summary>
		/// Removes the pending removal.
		/// </summary>
		/// <param name="container">The container.</param>
		private void RemoveOverlaidPendingRemoval(DrawableContainer container)
		{
			container.IsBeingRemoved = false;

			if (false == this.OverlaidPendingRemovals.TryGetValue(container.Drawable.DrawLayer, out var removeOrderList))
			{
				return;
			}

			removeOrderList.Remove(container);

			if (0 == removeOrderList.Count)
			{
				this.OverlaidPendingRemovals.Remove(container.Drawable.DrawLayer);
			}
		}

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawable(IAmDrawable drawable)
		{
			this.PendingAdds.Remove(drawable);

			if ((false == this.ContainerMappings.TryGetValue(drawable, out var container)) ||
				(false == this.ActiveDrawables.TryGetValue(container.Drawable.DrawLayer, out var orderList)))
			{
				return;
			}

			if (false == this.DrawInProgress)
			{
				this.ContainerMappings.Remove(drawable);
				orderList.Remove(container);

				if (0 == orderList.Count)
				{
					this.ActiveDrawables.Remove(container.Drawable.DrawLayer);
				}

				return;
			}

			if (true == container.IsBeingRemoved)
			{
				return;
			}

			this.AddPendingRemoval(container);
		}

		/// <summary>
		/// Removes the overlaid drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveOverlaidDrawable(IAmDrawable drawable)
		{
			this.OverlaidPendingAdds.Remove(drawable);

			if ((false == this.OverlaidContainerMappings.TryGetValue(drawable, out var container)) ||
				(false == this.OverlaidActiveDrawables.TryGetValue(container.Drawable.DrawLayer, out var orderList)))
			{
				return;
			}

			if (false == this.OverlaidDrawInProgress)
			{
				this.OverlaidContainerMappings.Remove(drawable);
				orderList.Remove(container);

				if (0 == orderList.Count)
				{
					this.OverlaidActiveDrawables.Remove(container.Drawable.DrawLayer);
				}

				return;
			}

			if (true == container.IsBeingRemoved)
			{
				return;
			}

			this.AddOverlaidPendingRemoval(container);
		}

		/// <summary>
		/// Changes the drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeDrawableLayer(int layer, IAmDrawable drawable)
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
		public void ChangeOverlaidDrawableLayer(int layer, IAmDrawable drawable)
		{
			this.RemoveOverlaidDrawable(drawable);
			drawable.DrawLayer = layer;
			this.AddOverlaidDrawable(drawable);
		}

		/// <summary>
		/// Cleans up the pending removals.
		/// </summary>
		private void CleanUpPendingRemovals()
		{
			foreach (var kvp in this.PendingRemovals)
			{
				if (false == this.ActiveDrawables.TryGetValue(kvp.Key, out var correspondingList))
				{
					continue;
				}

				foreach (var removingContainer in kvp.Value)
				{
					this.ContainerMappings.Remove(removingContainer.Drawable);
					correspondingList.Remove(removingContainer);
				}
			}

			this.PendingRemovals.Clear();
		}

		/// <summary>
		/// Cleans up the pending removals.
		/// </summary>
		private void CleanUpOverlaidPendingRemovals()
		{
			foreach (var kvp in this.OverlaidPendingRemovals)
			{
				if (false == this.OverlaidActiveDrawables.TryGetValue(kvp.Key, out var correspondingList))
				{
					continue;
				}

				foreach (var removingContainer in kvp.Value)
				{
					this.OverlaidContainerMappings.Remove(removingContainer.Drawable);
					correspondingList.Remove(removingContainer);
				}
			}

			this.OverlaidPendingRemovals.Clear();
		}

		/// <summary>
		/// Draws the active drawables.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Draw(GameTime gameTime)
		{
			var drawingService = this.Game.Services.GetService<IDrawingService>();

			foreach (var pendingAdd in this.PendingAdds)
			{
				this.AddDrawable(pendingAdd);
			}

			this.PendingAdds.Clear();
			this.DrawInProgress = true;
			drawingService.BeginDraw();

			foreach (var layer in this.ActiveDrawables.Values)
			{
				foreach (var container in layer)
				{
					if (true == container.IsBeingRemoved)
					{ 
						continue;
					}

					container.Drawable.Draw(gameTime, this.Game.Services);
				}
			}

			base.Draw(gameTime);
			drawingService.EndDraw(); 
			this.DrawInProgress = false;
			this.CleanUpPendingRemovals();

			foreach (var pendingAdd in this.OverlaidPendingAdds)
			{
				this.AddOverlaidDrawable(pendingAdd);
			}

			this.OverlaidPendingAdds.Clear();
			this.OverlaidDrawInProgress = true;
			drawingService.BeginDraw();

			foreach (var layer in this.OverlaidActiveDrawables.Values)
			{
				foreach (var container in layer)
				{
					if (true == container.IsBeingRemoved)
					{
						continue;
					}

					container.Drawable.Draw(gameTime, this.Game.Services);
				}
			}

			this.OverlaidDrawInProgress = false;
			this.CleanUpOverlaidPendingRemovals();
			drawingService.EndDraw();
		}
	}
}
