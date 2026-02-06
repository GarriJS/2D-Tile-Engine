using Engine.RunTime.Services.Contracts;
using System;

namespace Engine.RunTime.Services
{
	/// <summary>
	/// Represents a random service.
	/// </summary>
	sealed public class RandomService : IRandomService
	{
		/// <summary>
		/// The random generator.
		/// </summary>
		readonly private Random _random = new();

		/// <summary>
		/// Gets a random int.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns>A random int.</returns>
		public int GetRandomInt(int lowerBound, int upperBound)
		{
			var result = this._random.Next(lowerBound, upperBound + 1);

			return result;
		}
	}
}
