namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a elapsed time extender.
	/// </summary>
	/// <typeparam name="T">The type being extended with elapsed time.</typeparam>
	readonly public struct ElaspedTimeExtender<T>
	{
		/// <summary>
		/// Gets or sets the elapsed time.
		/// </summary>
		required readonly public double ElaspedTime { get; init; }

		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		required readonly public T Subject { get; init; }
	}
}
