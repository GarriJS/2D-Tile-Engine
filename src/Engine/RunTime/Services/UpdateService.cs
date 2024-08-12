using Engine.Drawing.Models;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.RunTime.Services
{
    /// <summary>
    /// Represents a update service.
    /// </summary>
    public class UpdateService : IUpdateService
    {
        private readonly GameServiceContainer _gameServiceContainer;

        /// <summary>
        /// Initializes a new instance of the update service.
        /// </summary>
        /// <param name="gameServices"></param>
        public UpdateService(GameServiceContainer gameServices)
        {
            this._gameServiceContainer = gameServices;
        }

        /// <summary>
        /// Updates the updateable.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="updateable">The updateable.</param>
        public void Update(GameTime gameTime, ICanBeUpdated updateable)
        {
            throw new System.InvalidCastException();
        }

        /// <summary>
        /// Updates the animation.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="animation">The animation.</param>
        public void Update(GameTime gameTime, Animation animation)
        {
			if (null == animation.FrameStartTime)
			{
				animation.FrameStartTime = gameTime.TotalGameTime.TotalMilliseconds;
				
                return;
			}

			if (animation.FrameStartTime + animation.FrameDuration <= gameTime.TotalGameTime.TotalMilliseconds)
			{
                if ((true == animation.FrameMinDuration.HasValue) && 
                    (true == animation.FrameMaxDuration.HasValue))
                {
                    var randomService = this._gameServiceContainer.GetService<RandomService>();
					animation.FrameDuration = randomService.GetRandomInt(animation.FrameMinDuration.Value, animation.FrameMaxDuration.Value);
                }
                else
                {
					animation.FrameDuration = animation.FrameDuration;
                }

				if (animation.CurrentFrameIndex < animation.Frames.Length - 1)
				{
					animation.CurrentFrameIndex++;
				}
				else
				{
					animation.CurrentFrameIndex = 0;
				}

				animation.FrameStartTime = gameTime.TotalGameTime.TotalMilliseconds;
			}
		}
    }
}
