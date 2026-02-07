using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.Controls;
using Common.DiskModels.Tiling;
using Common.DiskModels.UserInterface;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.DiskModels
{
	/// <summary>
	/// Represents a model processor exporter.
	/// </summary>
	static public class ModelProcessorExporter
	{
		/// <summary>
		/// Gets the model processing mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The model processing mappings.</returns>
		static public (Type type, Delegate factory)[] GetModelProcessingMappings(GameServiceContainer gameServices)
		{
			var tileService = gameServices.GetService<ITileService>();
			var uiMarginService = gameServices.GetService<IUserInterfaceMarginService>();
			var uiGraphicalTextWithMarginService = gameServices.GetService<IUserInterfaceGraphicalTextWithMarginService>();
			var uiElementService = gameServices.GetService<IUserInterfaceElementService>();
			var uiGroupService = gameServices.GetService<IUserInterfaceGroupService>();
			var uiZoneService = gameServices.GetService<IUserInterfaceZoneService>();
			var uiBlockService = gameServices.GetService<IUserInterfaceBlockService>();
			var uiRowService = gameServices.GetService<IUserInterfaceRowService>();
			var cursorService = gameServices.GetService<ICursorService>();
			var scrollStateService = gameServices.GetService<IScrollStateService>();

			(Type type, Delegate factory)[] result =
			[
				(typeof(TileMapModel), tileService.GetTileFromModel),
				(typeof(TileMapLayerModel), tileService.GetTileMapLayerFromModel),
				(typeof(TileModel), tileService.GetTileFromModel),
				(typeof(UiMarginModel), uiMarginService.GetUiMarginFromModel),
				(typeof(GraphicalTextWithMarginModel), uiGraphicalTextWithMarginService.GetGraphicTextWithMarginFromModel),
				(typeof(UiGroupModel), uiGroupService.GetUiGroupFromModel),
				(typeof(UiZoneModel), uiZoneService.GetUiZoneFromModel),
				(typeof(UiBlockModel), uiBlockService.GetUiBlockFromModel),
				(typeof(UiRowModel), uiRowService.GetUiRowFromModel),
				(typeof(CursorModel), cursorService.GetCursorFromModel),
				(typeof(ScrollStateModel), scrollStateService.GetScrollStateFromModel)
			];

			return result;
		}
	}
}
