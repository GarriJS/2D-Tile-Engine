using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.Controls;
using Common.DiskModels.UI;
using Common.DiskModels.UI.Elements;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Services.Contracts;
using Engine.Controls.Services.Contracts;
using Engine.Core.Textures.Contracts;
using Engine.DiskModels.Drawing;
using LevelEditor.Controls.Constants;
using LevelEditor.Core.Constants;
using LevelEditor.Scenes.Models;
using LevelEditor.Scenes.Services.Contracts;
using LevelEditor.Spritesheets.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Linq;

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

			cursorService.CursorControlComponent.SetPrimaryCursor(tileGridCursor, maintainHoverState: true);
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

			var secondaryCursor = cursorService.GetCursor(secondaryCursorModel, addCursor: false, drawLayerOffset: 1);
			cursorService.CursorControlComponent.AddSecondaryCursor(secondaryCursor, disableExisting: true);

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
					X = (primaryCursor?.TextureBox.Width ?? 25) + 3,
					Y = 0
				}
			};

			var secondaryHoverCursor = cursorService.GetCursor(secondaryHoverCursorModel, addCursor: false);
			cursorService.CursorControlComponent.AddSecondaryHoverCursor(secondaryHoverCursor, disableExisting: true);

			var sceneEditService = this._gameServices.GetService<ISceneEditService>();
			
			var addTileParams = new AddTileParams
			{
				Sprite = new ImageModel
				{
					TextureName = element.Graphic.TextureName,
					TextureBox = element.Graphic.TextureBox
				}
			};

			sceneEditService.AddTileComponent.AddTileParameters = addTileParams;
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
		/// Gets the user interface zone for the spritesheet buttons.
		/// </summary>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <param name="backgroundTexture">The background texture.</param>
		/// <param name="uiScreenZoneType">The user interface zone type.</param>
		/// <returns>The user interface zone.</returns>
		public UiZone GetUiZoneForSpritesheet(string spritesheetName, string backgroundTexture, UiScreenZoneTypes uiScreenZoneType)
		{
			var uiService = this._gameServices.GetService<IUserInterfaceService>();
			
			var spritesheetButtons = this.GetUiButtonsForSpritesheet(spritesheetName, new Point(32, 32));
			var flattenedButtons = spritesheetButtons?.SelectMany(row => row).ToArray();

			var uiZoneModel = new UiZoneModel
			{
				UiZoneName = "Spritesheet Buttons Zone",
				UiZoneType = (int)uiScreenZoneType,
				BackgroundTextureName = null,
				JustificationType = (int)UiZoneJustificationTypes.Bottom,
				ElementRows =
				[
					new UiRowModel
					{
						UiRowName = "Spritesheet Buttons Row",
						TopPadding = 15,
						BottomPadding = 32+15,
						BackgroundTextureName = "gray_transparent",
						RowHoverCursorName = CommonCursorNames.BasicCursorName,
						HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Center,
						VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Bottom,
						SubElements = flattenedButtons
					}
				]
			};

			return uiService.GetUiZone(uiZoneModel);
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
