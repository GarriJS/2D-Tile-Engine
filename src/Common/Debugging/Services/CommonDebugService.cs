using Common.Debugging.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Debugging.Services
{
	/// <summary>
	/// Represents a common debug service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the common debug service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class CommonDebugService(GameServiceContainer gameServices) : ICommonDebugService
	{
		private readonly GameServiceContainer _gameServices = gameServices;
	}
}
