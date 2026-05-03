using BaseContent.BaseContentConstants.Fonts;
using BaseContent.BaseContentConstants.Images;
using Common.Controls.CursorInteractions.Models;
using Common.Controls.Cursors.Constants;
using Common.DiskModels.UserInterface;
using Common.DiskModels.UserInterface.Elements;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Models.Elements;
using Common.UserInterface.Services.Contracts;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Enum;
using LevelEditor.Core.Constants;
using LevelEditor.LevelEditorContent.Images.Manifests;
using LevelEditor.Scenes.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Linq;

namespace LevelEditor.Scenes.Services
{
    /// <summary>
    /// Represents cursor interaction scene service.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the cursor interaction scene service.
    /// </remarks>
    /// <param name="gameServices"></param>
    public class SceneCursorInteractionService(GameServiceContainer gameServices) : ISceneCursorInteractionService
    {
        readonly private GameServiceContainer _gameServices = gameServices;

        /// <summary>
        /// Process the create scene button click event.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void PrcoessCreateSceneButtonClickEvent(CursorInteraction cursorInteraction)
        {
            if (cursorInteraction.Subject is not IAmAUiElement element)
            {
                // LOGGING
                return;
            }

            var sceneEditService = this._gameServices.GetService<ISceneEditService>();
            _ = sceneEditService.CreateNewScene(setCurrent: true, element.Name);
        }

        /// <summary>
        /// Process the load scene button click event.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void ProcessLoadSceneButtonClickEvent(CursorInteraction cursorInteraction)
        {
            if (cursorInteraction.Subject is not IAmAUiElement element)
            {
                // LOGGING
                return;
            }

            var sceneEditService = this._gameServices.GetService<ISceneEditService>();
            var tileMapModel = sceneEditService.LoadTileMapModel(element.Name);
            if (tileMapModel is null)
                return;

            var tileService = this._gameServices.GetService<ITileService>();
            var tileMap = tileService.GetTileMapFromModel(tileMapModel);
            sceneEditService.CreateNewScene(true, tileMap, element.Name);
        }

        /// <summary>
        /// Process the toggle tile grid click event.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void ProcessToggleTileGridClickEvent(CursorInteraction cursorInteraction)
        {
            var sceneEditService = this._gameServices.GetService<ISceneEditService>();
            sceneEditService.AddTileComponent.ToggleBackgroundGraphic();
        }

        /// <summary>
        /// Process the save scene button click event.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void ProcessSaveSceneButtonClickEvent(CursorInteraction cursorInteraction)
        {
            if ((false == cursorInteraction.SubjectUiParent.TryGetUiElement("Save Scene Modal Editor Element", out var elements)) ||
                (0 == elements.Length))
                return;

            var writableTextElement = elements[0] as UiWritableText;
            var sceneEditService = this._gameServices.GetService<ISceneEditService>();
            var currentScene = sceneEditService.CurrentScene;
            currentScene.SceneName = writableTextElement.WritableText.TextLines[0].Text;
            currentScene.TileMap.TileMapName = writableTextElement.WritableText.TextLines[0].Text;
            sceneEditService.SaveScene(currentScene);
        }

        /// <summary>
        /// Saves the scene.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void ProcessOpenSaveSceneModalClickEvent(CursorInteraction cursorInteraction)
        {
            if (cursorInteraction.Subject is not IAmAUiElement element)
            {
                // LOGGING
                return;
            }

            var sceneEditService = this._gameServices.GetService<ISceneEditService>();

            if (sceneEditService.CurrentScene is null)
                return;

            var modalName = "Save Scene Modal";
            var uiModalService = this._gameServices.GetService<IUiModalService>();

            if (uiModalService.ActiveUiModals.Any(e => e.Name == modalName))
                return;

            var saveSceneModalModel = new UiModalModel
            {
                Name = "Save Scene Modal",
                ResizeTexture = true,
                HorizontalLocationType = UiModalHorizontalLocationType.Center,
                VerticalLocationType = UiModalVerticalLocationType.Center,
                VerticalJustificationType = UiVerticalJustificationType.SpaceBetween,
                HorizontalModalSizeType = UiModalSizeType.FitContent,
                VerticalModalSizeType = UiModalSizeType.FitContent,
                HoverCursorName = CommonCursorNames.BasicCursorName,
                BackgroundTexture = new SimpleImageModel
                {
                    TextureName = "pallet",
                    TextureRegion = new TextureRegionModel
                    {
                        TextureRegionType = TextureRegionType.Tile,
                        TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_C7CFDD)
                    }
                },
                Blocks =
                [
                    new UiBlockModel
                    {
                        Name = "Save Scene Modal Block 1",
                        HorizontalJustificationType = UiHorizontalJustificationType.Center,
                        VerticalJustificationType = UiVerticalJustificationType.Top,
                        Rows =
                        [
                            new UiRowModel
                            {
                                Name = "Save Scene Modal Label Row 1",
                                ResizeTexture = true,
                                HorizontalJustificationType = UiHorizontalJustificationType.Center,
                                VerticalJustificationType = UiVerticalJustificationType.Center,
                                Elements =
                                [
                                    new UiTextModel
                                    {
                                        Name = "Save Scene Modal Element",
                                        HoverCursorName = CommonCursorNames.BasicCursorName,
                                        ResizeTexture = true,
                                        Margin = new UiMarginModel
                                        {
                                            LeftMargin = 10,
                                            RightMargin = 10,
                                        },
                                        HorizontalSizeType = UiElementSizeType.FitContent,
                                        VerticalSizeType = UiElementSizeType.FitContent,
                                        Text = new SimpleTextModel
                                        {
                                            TextColor = PalletColors.Hex_BF6F4A,
                                            FontName = FontNames.MonoBold,
                                            TextLines =
                                            [
                                                new TextLineModel
                                                {
                                                    IsManualBreak = true,
                                                    Text = "Name Scene"
                                                }
                                            ]
                                        },
                                        Graphic = new SimpleImageModel
                                        {
                                            TextureName = "pallet",
                                            TextureRegion = new TextureRegionModel
                                            {
                                                TextureRegionType = TextureRegionType.Fill,
                                                TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_1C121C)
                                            }
                                        }
                                    }
                                ]
                            }
                        ]
                    },
                    new UiBlockModel
                    {
                        Name = "Save Scene Modal Block 2",
                        HorizontalJustificationType = UiHorizontalJustificationType.Center,
                        VerticalJustificationType = UiVerticalJustificationType.Center,
                        Rows =
                        [
                            new UiRowModel
                            {
                                Name = "Save Scene Modal Editor Row 1",
                                ResizeTexture = true,
                                HorizontalJustificationType = UiHorizontalJustificationType.Center,
                                VerticalJustificationType = UiVerticalJustificationType.Center,
                                Elements =
                                [
                                    new UiWritableTextModel
                                    {
                                        Name = "Save Scene Modal Editor Element",
                                        HoverCursorName = CommonCursorNames.BasicCursorName,
                                        ResizeTexture = true,
                                        HorizontalSizeType = UiElementSizeType.FitContent,
                                        VerticalSizeType = UiElementSizeType.FitContent,
                                        HorizontalTextJustificationType = UiHorizontalTextJustification.Left,
                                        ClickableAreaScaler = new Vector2
                                        {
                                            X = 1,
                                            Y = 1
                                        },
                                        Margin = new UiMarginModel
                                        {
                                            LeftMargin = 10,
                                            RightMargin = 10,
                                        },
                                        Text = new WritableTextModel
                                        {
                                            MaxLineCharacterCount = 15,
                                            MaxLinesCount = 1,
                                            TextColor = PalletColors.Hex_BF6F4A,
                                            FontName = FontNames.MonoBold,
                                            TextLines =
                                            [
                                                new TextLineModel
                                                {
                                                    IsManualBreak = true,
                                                    Text = ""
                                                }
                                            ]
                                        },
                                        Graphic = new SimpleImageModel
                                        {
                                            TextureName = "pallet",
                                            TextureRegion = new TextureRegionModel
                                            {
                                                TextureRegionType = TextureRegionType.Fill,
                                                TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_1C121C)
                                            }
                                        },
                                        ActiveGraphic = new SimpleImageModel
                                        {
                                            TextureName = "pallet",
                                            TextureRegion = new TextureRegionModel
                                            {
                                                TextureRegionType = TextureRegionType.Fill,
                                                TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_1E6F50)
                                            }
                                        }
                                    }
                                ]
                            }
                        ]
                    },
                    new UiBlockModel
                    {
                        Name = "Save Scene Modal Block 3",
                        HorizontalJustificationType = UiHorizontalJustificationType.SpaceBetween,
                        VerticalJustificationType = UiVerticalJustificationType.Center,
                        Rows =
                        [
                            new UiRowModel
                            {
                                Name = "Save Scene Modal Buttons Row 1",
                                ResizeTexture = true,
                                HorizontalJustificationType = UiHorizontalJustificationType.SpaceBetween,
                                VerticalJustificationType = UiVerticalJustificationType.Center,
                                Elements =
                                [
                                    new UiButtonModel
                                    {
                                        Name = "Save Scene Back Button",
                                        HorizontalSizeType = UiElementSizeType.FitContent,
                                        VerticalSizeType = UiElementSizeType.FitContent,
                                        Text = new SimpleTextModel
                                        {
                                            TextColor = PalletColors.Hex_BF6F4A,
                                            FontName = FontNames.MonoBold,
                                            TextLines =
                                            [
                                                new TextLineModel
                                                {
                                                    IsManualBreak = true,
                                                    Text = "Back"
                                                }
                                            ]
                                        },
                                        ClickableAreaAnimation = new TriggeredAnimationModel
                                        {
                                            CurrentFrameIndex = 0,
                                            FrameDuration = 500,
                                            Frames =
                                            [
                                                DarkBlueButtonsManifest.GetUnpressedCompositeEmptyButton(50, 50),
                                                DarkBlueButtonsManifest.GetPressedCompositeEmptyButton(50, 50)
                                            ],
                                            RestingFrameIndex = 0,
                                        },
                                        ClickableAreaScaler = new Vector2
                                        {
                                            X = 1,
                                            Y = 1
                                        },
                                        //ClickEventName = UiEventName.SaveSceneClick
                                    },
                                    new UiButtonModel
                                    {
                                        Name = "Save Scene Confirm Button",
                                        HorizontalSizeType = UiElementSizeType.FitContent,
                                        VerticalSizeType = UiElementSizeType.FitContent,
                                        Text = new SimpleTextModel
                                        {
                                            TextColor = PalletColors.Hex_BF6F4A,
                                            FontName = FontNames.MonoBold,
                                            TextLines =
                                            [
                                                new TextLineModel
                                                {
                                                    IsManualBreak = true,
                                                    Text = "Confirm"
                                                }
                                            ]
                                        },
                                        ClickableAreaAnimation = new TriggeredAnimationModel
                                        {
                                            CurrentFrameIndex = 0,
                                            FrameDuration = 500,
                                            Frames =
                                            [
                                                DarkBlueButtonsManifest.GetUnpressedCompositeEmptyButton(50, 50),
                                                DarkBlueButtonsManifest.GetPressedCompositeEmptyButton(50, 50)
                                            ],
                                            RestingFrameIndex = 0,
                                        },
                                        ClickableAreaScaler = new Vector2
                                        {
                                            X = 1,
                                            Y = 1
                                        },
                                        ClickEventName = UiEventName.SaveSceneClick
                                    }
                                ]
                            }
                        ]
                    }
                ]
            };
            _ = uiModalService.GetUiModalFromModel(saveSceneModalModel, makeActive: true);
        }
    }
}
