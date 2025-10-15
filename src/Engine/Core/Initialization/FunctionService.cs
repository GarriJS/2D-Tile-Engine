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
			return this.Functions.TryAdd(functionName, function);
		}

		/// <summary>
		/// Tries to get the function.
		/// </summary>
		/// <param name="functionName">The function name.</param>
		/// <param name="function">The function.</param>
		/// <returns>A value indicating whether the function was found.</returns>
		public bool TryGetFunction(string functionName, out Delegate function)
		{ 
			return this.Functions.TryGetValue(functionName, out function);
		}

		/// <summary>
		/// Tries to get the typed function.
		/// </summary>
		/// <typeparam name="T">The type of the function.</typeparam>
		/// <param name="functionName">The function name.</param>
		/// <param name="typedFunction">The typed function .</param>
		/// <returns>A value indicating whether the typed function was found.</returns>
		public bool TryGetFunction<T>(string functionName, out T typedFunction)
			where T : Delegate
		{
			functionName ??= string.Empty;
			var found = this.Functions.TryGetValue(functionName, out var function);

			if ((true == found) &&
				(function is T))
			{
				typedFunction = function as T;

				return true;	
			}

			// LOGGING

			typedFunction = null;

			return false;
		}
	}
}
