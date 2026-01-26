using Engine.Debugging.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Debugging.Models
{
	/// <summary>
	/// Represents a FPS counter.
	/// </summary>
	internal class FpsCounter : IAmDebugDrawable
	{
		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the FPS text.
		/// </summary>
		public string FpsText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the last frame time.
		/// </summary>
		public double? LastFrameTime { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the font.
		/// </summary>
		public SpriteFont Font { get; set; }

		/// <summary>
		/// Draws the debug drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices)
		{
			var writingService = gameServices.GetService<IWritingService>();

			if (false == this.LastFrameTime.HasValue)
				this.LastFrameTime = gameTime.TotalGameTime.TotalMilliseconds;
			else
			{
				var frameDelta = gameTime.TotalGameTime.TotalMilliseconds - this.LastFrameTime.Value;
				this.LastFrameTime = gameTime.TotalGameTime.TotalMilliseconds;
				this.FpsText = $"FPS: {Math.Round(1000.0 / frameDelta)}";
			}

			writingService.Draw(this.Font, this.FpsText, this.Position.Coordinates, Color.MonoGameOrange);
		}
	}
}
