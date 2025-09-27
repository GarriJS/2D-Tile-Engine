using BaseContent.BaseContent.Controls;
using Common.Controls.CursorInteraction.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.Controls.Models.Contracts;
using Common.DiskModels.Common.Tiling;
using Common.Tiling.Services.Contracts;
using Engine.Controls.Models;
using Engine.Core.Constants;
using LevelEditor.Scenes.Services.Contracts;
using Microsoft.Xna.Framework;

namespace LevelEditor.Scenes.Models
{
	/// <summary>
	/// Represents a add tile component.
	/// </summary>
	/// <remarks>
	/// Initializes a add tile control component.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class AddTileComponent(GameServiceContainer gameServices) : IAmACursorControlContextComponent
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets the add tile parameters.
		/// </summary>
		public AddTileParams AddTileParameters { get; set; }

		/// <summary>
		/// Consumes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		public void ConsumeControlState(GameTime gameTime, ControlState controlState, ControlState priorControlState)
		{
			var cursorService = this._gameServices.GetService<ICursorService>();

			var hoverState = cursorService.GetCursorHoverState();
			this.ConsumeControlState(gameTime, controlState, priorControlState, hoverState);
		}

		/// <summary>
		/// Consumes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		/// <param name="hoverState">The hover state.</param>
		public void ConsumeControlState(GameTime gameTime, ControlState controlState, ControlState priorControlState, HoverState hoverState)
		{
			if (false == controlState.ActionNameIsFresh(BaseControlNames.LeftClick))
			{
				return;
			}

			var sceneEditService = this._gameServices.GetService<ISceneEditService>();

			if ((null == sceneEditService.CurrentScene )||
				(null == this.AddTileParameters) ||
				(null != hoverState))
			{
				return;
			}

			var cursorService = this._gameServices.GetService<ICursorService>();
			var tileService = this._gameServices.GetService<ITileService>();

			var tilePosition = tileService.GetLocalTileCoordinates(cursorService.CursorControlComponent.CursorPosition.Coordinates);
			var tileModel = new TileModel
			{
				Row = (int)tilePosition.Y / TileConstants.TILE_SIZE,
				Column = (int)tilePosition.X / TileConstants.TILE_SIZE,
				Sprite = this.AddTileParameters.Sprite,
			};

			var tile = tileService.GetTile(tileModel);
			sceneEditService.CurrentScene.TileMap.AddTile(1, tile);
		}
	}
}
