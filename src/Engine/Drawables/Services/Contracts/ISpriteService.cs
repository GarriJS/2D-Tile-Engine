using Engine.DiskModels.Drawing;
using Engine.Drawables.Models;

namespace Engine.Drawables.Services.Contracts
{
	/// <summary>
	/// Represents a sprite service.
	/// </summary>
	public interface ISpriteService
	{
		/// <summary>
		/// Gets the sprite.
		/// </summary>
		/// <param name="spriteModel">The sprite model.</param>
		/// <returns>The sprite.</returns>
		public Sprite GetSprite(SpriteModel spriteModel);
	}
}
