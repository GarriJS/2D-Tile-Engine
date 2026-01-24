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
		public int? CurrentKey { get; set; }

		/// <summary>
		/// Gets the active models.
		/// </summary>
		public SortedDictionary<int, List<T>> ActiveModels { get; private set; } = [];

		/// <summary>
		/// Gets the pending list adds.
		/// </summary>
		public SortedDictionary<int, List<T>> PendingListAdds { get; private set; } = [];

		/// <summary>
		/// Gets the pending list removals.
		/// </summary>
		public HashSet<int> PendingListRemovals { get; private set; } = [];

		/// <summary>
		/// Gets the pending adds.
		/// </summary>
		public HashSet<T> PendingAdds { get; private set; } = [];

		/// <summary>
		/// Gets the pending removals.
		/// </summary>
		public HashSet<T> PendingRemovals { get; private set; } = [];

		/// <summary>
		/// Gets the key function.
		/// </summary>
		public required Func<T, int> KeyFunction { get; init; }

		/// <summary>
		/// Adds the model to the runtime collection.
		/// </summary>
		/// <param name="model">The model being added.</param>
		public void AddModel(T model)
		{
			if (true == this.PendingAdds.Contains(model))
				return;

			var key = this.KeyFunction(model);

			if (false == this.ActiveModels.TryGetValue(key, out var modelList))
			{
				if (false == this.CurrentKey.HasValue)
					this.ActiveModels[key] = [model];
				else if (true == this.PendingListAdds.TryGetValue(key, out var pendingModelList))
				{
					if (false == pendingModelList.Contains(model))
						pendingModelList.Add(model);
				}
				else
					this.PendingListAdds[key] = [model];

				return;
			}

			if (key == this.CurrentKey)
			{
				if (true == this.PendingRemovals.Contains(model))
					this.PendingRemovals.Remove(model);
				else
					this.PendingAdds.Add(model);
			}
			else if (false == modelList.Contains(model))
			{
				modelList.Add(model);

				if (true == this.PendingListRemovals.Contains(key))
					this.PendingListRemovals.Remove(key);
			}
		}

		/// <summary>
		/// Removes the model.
		/// </summary>
		/// <param name="model">The model.</param>
		public void RemoveModel(T model)
		{
			if (true == this.PendingRemovals.Contains(model))
				return;

			var key = this.KeyFunction(model);

			if (false == this.ActiveModels.TryGetValue(key, out var modelList))
				return;

			if (key == this.CurrentKey)
			{
				if (true == this.PendingAdds.Contains(model))
					this.PendingAdds.Remove(model);
				else
					this.PendingRemovals.Add(model);
			}
			else if (true == modelList.Contains(model))
			{
				modelList.Remove(model);

				if (0 < modelList.Count)
					return;

				if (true == this.CurrentKey.HasValue)
					this.PendingListRemovals.Add(key);
				else
					this.ActiveModels.Remove(key);
			}
		}

		/// <summary>
		/// Resolves the pending models.
		/// </summary>		
		public void ResolvePendingModels()
		{
			if ((false == this.CurrentKey.HasValue) ||
				(false == this.ActiveModels.TryGetValue(this.CurrentKey.Value, out var modelList)))
			{
				this.ClearPendingModels();

				return;
			}

			modelList.AddRange(this.PendingAdds);
			modelList.RemoveAll(this.PendingRemovals.Contains);

			if (modelList.Count == 0)
				this.PendingListRemovals.Add(this.CurrentKey.Value);

			this.ClearPendingModels();
		}

		/// <summary>
		/// /Resolves the pending lists.
		/// </summary>
		public void ResolvePendingLists()
		{
			if (true == this.CurrentKey.HasValue)
				this.ClearPendingLists();

			foreach (var kvp in this.PendingListAdds)
				this.ActiveModels[kvp.Key] = kvp.Value;

			foreach (var key in this.PendingListRemovals)
				this.ActiveModels.Remove(key);

			this.ClearPendingLists();
		}

		/// <summary>
		/// Clears the pending models.
		/// </summary>
		private void ClearPendingModels()
		{
			this.PendingAdds.Clear();
			this.PendingRemovals.Clear();
		}

		/// <summary>
		/// Clears the pending lists.
		/// </summary>
		private void ClearPendingLists()
		{
			this.PendingListAdds.Clear();
			this.PendingListRemovals.Clear();
		}
	}
}