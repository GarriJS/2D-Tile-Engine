using System;

namespace Game.RunTime.Services
{
	/// <summary>
	/// Represents a random service.
	/// </summary>
	public class RandomService
	{
		/// <summary>
		/// Gets or sets the random generator.
		/// </summary>
		private Random Random { get; }

		/// <summary>
		/// Initializes a new instance of the random service.
		/// </summary>
		public RandomService()
		{ 
			this.Random = new Random();
		}

		/// <summary>
		/// Gets a random int.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns>A random int.</returns>
		public int GetRandomInt(int lowerBound, int upperBound)
		{
			return this.Random.Next(lowerBound, upperBound + 1);
		}
	}
}
