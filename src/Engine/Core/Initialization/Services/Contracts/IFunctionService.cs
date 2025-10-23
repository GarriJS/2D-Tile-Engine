using System;

namespace Engine.Core.Initialization.Services.Contracts
{
	/// <summary>
	/// Represents a function service.
	/// </summary>
	public interface IFunctionService
	{
		/// <summary>
		/// Tries to add the function.
		/// </summary>
		/// <param name="functionName">The function name.</param>
		/// <param name="function">The function.</param>
		/// <returns>A value indicating whether the function was added.</returns>
		internal bool TryAddFunction(string functionName, Delegate function);

		/// <summary>
		/// Tries to get the function.
		/// </summary>
		/// <param name="functionName">The function name.</param>
		/// <param name="function">The function.</param>
		/// <returns>A value indicating whether the function was found.</returns>
		public bool TryGetFunction(string functionName, out Delegate function);

		/// <summary>
		/// Tries to get the typed function.
		/// </summary>
		/// <typeparam name="T">The type of the function.</typeparam>
		/// <param name="functionName">The function name.</param>
		/// <param name="typedFunction">The typed function .</param>
		/// <returns>A value indicating whether the typed function was found.</returns>
		public bool TryGetFunction<T>(string functionName, out T typedFunction) where T : Delegate;
	}
}
