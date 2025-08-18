using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.Controls;
using Common.DiskModels.UI.Elements;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Models.Contracts;
using Engine.Controls.Services.Contracts;
using Engine.Core.Textures.Contracts;
using LevelEditor.Controls.Constants;
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
		/// The spritesheet button click event processor.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void SpritesheetButtonClickEventProcessor(IAmAUiElement element, Vector2 elementLocation)
		{
			var cursorService = this._gameServices.GetService<ICursorService>();

			if (false == cursorService.Cursors.TryGetValue(CommonCursorNames.TileGridCursorName, out var tileGridCursor))
			{
				// LOGGING
				return;
			}

			var tileService = this._gameServices.GetService<ITileService>();
			var controlService = this._gameServices.GetService<IControlService>();

			cursorService.SetPrimaryCursor(tileGridCursor);
			var position = controlService.ControlState.MousePosition;
			var localTileLocation = tileService.GetLocalTileCoordinates(position);

			var secondaryCursorModel = new CursorModel
			{
				CursorName = LevelEditorCursorNames.SpritesheetButtonCursorName,
				TextureBox = element.Graphic.TextureBox,
				AboveUi = false,
				TextureName = element.Graphic.TextureName,
				Offset = new Vector2
				{
					X = localTileLocation.X - position.X,
					Y = localTileLocation.Y - position.Y
				},
				CursorUpdaterName = LevelEditorCursorUpdatersNames.SpritesheetButtonCursorUpdater
			};

			var secondaryCursor = cursorService.GetCursor(secondaryCursorModel, addCursor: false);
			cursorService.AddSecondaryCursor(secondaryCursor, disableExisting: true);

			if (false == cursorService.Cursors.TryGetValue(CommonCursorNames.BasicCursorName, out var primaryCursor))
			{
				// LOGGING
				return;
			}

			var secondaryHoverCursorModel = new CursorModel
			{
				CursorName = LevelEditorCursorNames.SpritesheetButtonCursorName,
				TextureBox = element.Graphic.TextureBox,
				AboveUi = true,
				TextureName = element.Graphic.TextureName,
				Offset = new Vector2
				{
					X = (primaryCursor?.TextureBox.Width ?? 25) + 1,
					Y = (primaryCursor?.TextureBox.Height ?? 25) + 1
				},
				CursorUpdaterName = CommonCursorUpdatersNames.BasicCursorUpdater
			};

			var secondaryHoverCursor = cursorService.GetCursor(secondaryHoverCursorModel, addCursor: false);
			cursorService.AddSecondaryHoverCursor(secondaryHoverCursor, disableExisting: true);
		}

		/// <summary>
		/// Updates the spritesheet button trailing cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>\
		/// <param name="gameTime">The game time.</param>
		public void SpritesheetButtonCursorUpdater(Cursor cursor, GameTime gameTime)
		{
			var tileService = this._gameServices.GetService<ITileService>();

			var localTileLocation = tileService.GetLocalTileCoordinates(cursor.Position.Coordinates);
			cursor.Offset = new Vector2
			{
				X = localTileLocation.X - cursor.Position.Coordinates.X,
				Y = localTileLocation.Y - cursor.Position.Coordinates.Y
			};
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
					var textureName = textureService.GetTextureName(
						spritesheetName,
						new Rectangle
						{
							X = i * spriteDimensions.X,
							Y = j * spriteDimensions.Y,
							Width = spriteDimensions.X,
							Height = spriteDimensions.Y
						});

					if (false == textureService.TryGetTexture(textureName, out var buttonTexture))
					{
						continue;
					}

					buttons[i][j] = new UiButtonModel
					{
						UiElementName = textureName,
						LeftPadding = 2,
						RightPadding = 2,
						FixedSized = new Vector2
						{
							X = spriteDimensions.X,
							Y = spriteDimensions.Y
						},
						BackgroundTextureName = textureName,
						ButtonText = string.Empty,
						ClickableAreaScaler = new Vector2
						{
							X = 1,
							Y = 1
						},
						ButtonClickEventName = UiEventName.SpritesheetButtonClick
					};
				}
			}

			return buttons;
		}
	}
}
