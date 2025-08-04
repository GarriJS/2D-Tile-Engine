using System;
using System.Collections.Generic;

namespace Engine.RunTime.Models
{
	/// <summary>
	/// Represents a run time collection.
	/// </summary>
	/// <typeparam name="T">The model type.</typeparam>
	internal class RunTimeCollection<T>
	{
		/// <summary>
		/// Gets the current key.
		/// </summary>
		public int? CurrentKey { get; set; }

		/// <summary>
		/// Gets or sets the active models.
		/// </summary>
		public SortedDictionary<int, List<T>> ActiveModels { get; private set; } = [];

		/// <summary>
		/// Gets or sets the pending adds.
		/// </summary>
		public HashSet<T> PendingAdds { get; private set; } = [];

		/// <summary>
		/// Gets or sets the pending removals.
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
			{
				return;
			}

			var key = this.KeyFunction(model);

			if (false == this.ActiveModels.TryGetValue(key, out var modelList))
			{
				this.ActiveModels[key] = [model];

				return;
			}

			if (key == this.CurrentKey)
			{
				if (true == this.PendingRemovals.Contains(model))
				{
					this.PendingRemovals.Remove(model);
				}
				else
				{
					this.PendingAdds.Add(model);
				}
			}
			else if (false == modelList.Contains(model))
			{
				modelList.Add(model);
			}
		}

		/// <summary>
		/// Removes the model.
		/// </summary>
		/// <param name="model">The model.</param>
		public void RemoveModel(T model)
		{
			if (true == this.PendingRemovals.Contains(model))
			{
				return;
			}

			var key = this.KeyFunction(model);

			if (false == this.ActiveModels.TryGetValue(key, out var modelList))
			{
				return;
			}

			if (key == this.CurrentKey)
			{
				if (true == this.PendingAdds.Contains(model))
				{
					this.PendingAdds.Remove(model);
				}
				else
				{
					this.PendingRemovals.Add(model);
				}
			}
			else if (true == modelList.Contains(model))
			{
				modelList.Remove(model);

				if (0 == modelList.Count)
				{
					this.ActiveModels.Remove(key);
				}
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
			{
				this.ActiveModels.Remove(this.CurrentKey.Value);
			}

			this.ClearPendingModels();
		}

		/// <summary>
		/// Clears the pending.
		/// </summary>
		private void ClearPendingModels()
		{ 
			this.PendingAdds.Clear();
			this.PendingRemovals.Clear();
		}
	}
}