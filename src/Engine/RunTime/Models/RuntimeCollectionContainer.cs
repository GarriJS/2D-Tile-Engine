using System;
using System.Collections.Generic;

namespace Engine.RunTime.Models
{
	/// <summary>
	/// Represents a run time collection.
	/// </summary>
	/// <typeparam name="T">The model type.</typeparam>
	public class RunTimeCollection<T>
	{
		/// <summary>
		/// Gets the current key.
		/// </summary>
		required public int? CurrentKey { get; set; }

		/// <summary>
		/// Gets the active models.
		/// </summary>
		readonly public SortedDictionary<int, List<T>> _activeModels = [];

		/// <summary>
		/// Gets the pending list adds.
		/// </summary>
		readonly public SortedDictionary<int, List<T>> _pendingListAdds = [];

		/// <summary>
		/// Gets the pending list removals.
		/// </summary>
		readonly public HashSet<int> _pendingListRemovals = [];

		/// <summary>
		/// Gets the pending adds.
		/// </summary>
		readonly public HashSet<T> _pendingAdds = [];

		/// <summary>
		/// Gets the pending removals.
		/// </summary>
		readonly public HashSet<T> _pendingRemovals = [];

		/// <summary>
		/// Gets the key function.
		/// </summary>
		required public Func<T, int> KeyFunction { get; init; }

		/// <summary>
		/// Adds the model to the runtime collection.
		/// </summary>
		/// <param name="model">The model being added.</param>
		public void AddModel(T model)
		{
			if (true == this._pendingAdds.Contains(model))
				return;

			var key = this.KeyFunction(model);

			if (false == this._activeModels.TryGetValue(key, out var modelList))
			{
				if (false == this.CurrentKey.HasValue)
					this._activeModels[key] = [model];
				else if (true == this._pendingListAdds.TryGetValue(key, out var pendingModelList))
				{
					if (false == pendingModelList.Contains(model))
						pendingModelList.Add(model);
				}
				else
					this._pendingListAdds[key] = [model];

				return;
			}

			if (key == this.CurrentKey)
			{
				if (true == this._pendingRemovals.Contains(model))
					this._pendingRemovals.Remove(model);
				else
					this._pendingAdds.Add(model);
			}
			else if (false == modelList.Contains(model))
			{
				modelList.Add(model);

				if (true == this._pendingListRemovals.Contains(key))
					this._pendingListRemovals.Remove(key);
			}
		}

		/// <summary>
		/// Removes the model.
		/// </summary>
		/// <param name="model">The model.</param>
		public void RemoveModel(T model)
		{
			if (true == this._pendingRemovals.Contains(model))
				return;

			var key = this.KeyFunction(model);

			if (false == this._activeModels.TryGetValue(key, out var modelList))
				return;

			if (key == this.CurrentKey)
			{
				if (true == this._pendingAdds.Contains(model))
					this._pendingAdds.Remove(model);
				else
					this._pendingRemovals.Add(model);
			}
			else if (true == modelList.Contains(model))
			{
				modelList.Remove(model);

				if (0 < modelList.Count)
					return;

				if (true == this.CurrentKey.HasValue)
					this._pendingListRemovals.Add(key);
				else
					this._activeModels.Remove(key);
			}
		}

		/// <summary>
		/// Resolves the pending models.
		/// </summary>		
		public void ResolvePendingModels()
		{
			if ((false == this.CurrentKey.HasValue) ||
				(false == this._activeModels.TryGetValue(this.CurrentKey.Value, out var modelList)))
			{
				this.ClearPendingModels();

				return;
			}

			modelList.AddRange(this._pendingAdds);
			modelList.RemoveAll(this._pendingRemovals.Contains);

			if (modelList.Count == 0)
				this._pendingListRemovals.Add(this.CurrentKey.Value);

			this.ClearPendingModels();
		}

		/// <summary>
		/// /Resolves the pending lists.
		/// </summary>
		public void ResolvePendingLists()
		{
			if (true == this.CurrentKey.HasValue)
				this.ClearPendingLists();

			foreach (var kvp in this._pendingListAdds)
				this._activeModels[kvp.Key] = kvp.Value;

			foreach (var key in this._pendingListRemovals)
				this._activeModels.Remove(key);

			this.ClearPendingLists();
		}

		/// <summary>
		/// Clears the pending models.
		/// </summary>
		private void ClearPendingModels()
		{
			this._pendingAdds.Clear();
			this._pendingRemovals.Clear();
		}

		/// <summary>
		/// Clears the pending lists.
		/// </summary>
		private void ClearPendingLists()
		{
			this._pendingListAdds.Clear();
			this._pendingListRemovals.Clear();
		}
	}
}