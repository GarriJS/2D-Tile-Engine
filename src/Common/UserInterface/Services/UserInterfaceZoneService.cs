using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.Core.Constants;
using Common.DiskModels.UserInterface;
using Common.UserInterface.Constants;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Engine.Core.Initialization.Services.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a user interface zone service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface zone service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	sealed public class UserInterfaceZoneService(GameServiceContainer gameServices) : IUserInterfaceZoneService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the user interface zone from the model.
		/// </summary>
		/// <param name="uiZoneModel">The user interface model.</param>
		/// <returns>The user interface zone.</returns>
		public UiZone GetUiZoneFromModel(UiZoneModel uiZoneModel)
		{
			var uiZoneService = this._gameServices.GetService<IUserInterfaceScreenZoneService>();
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var functionService = this._gameServices.GetService<IFunctionService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var cursorService = this._gameServices.GetService<ICursorService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var scrollStateService = this._gameServices.GetService<IScrollStateService>();
			var uiBlockService = this._gameServices.GetService<IUserInterfaceBlockService>();
			var uiRowService = this._gameServices.GetService<IUserInterfaceRowService>();

			if (false == uiZoneService.UserInterfaceScreenZones.TryGetValue(uiZoneModel.UiZonePositionType, out UiScreenZone uiScreenZone))
				uiScreenZone = uiZoneService.UserInterfaceScreenZones[UiZonePositionType.Unknown];

			var blocks = new List<UiBlock>();

			foreach (var uiBlockModel in uiZoneModel.Blocks ?? [])
			{
				var block = uiBlockService.GetUiBlockFromModel(uiBlockModel);
				blocks.Add(block);
			}

			var contentHeight = blocks.Sum(e => e.TotalHeight);
			var rows = blocks.Where(e => e._rows.Count != 0)
							 .SelectMany(e => e._rows)
							 .ToArray();
			var dynamicRows = rows.Where(r => r._elements.Any(e => UserInterfaceGroupService._dynamicSizedTypes.Contains(e.VerticalSizeType)))
								  .ToArray();
			var remainingHeight = uiScreenZone.Area.Height - contentHeight;
			var dynamicHeight = remainingHeight / dynamicRows.Length;

			if (uiScreenZone.Area.Height * ElementSizesScalars.ExtraSmall.Y > dynamicHeight)
			{
				// LOGGING
				dynamicHeight = uiScreenZone.Area.Height * ElementSizesScalars.ExtraSmall.Y;
			}

			foreach (var dynamicRow in dynamicRows ?? [])
				uiRowService.UpdateRowDynamicHeight(dynamicRow, dynamicHeight);

			if (((uiZoneModel.ScrollStateModel is null) ||
				 (true == uiZoneModel.ScrollStateModel.DisableScrolling)) &&
				(contentHeight > uiScreenZone.Area.Height))
			{
				var exessHeight = contentHeight - uiScreenZone.Area.Height;
				var scrollableBlocks = blocks.Where(e => false == e.ScrollState?.DisableScrolling)
											 .ToArray();
				var splitExessHeight = exessHeight / scrollableBlocks.Length;

				foreach (var scrollableBlock in scrollableBlocks)
					scrollableBlock.ScrollState.MaxVisibleHeight -= splitExessHeight;
			}

			IAmAGraphic background = null;

			if (uiZoneModel.BackgroundTexture is not null)
			{
				background = imageService.GetImageFromModel(uiZoneModel.BackgroundTexture);

				if ((true == uiZoneModel.ResizeTexture) ||
					(background is CompositeImage))
					background.SetDrawDimensions(uiScreenZone.Area.ToSubArea);
			}

			Cursor hoverCursor = null;

			if ((false == string.IsNullOrEmpty(uiZoneModel.HoverCursorName)) &&
				(false == cursorService.Cursors.TryGetValue(uiZoneModel.HoverCursorName, out hoverCursor)))
			{
				// LOGGING
			}

			var scrollState = scrollStateService.GetScrollStateFromModel(uiZoneModel.ScrollStateModel);
			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<UiZone>(uiScreenZone.Area.ToSubArea, null);
			var result = new UiZone
			{
				ResetCalculateCachedOffsets = true,
				Name = uiZoneModel.Name,
				DrawLayer = RunTimeConstants.BaseUiDrawLayer,
				VerticalJustificationType = uiZoneModel.VerticalJustificationType,
				Graphic = background,
				ScrollState = scrollState,
				HoverCursor = hoverCursor,
				CursorConfiguration = cursorConfiguration,
				UserInterfaceScreenZone = uiScreenZone
			};
			result._blocks.AddRange(blocks);

			return result;
		}
	}
}
