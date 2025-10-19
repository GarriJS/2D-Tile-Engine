using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.Controls;
using Common.DiskModels.Tiling;
using Common.DiskModels.UI;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.DiskModels
{
	/// <summary>
	/// Represents a model processor exporter.
	/// </summary>
	public static class ModelProcessorExporter
	{
		/// <summary>
		/// Gets the model processing mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The model processing mappings.</returns>
		public static (Type typeIn, Delegate)[] GetModelProcessingMappings(GameServiceContainer gameServices)
		{
			var tileService = gameServices.GetService<ITileService>();
			var uiElementService = gameServices.GetService<IUserInterfaceElementService>();
			var uiService = gameServices.GetService<IUserInterfaceService>();
			var cursorService = gameServices.GetService<ICursorService>();

			return
			[
				(typeof(AnimatedTileModel), tileService.GetTile),
				(typeof(TileMapLayerModel), tileService.GetTileMapLayer),
				(typeof(TileMapModel), tileService.GetTileMap),
				(typeof(TileModel), tileService.GetTile),
				(typeof(UiPadding), uiElementService.GetUiPaddingFromModel),
				(typeof(UiGroupModel), uiService.GetUiGroup),
				(typeof(CursorModel), cursorService.GetCursor)
			];
		}
	}
}
