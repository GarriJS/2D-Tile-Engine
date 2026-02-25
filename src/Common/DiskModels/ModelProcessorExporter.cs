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
			var uiMarginService = gameServices.GetService<IUiMarginService>();
			var uiGraphicalTextWithMarginService = gameServices.GetService<ISimpleTextWithMarginService>();
			var uiElementService = gameServices.GetService<IUiElementService>();
			var uiGroupService = gameServices.GetService<IUiGroupService>();
			var uiZoneService = gameServices.GetService<IUiZoneService>();
			var uiBlockService = gameServices.GetService<IUiBlockService>();
			var uiRowService = gameServices.GetService<IUiRowService>();
			var uiModalService = gameServices.GetService<IUiModalService>();
			var cursorService = gameServices.GetService<ICursorService>();
			var scrollStateService = gameServices.GetService<IScrollStateService>();

			(Type type, Delegate factory)[] result =
			[
				(typeof(TileMapModel), tileService.GetTileFromModel),
				(typeof(TileMapLayerModel), tileService.GetTileMapLayerFromModel),
				(typeof(TileModel), tileService.GetTileFromModel),
				(typeof(UiMarginModel), uiMarginService.GetUiMarginFromModel),
				(typeof(SinmpleTextWithMarginModel), uiGraphicalTextWithMarginService.GetSimpleTextWithMarginFromModel),
				(typeof(UiGroupModel), uiGroupService.GetUiGroupFromModel),
				(typeof(UiZoneModel), uiZoneService.GetUiZoneFromModel),
				(typeof(UiBlockModel), uiBlockService.GetUiBlockFromModel),
				(typeof(UiRowModel), uiRowService.GetUiRowFromModel),
				(typeof(UiModalModel), uiModalService.GeActivetUiModalFromModel),
				(typeof(CursorModel), cursorService.GetCursorFromModel),
				(typeof(ScrollStateModel), scrollStateService.GetScrollStateFromModel)
			];

			return result;
		}
	}
}
