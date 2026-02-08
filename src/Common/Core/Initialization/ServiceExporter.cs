using Common.Controls.CursorInteraction.Services;
using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Services;
using Common.Controls.Cursors.Services.Contracts;
using Common.Debugging.Services;
using Common.Debugging.Services.Contracts;
using Common.Tiling.Services;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Services;
using Common.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.Core.Initialization
{
	/// <summary>
	/// Represents a service exporter.
	/// </summary>
	static public class ServiceExporter
	{
		/// <summary>
		/// Gets the service contract pairs.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>The service contract pairs.</returns>
		static public (Type type, object provider)[] GetServiceContractPairs(Game game)
		{
			(Type type, object provider)[] result =
			[
				(typeof(ICommonDebugService), new CommonDebugService(game.Services)),
				(typeof(ICursorService), new CursorService(game.Services)),
				(typeof(ICursorInteractionService), new CursorInteractionService(game.Services)),
				(typeof(IScrollStateService), new ScrollStateService(game.Services)),
				(typeof(IUiMarginService), new UiMarginService(game.Services)),
				(typeof(IGraphicalTextWithMarginService), new GraphicalTextWithMarginService(game.Services)),
				(typeof(IUiScreenService), new UiScreenService(game.Services)),
				(typeof(IUiElementService), new UiElementService(game.Services)),
				(typeof(IUiRowService), new UiRowService(game.Services)),
				(typeof(IUiBlockService), new UiBlockService(game.Services)),
				(typeof(IUiZoneService), new UIZoneService(game.Services)),
				(typeof(IUiGroupService), new UiGroupService(game.Services)),
				(typeof(IUiModalService), new UiModalService(game.Services)),
				(typeof(IUiLocationService), new UiLocationService(game.Services)),
				(typeof(ITileService), new TileService(game.Services))
			];

			return result;
		}
	}
}
