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
			var uiElementService = gameServices.GetService<IUserInterfaceElementService>();
			var uiService = gameServices.GetService<IUserInterfaceService>();
			var cursorService = gameServices.GetService<ICursorService>();

			(Type type, Delegate factory)[] result =
			[
				(typeof(TileMapModel), tileService.GetTileFromModel),
				(typeof(TileMapLayerModel), tileService.GetTileMapLayerFromModel),
				(typeof(TileModel), tileService.GetTileFromModel),
				(typeof(UiPadding), uiElementService.GetUiPaddingFromModel),
				(typeof(UiGroupModel), uiService.GetUiGroup),
				(typeof(CursorModel), cursorService.GetCursor)
			];

			return result;
		}
	}
}
