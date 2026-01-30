using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.RunTime.Managers
{
	/// <summary>
	/// Represents a prerender manager.
	/// </summary>
	/// <param name="game">The game.</param>
	sealed public class PreRenderManager(Game game) : DrawableGameComponent(game), IPreRenderService
	{
		/// <summary>
		/// The prerenders.
		/// </summary>
		readonly public List<IRequirePreRender> PreRenders = [];

		/// <summary>
		/// The overlaid prerenders.
		/// </summary>
		readonly public List<IRequirePreRender> OverlaidPrerenders = [];

		/// <summary>
		/// Initializes the prerender manager.
		/// </summary>
		public override void Initialize()
		{
			this.DrawOrder = int.MinValue;
			base.Initialize();
		}

		/// <summary>
		/// Adds the prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void AddPrerender(IRequirePreRender prerender)
		{ 
			this.PreRenders.Add(prerender);
		}

		/// <summary>
		/// Removes the prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void RemovePrerender(IRequirePreRender prerender)
		{
			this.PreRenders.Remove(prerender);
		}

		/// <summary>
		/// Adds the overlaid prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void AddOverlaidPrerender(IRequirePreRender prerender)
		{
			this.PreRenders.Add(prerender);
		}

		/// <summary>
		/// Removes the overlaid prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void RemoveOverlaidPrerender(IRequirePreRender prerender)
		{
			this.PreRenders.Remove(prerender);
		}

		/// <summary>
		/// Draws the prerenders.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Draw(GameTime gameTime)
		{
			foreach (var subRender in this.PreRenders)
				if (true == subRender.ShouldPreRender())
					subRender.PreRender(gameTime, this.Game.Services);

			foreach (var subRender in this.OverlaidPrerenders)
				if (true == subRender.ShouldPreRender())
					subRender.PreRender(gameTime, this.Game.Services);

			this.GraphicsDevice.Clear(Color.CornflowerBlue);
			base.Draw(gameTime);
		}
	}
}
