using Engine.Controls.Services.Contracts;
using Engine.Controls.Typing;
using Engine.Core.Contracts;
using Engine.Core.Initialization;
using Engine.Debugging.Services.Contracts;
using Engine.DiskModels;
using Engine.DiskModels.Engine.Drawing;
using Engine.DiskModels.Engine.UI;
using Engine.DiskModels.Engine.UI.Elements;
using Engine.UI.Models.Elements;
using Engine.UI.Models.Enums;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine
{
	/// <summary>
	/// Represents a engine.
	/// </summary>
	public partial class Engine : Game
	{
		public GraphicsDeviceManager _graphics;

		/// <summary>
		/// Gets the external services.
		/// </summary>
		private List<Func<Game, (Type type, object provider)[]>> ExternalServiceProviders { get; } = [];

		/// <summary>
		/// Gets the external model processor map providers.
		/// </summary>
		private List<Func<GameServiceContainer, (Type typeIn, Delegate)[]>> ExternalModelProcessorMapProviders { get; } = [];

		/// <summary>
		/// Gets the initial models.
		/// </summary>
		private List<object> InitialModels { get; } = [];

		/// <summary>
		/// Gets the loadables.
		/// </summary>
		internal List<ILoadContent> Loadables { get; } = [];

		/// <summary>
		/// Gets the initializations. 
		/// </summary>
		internal List<INeedInitialization> Initializations { get; } = [];

		/// <summary>
		/// Initializes a new instance of the game1.
		/// </summary>
		public Engine()
		{
			this._graphics = new GraphicsDeviceManager(this);
			this.Content.RootDirectory = "Content";
			this.Window.AllowUserResizing = true;
			this.IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			// Start services
			_ = ServiceInitializer.StartEngineServices(this);

			foreach (var externalServiceProvider in this.ExternalServiceProviders)
			{
				_ = ServiceInitializer.StartServices(this, externalServiceProvider);
			}

			this.ExternalServiceProviders.Clear();

			// Do service initializations
			foreach (var initialization in this.Initializations)
			{
				initialization.Initialize();
			}

			// Load model processors
			ModelMapper.LoadEngineModelProcessingMappings(this.Services);

			foreach (var externalModelProcessorMapProvider in this.ExternalModelProcessorMapProviders)
			{
				ModelMapper.LoadModelProcessingMappings(this.Services, externalModelProcessorMapProvider);
			}

			this.ExternalModelProcessorMapProviders.Clear();

			// Other

			this._graphics.PreferredBackBufferWidth = 1920;
			this._graphics.PreferredBackBufferHeight = 1080;
			this._graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			// Do any content loading
			foreach (var loadable in this.Loadables)
			{
				loadable.LoadContent();
			}

			// Load the initial models
			ModelProcessor.ProcessInitialModels(this.InitialModels);
			this.InitialModels.Clear();

			// Other

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
							TextureName = "debug"
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
										ClickableAreaScaler = new Vector2(.9f, .9f),
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
									},
									new UiButtonModel
									{
										UiElementName = "foo1button2",
										LeftPadding = 5,
										RightPadding = 5,
										BackgroundTextureName = "black",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.Large
									}
								]
							}
						]
					}
				]
			};


			var bar = new UiGroupModel
			{
				UiGroupName = "bar",
				VisibilityGroupId = 2,
				UiZoneElements =
				[
					new UiZoneModel
					{
						UiZoneName = "foo1",
						UiZoneType = (int)UiScreenZoneTypes.Row3Col2,
						Background = new ImageModel
						{
							TextureName = "debug"
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
										FixedSized = new Vector2(100, 100)
									},
									new UiButtonModel
									{
										UiElementName = "foo1button2",
										LeftPadding = 0,
										RightPadding = 0,
										BackgroundTextureName = "gray",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.ExtraLarge,
										ClickableAreaScaler = new Vector2(.9f, .9f),
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
									},
									new UiButtonModel
									{
										UiElementName = "foo1button3",
										LeftPadding = 5,
										RightPadding = 5,
										BackgroundTextureName = "white",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.Small,
										ClickableAreaScaler = new Vector2(1f, 1f),
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
										ClickableAreaScaler = new Vector2(1f, 1f),
									},
									new UiButtonModel
									{
										UiElementName = "foo1button2",
										LeftPadding = 5,
										RightPadding = 5,
										BackgroundTextureName = "black",
										ButtonText = "Push Me",
										SizeType = (int)UiElementSizeTypes.Large,
										ClickableAreaScaler = new Vector2(1f, 1f),
									}
								]
							}
						]
					}
				]
			};

			var uiService = this.Services.GetService<IUserInterfaceService>();
			var group = uiService.GetUiGroup(foo);
			var group2 = uiService.GetUiGroup(bar);
			uiService.UserInterfaceGroups.Add(group);
			uiService.UserInterfaceGroups.Add(group2);
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
				Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				button.RaisePressEvent(foo.Location);
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
