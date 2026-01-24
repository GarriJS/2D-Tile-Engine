using Engine.Debugging.Models.Contracts;
using Engine.RunTime.Models;

namespace Engine.Debugging.Models
{
	/// <summary>
	/// Represents a debug runtime collection.
	/// </summary>
	public class DebugRuntimeCollection
	{
		/// <summary>
		/// Gets the run time drawable collection.
		/// </summary>
		public RunTimeCollection<IAmDebugDrawable> RunTimeDrawableCollection { get; private set; }

		/// <summary>
		/// Gets the run time overlaid drawable collection.
		/// </summary>
		public RunTimeCollection<IAmDebugDrawable> RunTimeOverlaidDrawableCollection { get; private set; }

		/// <summary>
		/// Gets the run time updateable collection.
		/// </summary>
		public RunTimeCollection<IAmDebugUpdateable> RunTimeUpdateableCollection { get; private set; }
	}
}
