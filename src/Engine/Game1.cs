using DiscModels.Engine.Drawing;
using DiscModels.Engine.UI;
using DiscModels.Engine.UI.Elements;
using Engine.Controls.Services.Contracts;
using Engine.Controls.Typing;
using Engine.Core.Initialization;
using Engine.Core.Initialization.Models;
using Engine.Debugging.Services.Contracts;
using Engine.Drawing.Services.Contracts;
using Engine.UI.Models.Elements;
using Engine.UI.Models.Enums;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public class Game1: Game
	{
		public GraphicsDeviceManager _graphics;

		public Game1()
		{
			this._graphics = new GraphicsDeviceManager(this);
			this.Content.RootDirectory = "Content";
			this.Window.AllowUserResizing = true;
			this.IsMouseVisible = true;
		}

		public void SetLoadingInstructions(LoadingInstructions loadingInstructions)
		{
			LoadingInstructionsContainer.LoadingInstructions = loadingInstructions;
		}

		protected override void Initialize()
		{
			_ = ServiceInitializer.StartServices(this);

			this._graphics.PreferredBackBufferWidth = 1920;
			this._graphics.PreferredBackBufferHeight = 1080;
			this._graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			foreach (var initialization in ServiceInitializer.Initializations)
			{
				initialization.Initialize();
			}

			foreach (var loadable in ServiceInitializer.Loadables)
			{ 
				loadable.LoadContent();
			}

			var debugService = this.Services.GetService<IDebugService>();
			debugService.ToggleScreenAreaIndicators();

			var foo = new UiGroupModel
			{
				UiGroupName = "foo",
				VisibilityGroupId = 1,
				UiZoneElements =
				[
					new UiZoneModel
					{
						UiZoneName = "foo1",
						UiZoneType = (int)UiScreenZoneTypes.Row1Col1,
						Background = new ImageModel
						{
							TextureName = "tile_grid"
						},
						JustificationType = (int)UiZoneJustificationTypes.Center,
						ElementRows =
						[
							new UiRowModel
							{
								UiRowName = "foo1row1",
								TopPadding = 5,
								BottomPadding = 5,
								HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Left,
								VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Top,
								SubElements =
								[
									new UiButtonModel
									{
										UiElementName = "foo1button1",
										LeftPadding = 5,
										RightPadding = 5,
										BackgroundTextureName = "black",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.ExtraSmall,
										ClickableAreaScaler = new Vector2(.9f, .9f),
										Signal = null,
										ClickableAreaAnimation = new TriggeredAnimationModel
										{
											CurrentFrameIndex = 0,
											FrameDuration = 1000,
											RestingFrameIndex = 0,
											Frames =
											[
												new ImageModel
												{ 
													TextureName = "white",
												},
												new ImageModel
												{
													TextureName = "black",
												}
											]
										}
									},
									new UiButtonModel
									{
										UiElementName = "foo1button2",
										LeftPadding = 0,
										RightPadding = 0,
										BackgroundTextureName = "gray",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.ExtraLarge,
										ClickableAreaScaler = new Vector2(.95f, .95f),
										Signal = null,
										ClickableAreaAnimation = new TriggeredAnimationModel
										{
											CurrentFrameIndex = 0,
											FrameDuration = 1000,
											RestingFrameIndex = 0,
											Frames =
											[
												new ImageModel
												{
													TextureName = "white",
												},
												new ImageModel
												{
													TextureName = "gray",
												}
											]
										}
									},
									new UiButtonModel
									{
										UiElementName = "foo1button3",
										LeftPadding = 5,
										RightPadding = 5,
										BackgroundTextureName = "white",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.Small,
										ClickableAreaScaler = new Vector2(.9f, .9f),
										Signal = null,
										ClickableAreaAnimation = new TriggeredAnimationModel
										{
											CurrentFrameIndex = 0,
											FrameDuration = 1000,
											RestingFrameIndex = 0,
											Frames =
											[
												new ImageModel
												{
													TextureName = "black",
												},
												new ImageModel
												{
													TextureName = "white",
												}
											]
										}
									}
								]
							},
							new UiRowModel
							{
								UiRowName = "foo1row2",
								TopPadding = 5,
								BottomPadding = 5,
								HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Left,
								VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Bottom,
								SubElements =
								[
									new UiButtonModel
									{
										UiElementName = "foo1button1",
										LeftPadding = 5,
										RightPadding = 0,
										BackgroundTextureName = "white",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.Medium,
										Signal = null
									},
									new UiButtonModel
									{
										UiElementName = "foo1button2",
										LeftPadding = 5,
										RightPadding = 5,
										BackgroundTextureName = "black",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.Large,
										Signal = null
									}
								]
							}
						]
					},
					new UiZoneModel
					{
						UiZoneName = "foo1",
						UiZoneType = (int)UiScreenZoneTypes.Row1Col2,
						Background = new ImageModel
						{
							TextureName = "tile_grid"
						},
						JustificationType = (int)UiZoneJustificationTypes.Center,
						ElementRows =
						[
							new UiRowModel
							{
								UiRowName = "foo1row1",
								TopPadding = 5,
								BottomPadding = 5,
								HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Right,
								VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Top,
								SubElements =
								[
									new UiButtonModel
									{
										UiElementName = "foo1button1",
										LeftPadding = 5,
										RightPadding = 5,
										BackgroundTextureName = "black",
										ButtonText = "Push Me",          
										FixedSized = new Vector2(100, 100),
										Signal = null
									},
									new UiButtonModel
									{
										UiElementName = "foo1button2",
										LeftPadding = 0,
										RightPadding = 0,
										BackgroundTextureName = "gray",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.ExtraLarge,
										Signal = null
									},
									new UiButtonModel
									{
										UiElementName = "foo1button3",
										LeftPadding = 5,
										RightPadding = 5,
										BackgroundTextureName = "white",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.Small,
										Signal = null
									}
								]
							},
							new UiRowModel
							{
								UiRowName = "foo1row2",
								TopPadding = 5,
								BottomPadding = 5,
								HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Right,
								VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Bottom,
								SubElements =
								[
									new UiButtonModel
									{
										UiElementName = "foo1button1",
										LeftPadding = 5,
										RightPadding = 0,
										BackgroundTextureName = "white",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.Medium,
										Signal = null
									},
									new UiButtonModel
									{
										UiElementName = "foo1button2",
										LeftPadding = 5,
										RightPadding = 5,
										BackgroundTextureName = "black",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.Large,
										Signal = null
									}
								]
							}
						]
					}
				]
			};

			var uiService = this.Services.GetService<IUserInterfaceService>();
			var group = uiService.GetUiGroup(foo);
			uiService.UserInterfaceGroups.Add(group);
			uiService.ToggleUserInterfaceGroupVisibility(group);
		}

		protected override void Update(GameTime gameTime)
		{
			var controlService = this.Services.GetService<IControlService>();
			var controlState = controlService.ControlState;

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			if (Keyboard.GetState().IsKeyDown(Keys.G))
			{

			}

			var mouse = Mouse.GetState().Position.ToVector2();
			var uiService = this.Services.GetService<IUserInterfaceService>();
			var foo = uiService.GetUiElementAtScreenLocation(mouse);

			if (foo?.Element is UiButton button &&
				null != button.ClickAnimation &&
				Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				var animationService = this.Services.GetService<IAnimationService>();

				animationService.TriggerAnimation(button.ClickAnimation);
			}

			base.Update(gameTime);

			KeyboardTyping.OldPressedKeys = Keyboard.GetState().GetPressedKeys();
		}

		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(Color.CornflowerBlue);
			//var drawService = this.Services.GetService<IDrawingService>();

			//drawService.BeginDraw();

			//drawService.EndDraw();

			base.Draw(gameTime);
		}
	}
}
