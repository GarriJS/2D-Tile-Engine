using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a user interface block service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface block service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	sealed public class UiBlockService(GameServiceContainer gameServices) : IUiBlockService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Get the user interface block from the model.
		/// </summary>
		/// <param name="uiBlockModel">The user interface block model.</param>
		/// <param name="outterArea">The encapsulating area of the block.</param>
		/// <returns>The user interface block.</returns>
		public UiBlock GetUiBlockFromModel(UiBlockModel uiBlockModel, SubArea outterArea = null)
		{
			var imageService = this._gameServices.GetService<IImageService>();
			var cursorService = this._gameServices.GetService<ICursorService>();
			var scrollStateService = this._gameServices.GetService<IScrollStateService>();
			var uiMarginService = this._gameServices.GetService<IUiMarginService>();
			var uiRowService = this._gameServices.GetService<IUiRowService>();
			var rows = new List<UiRow>();

			foreach (var rowModel in uiBlockModel.Rows ?? [])
			{
				var row = uiRowService.GetUiRowFromModel(rowModel);
				rows.Add(row);
			}

			var margin = uiMarginService.GetUiMarginFromModel(uiBlockModel.Margin); 
			IAmAGraphic background = null;

			if (uiBlockModel.BackgroundTexture is not null)
				background = imageService.GetImageFromModel(uiBlockModel.BackgroundTexture);

			Cursor hoverCursor = null;

			if ((false == string.IsNullOrEmpty(uiBlockModel.HoverCursorName)) &&
				(false == cursorService.Cursors.TryGetValue(uiBlockModel.HoverCursorName, out hoverCursor)))
			{
				// LOGGING
			}

			var scrollState = scrollStateService.GetScrollStateFromModel(uiBlockModel.ScrollStateModel);
			var result = new UiBlock
			{
				Name = uiBlockModel.Name,
				ResizeTexture = uiBlockModel.ResizeTexture,
				FlexRows = uiBlockModel.FlexRows,
				ExtendBackgroundToMargin = uiBlockModel.ExtendBackgroundToMargin,
				Margin = margin,
				HorizontalJustificationType = uiBlockModel.HorizontalJustificationType,
				VerticalJustificationType = uiBlockModel.VerticalJustificationType,
				Graphic = background,
				ScrollState = scrollState,
				HoverCursor = hoverCursor,
			};
			result._rows.AddRange(rows);

			return result;
		}
	}
}
