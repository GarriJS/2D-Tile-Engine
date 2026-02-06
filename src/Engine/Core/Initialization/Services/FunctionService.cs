using Engine.Core.Initialization.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.Core.Initialization.Services
{
	/// <summary>
	/// Represents a function service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the function service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	sealed public class FunctionService(GameServiceContainer gameServices) : IFunctionService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the functions.
		/// </summary>
		readonly private Dictionary<string, Delegate> Functions = [];

		/// <summary>
		/// Tries to add the function.
		/// </summary>
		/// <param name="functionName">The function name.</param>
		/// <param name="function">The function.</param>
		/// <returns>A value indicating whether the function was added.</returns>
		public bool TryAddFunction(string functionName, Delegate function)
		{
			var result = this.Functions.TryAdd(functionName, function);

			return result;
		}

		/// <summary>
		/// Tries to get the function.
		/// </summary>
		/// <param name="functionName">The function name.</param>
		/// <param name="function">The function.</param>
		/// <returns>A value indicating whether the function was found.</returns>
		public bool TryGetFunction(string functionName, out Delegate function)
		{ 
			var result = this.Functions.TryGetValue(functionName, out function);
			
			return result;
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
			if (true == string.IsNullOrEmpty(functionName))
			{
				typedFunction = null;

				return false;
			}

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
