using Engine.Drawables.Models.Contracts;
using Engine.Drawables.Services.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Debugging.Models
{
	/// <summary>
	/// Represents a performance rate counter.
	/// </summary>
	public class PerformanceRateCounter : ICanBeDrawn, ICanBeUpdated
	{
		/// <summary>
		/// A value describing whether the performance rate counter is enabled.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the update order.
		/// </summary>
		public int UpdateOrder { get; set; }

		/// <summary>
		/// Gets or sets the FPS text.
		/// </summary>
		public string FpsText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the TPS text.
		/// </summary>
		public string TpsText { get; set; } = string.Empty;

		/// <summary>
		/// Gets or sets the last frame time.
		/// </summary>
		public double? LastFrameTime { get; set; }

		/// <summary>
		/// Gets or sets the last tick time.
		/// </summary>
		public double? LastTickTime { get; set; }

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
			writingService.Draw(this.Font, this.TpsText, this.Position.Coordinates + new Vector2(0, 20), Color.MonoGameOrange);
		}

		/// <summary>
		/// Updates the updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Update(GameTime gameTime, GameServiceContainer gameServices)
		{
			if (false == this.IsActive)
			{
				return;
			}

			if (false == this.LastTickTime.HasValue)
			{
				this.LastTickTime = gameTime.TotalGameTime.TotalMilliseconds;
			}
			else
			{
				var updateDelta = gameTime.TotalGameTime.TotalMilliseconds - this.LastTickTime.Value;
				this.LastTickTime = gameTime.TotalGameTime.TotalMilliseconds;
				this.TpsText = $"TPS: {Math.Round(1000.0 / updateDelta)}";
			}
		}
	}
}
