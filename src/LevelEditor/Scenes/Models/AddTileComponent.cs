using BaseContent.BaseContentConstants.Controls;
using Common.Controls.CursorInteraction.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.Controls.Models.Contracts;
using Common.DiskModels.Tiling;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Models;
using Engine.Controls.Models;
using Engine.Core.Constants;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Contracts;
using Engine.DiskModels.Physics;
using Engine.Graphics.Models;
using Engine.Graphics.Services.Contracts;
using Engine.RunTime.Services.Contracts;
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
		/// Gets a value indicating whether the background TileGraphic is active.
		/// </summary>
		public bool BackgroundGraphicActive { get; private set; }

		/// <summary>
		/// Gets or sets the add tile parameters.
		/// </summary>
		public AddTileParams AddTileParameters { get; set; }

		/// <summary>
		/// Gets the background TileGraphic.
		/// </summary>
		public IndependentGraphic BackgroundGraphic { get; private set; }

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
			//if ((true == this.BackgroundGraphicActive) &&
			//	(this.BackgroundGraphic.TileGraphic is TiledImage fillImage))
			//{
			//	var graphicDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();

			//	var FillBox = new Vector2
			//	{
			//		X = graphicDeviceService.GraphicsDevice.Viewport.InsideWidth,
			//		Y = graphicDeviceService.GraphicsDevice.Viewport.InsideHeight
			//	};

			//	if (fillImage.FillBox != FillBox)
			//	{ 
			//		fillImage.FillBox = FillBox;
			//	}
			//}

			if (false == controlState.ActionNameIsActive(BaseControlNames.LeftClick))
			{
				return;
			}

			var sceneEditService = this._gameServices.GetService<ISceneEditService>();

			if ((null == sceneEditService.CurrentScene )||
				(null == this.AddTileParameters) ||
				((null != hoverState) &&
				 ((hoverState.HoverObjectLocation.Object is not UiZone uiZone) ||
				  (null != uiZone.Graphic))))
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
				Graphic = this.AddTileParameters.TileGraphic,
			};

			var tile = tileService.GetTileFromModel(tileModel);
			sceneEditService.CurrentScene.TileMap.AddTile(1, tile);
		}

		/// <summary>
		/// Sets the background TileGraphic.
		/// </summary>
		/// <param name="textureRegionImageModel">The texture region image model.</param>
		public void SetBackgroundGraphic(IAmAGraphicModel textureRegionImageModel)
		{
			var independentGraphicService = this._gameServices.GetService<IIndependentGraphicService>();
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			var independentGraphicModel = new IndependentGraphicModel
			{
				Position = new PositionModel
				{
					X = 0,
					Y = 0,
				},
				Graphic = textureRegionImageModel
			};

			if ((null != this.BackgroundGraphic) &&
				(true == this.BackgroundGraphicActive))
			{
				runTimeOverlaidDrawService.RemoveDrawable(this.BackgroundGraphic);
			}

			this.BackgroundGraphic = independentGraphicService.GetIndependentGraphicFromModel(independentGraphicModel);

			if (true == this.BackgroundGraphicActive)
			{
				runTimeOverlaidDrawService.AddDrawable(this.BackgroundGraphic);
			}
		}

		/// <summary>
		/// Toggles the background TileGraphic.
		/// </summary>
		public void ToggleBackgroundGraphic()
		{
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (true == this.BackgroundGraphicActive)
			{
				runTimeOverlaidDrawService.RemoveDrawable(this.BackgroundGraphic);
			}
			else
			{
				runTimeOverlaidDrawService.AddDrawable(this.BackgroundGraphic);
			}

			this.BackgroundGraphicActive = false == this.BackgroundGraphicActive;
		}
	}
}
