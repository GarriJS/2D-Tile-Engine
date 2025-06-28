using Engine.Core.Initialization.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.Core.Initialization
{
	/// <summary>
	/// Represents a function service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the function service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class FunctionService(GameServiceContainer gameServices) : IFunctionService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the functions.
		/// </summary>
		private Dictionary<string, Delegate> Functions { get; } = [];

		/// <summary>
		/// Tries to add the function.
		/// </summary>
		/// <param name="functionName">The function name.</param>
		/// <param name="function">The function.</param>
		/// <returns>A value indicating whether the function was added.</returns>
		public bool TryAddFunction(string functionName, Delegate function)
		{
			var added = this.Functions.TryAdd(functionName, function);

			return added;
		}

		/// <summary>
		/// Tries to get the function.
		/// </summary>
		/// <param name="functionName">The function name.</param>
		/// <returns>A value indicating whether the function was found.</returns>
		public bool TryGetFunction(string functionName, out Delegate function)
		{ 
			var found = this.Functions.TryGetValue(functionName, out function);

			return found;
		}
	}
}
