namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a elapsed time extender.
	/// </summary>
	/// <typeparam name="T">The type being extended with elapsed time.</typeparam>
	public struct ElaspedTimeExtender<T>
	{
		/// <summary>
		/// Gets or sets the elapsed time.
		/// </summary>
		required public double ElaspedTime { get; set; }

		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		required public T Subject { get; set; }
	}
}
