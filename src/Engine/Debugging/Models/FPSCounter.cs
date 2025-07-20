using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Debugging.Models
{
	/// <summary>
	/// Represents a FPS counter.
	/// </summary>
	internal class FpsCounter : IAmDrawable
	{
		/// <summary>
		/// A value describing whether the FPS counter is enabled.
		/// </summary>
		public bool IsActive { get; set; }

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
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			if (false == this.IsActive)
			{
				return;
			}

			var writingService = gameServices.GetService<IWritingService>();

			if (false == this.LastFrameTime.HasValue)
			{
				this.LastFrameTime = gameTime.TotalGameTime.TotalMilliseconds;
			}
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
