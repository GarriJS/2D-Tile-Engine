using Engine.Debugging.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Debugging.Models
{
	/// <summary>
	/// Represents a TPS counter.
	/// </summary>
	internal class TpsCounter : IAmDebugDrawable, IAmDebugUpdateable
	{
		/// <summary>
		/// The draw offset.
		/// </summary>
		private readonly Vector2 Offset = new()
		{
			X = 0,
			Y = 20
		};

		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the update order.
		/// </summary>
		public int UpdateOrder { get; set; }

		/// <summary>
		/// Gets or sets the TPS text.
		/// </summary>
		public string TpsText { get; set; } = string.Empty;

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
		/// Draws the debug drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices)
		{
			var writingService = gameServices.GetService<IWritingService>();
			writingService.Draw(this.Font, this.TpsText, this.Position.Coordinates + Offset, Color.MonoGameOrange);
		}

		/// <summary>
		/// Updates the debug updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void UpdateDebug(GameTime gameTime, GameServiceContainer gameServices)
		{
			if (false == this.LastTickTime.HasValue)
				this.LastTickTime = gameTime.TotalGameTime.TotalMilliseconds;
			else
			{
				var updateDelta = gameTime.TotalGameTime.TotalMilliseconds - this.LastTickTime.Value;
				this.LastTickTime = gameTime.TotalGameTime.TotalMilliseconds;
				this.TpsText = $"TPS: {Math.Round(1000.0 / updateDelta)}";
			}
		}
	}
}
