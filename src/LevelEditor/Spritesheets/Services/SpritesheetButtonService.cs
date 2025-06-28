using Common.Controls.Models;
using Common.Controls.Models.Constants;
using Common.Controls.Services.Contracts;
using Common.DiskModels.UI.Elements;
using Common.Tiling.Services.Contracts;
using Common.UI.Models.Contracts;
using Engine.Controls.Services.Contracts;
using Engine.Core.Constants;
using Engine.Core.Textures.Contracts;
using LevelEditor.Controls.Models.Constants;
using LevelEditor.Core.Constants;
using LevelEditor.Spritesheets.Services.Contracts;
using Microsoft.Xna.Framework;

namespace LevelEditor.Spritesheets.Services
{
    /// <summary>
    /// Represents a user interface service.
    /// </summary>
    /// <remarks>
    /// Initializes the spritesheet button service.
    /// </remarks>
    /// <param name="gameServices">The game services.</param>
    public class SpritesheetButtonService(GameServiceContainer gameServices) : ISpritesheetButtonService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// The spritesheet button hover event processor.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void SpritesheetButtonHoverEventProcessor(IAmAUiElement element, Vector2 elementLocation)
		{
			var cursorService = this._gameServices.GetService<ICursorService>();

			if (null != cursorService.ActiveCursor?.HoverCursor)
			{ 
				cursorService.ActiveCursor.HoverCursor.IsActive = true;

				return;
			}

			if (false == cursorService.Cursors.TryGetValue(CommonCursorNames.PrimaryCursorName, out var primaryCursor))
			{
				return;
			}

			cursorService.SetActiveCursor(primaryCursor);
		}

		/// <summary>
		/// The spritesheet button click event processor.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void SpritesheetButtonClickEventProcessor(IAmAUiElement element, Vector2 elementLocation)
		{
			var cursorService = this._gameServices.GetService<ICursorService>();
			var tileService = this._gameServices.GetService<ITileService>();
			var controlService = this._gameServices.GetService<IControlService>();

			if ((false == cursorService.Cursors.TryGetValue(CommonCursorNames.TileGridCursorName, out var tileGridCursor)) ||
				(false == cursorService.Cursors.TryGetValue(CommonCursorNames.PrimaryCursorName, out var primaryCursor)))
			{
				return;
			}
			
			cursorService.SetActiveCursor(tileGridCursor);
			var position = controlService.ControlState.MouseState.Position.ToVector2();
			var localTileLocation = tileService.GetLocalTileCoordinates(position);

			var trailingCursor = new TrailingCursor
			{
				IsActive = true,
				TrailingCursorName = LevelEditorCursorNames.SpritesheetButtonCursorName,
				Offset = new Vector2(localTileLocation.X - position.X,
									 localTileLocation.Y - position.Y),
				Image = element.Image,
				CursorUpdater = this.SpritesheetButtonTrailingCursorUpdater,
			};

			tileGridCursor.TrailingCursors.Add(trailingCursor);
			tileGridCursor.Offset = new Vector2(localTileLocation.X - tileGridCursor.Position.X - ((tileGridCursor.Image.Texture.Width / 2) - (TileConstants.TILE_SIZE / 2)),
												localTileLocation.Y - tileGridCursor.Position.Y - ((tileGridCursor.Image.Texture.Height / 2) - (TileConstants.TILE_SIZE / 2)));

			if (null != tileGridCursor.HoverCursor)
			{
				tileGridCursor.HoverCursor.IsActive = true;

				var trailingHoverCursor = new TrailingCursor
				{
					IsActive = true,
					TrailingCursorName = LevelEditorCursorNames.SpritesheetButtonCursorName,
					Offset = new Vector2(tileGridCursor.HoverCursor.Image.Texture.Width,
										tileGridCursor.HoverCursor.Image.Texture.Height),
					Image = element.Image
				};

				tileGridCursor.HoverCursor.TrailingCursors.Add(trailingHoverCursor);
			}
		}

		/// <summary>
		/// Updates the spritesheet button trailing cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="trailingCursor">The trailing cursor.</param>
		/// <param name="gameTime">The game time.</param>
		private void SpritesheetButtonTrailingCursorUpdater(Cursor cursor, TrailingCursor trailingCursor, GameTime gameTime)
		{
			var controlService = this._gameServices.GetService<IControlService>();
			var tileService = this._gameServices.GetService<ITileService>();

			if (null == controlService.ControlState)
			{
				return;
			}

			var position = controlService.ControlState.MouseState.Position.ToVector2();
			var localTileLocation = tileService.GetLocalTileCoordinates(position);
			trailingCursor.Offset = new Vector2(localTileLocation.X - position.X,
												localTileLocation.Y - position.Y);
		}

		/// <summary>
		/// Gets the user interface buttons for the spritesheet.
		/// </summary>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <param name="spriteDimensions">The sprite dimensions.</param>
		/// <returns></returns>
		public UiButtonModel[][] GetUiButtonsForSpritesheet(string spritesheetName, Point spriteDimensions)
		{
			var textureService = this._gameServices.GetService<ITextureService>();
			
			if (true == string.IsNullOrEmpty(spritesheetName))
			{
				return null;
			}

			if (false == textureService.TryGetTexture(spritesheetName, out var texture))
			{
				return null;
			}	

			var horizontalSize = texture.Width / spriteDimensions.X;
			var verticalSize = texture.Height / spriteDimensions.Y;
			var buttons = new UiButtonModel[horizontalSize][];

			for (var i = 0; i < horizontalSize; i++)
			{
				buttons[i] = new UiButtonModel[verticalSize];

				for (var j = 0; j < verticalSize; j++)
				{
					var textureName = textureService.GetTextureName(spritesheetName, new Rectangle(i * spriteDimensions.X, j * spriteDimensions.Y, spriteDimensions.X, spriteDimensions.Y));
					
					if (false == textureService.TryGetTexture(textureName, out var buttonTexture))
					{
						continue;
					}

					buttons[i][j] = new UiButtonModel
					{
						UiElementName = textureName,
						LeftPadding = 2,
						RightPadding = 2,
						FixedSized = new Vector2(spriteDimensions.X, spriteDimensions.Y),
						BackgroundTextureName = textureName,
						Text = string.Empty,
						ClickableAreaScaler = new Vector2(1, 1),
						ButtonHoverEventName = UiEventNameConstants.SpritesheetButtonHover,
						ButtonClickEventName = UiEventNameConstants.SpritesheetButtonClick
					};
				}
			}

			return buttons;
		}
	}
}
