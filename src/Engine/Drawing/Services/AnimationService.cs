using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Drawing.Services
{
	/// <summary>
	/// Represents a animation service.
	/// </summary>
	public class AnimationService : IAnimationService
	{
		private readonly GameServiceContainer _gameServiceContainer;

		/// <summary>
		/// Initializes a new instance of the animation service.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public AnimationService(GameServiceContainer gameServices)
		{
			this._gameServiceContainer = gameServices;
		}

		//public Animation GetAnimation(AnimationModel animation)
		//{ 
		
		//}



	}
}
