﻿using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteraction.Services
{
	/// <summary>
	/// Represents a cursor interaction service.
	/// </summary>   
	/// <remarks>
	/// Initialize the cursor interaction service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class CursorInteractionService(GameServiceContainer gameServices) : ICursorInteractionService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the hover configuration.
		/// </summary>
		/// <typeparam name="T">The parent type.</typeparam>
		/// <param name="area">The area.</param>
		/// <param name="hoverCursorName">The hover cursor name.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The hover configuration.</returns>
		public HoverConfiguration<T> GetHoverConfiguration<T>(SubArea area, string hoverCursorName, Vector2 offset = default)
		{
			var cursorService = this._gameServices.GetService<ICursorService>();

			hoverCursorName ??= string.Empty;
			if (false == cursorService.Cursors.TryGetValue(hoverCursorName, out var cursor))
			{
				// LOGGING
			}

			return new HoverConfiguration<T>
			{
				Area = area,
				Offset = offset,
				HoverCursor = cursor,
			};
		}

		/// <summary>
		/// Gets the press configuration.
		/// </summary>
		/// <typeparam name="T">The parent type.</typeparam>
		/// <param name="area">The area.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The press configuration.</returns>
		public PressConfiguration<T> GetPressConfiguration<T>(SubArea area, Vector2 offset = default)
		{
			return new PressConfiguration<T>
			{
				Area = area,
				Offset = offset
			};
		}

		/// <summary>
		/// Gets the click configuration.
		/// </summary>
		/// <typeparam name="T">The parent type.</typeparam>
		/// <param name="area">The area.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The click configuration.</returns>
		public ClickConfiguration<T> GetClickConfiguration<T>(SubArea area, Vector2 offset = default)
		{
			return new ClickConfiguration<T>
			{
				Area = area,
				Offset = offset
			};
		}
	}
}
