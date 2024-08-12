namespace Engine.RunTime.Services.Contracts
{
	/// <summary>
	/// Represents a random service.
	/// </summary>
	public interface IRandomService
	{
		/// <summary>
		/// Gets a random int.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns>A random int.</returns>
		/// <returns>A random int.</returns>
		public int GetRandomInt(int lowerBound, int upperBound);
	}
}
