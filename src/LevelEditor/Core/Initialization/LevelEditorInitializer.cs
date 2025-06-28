using Common.DiskModels.UI;
using Common.DiskModels.UI.Elements;
using Common.UI.Models.Contracts;
using Common.UI.Models.Enums;
using Engine.DiskModels.Drawing;
using LevelEditor.Spritesheets.Models.Constants;
using LevelEditor.Spritesheets.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LevelEditor.Core.Initialization
{
	/// <summary>
	/// Represents a level editor initializer.
	/// </summary>
	public static class LevelEditorInitializer
	{
		/// <summary>
		/// Gets the initial hover event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the hover event processors.</returns>
		public static Dictionary<string, Action<IAmAUiElement, Vector2>> GetInitialHoverEventProcessors(GameServiceContainer gameServices)
		{
			var spritesheetButtonService = gameServices.GetService<ISpritesheetButtonService>();

			return new Dictionary<string, Action<IAmAUiElement, Vector2>>
			{
				[UiEventNameConstants.SpritesheetButtonHover] = spritesheetButtonService.SpritesheetButtonHoverEventProcessor
			};
		}

		/// <summary>
		/// Gets the initial press event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the press event processors.</returns>
		public static Dictionary<string, Action<IAmAUiElement, Vector2>> GetInitialPressEventProcessors(GameServiceContainer gameServices)
		{
			return new Dictionary<string, Action<IAmAUiElement, Vector2>>
			{

			};
		}

		/// <summary>
		/// Gets the initial click event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the click event processors.</returns>
		public static Dictionary<string, Action<IAmAUiElement, Vector2>> GetInitialClickEventProcessors(GameServiceContainer gameServices)
		{
			var spritesheetButtonService = gameServices.GetService<ISpritesheetButtonService>();

			return new Dictionary<string, Action<IAmAUiElement, Vector2>>
			{
				[UiEventNameConstants.SpritesheetButtonClick] = spritesheetButtonService.SpritesheetButtonClickEventProcessor
			};
		}

		/// <summary>
		/// Gets the initial user interface models.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The user interface models.</returns>
		public static IList<object> GetInitialUiModels(GameServiceContainer gameServices)
		{
			var spritesheetButtonService = gameServices.GetService<ISpritesheetButtonService>();
			var spritesheetButtons = spritesheetButtonService.GetUiButtonsForSpritesheet("dark_grass_simplified", new Point(32, 32));
			var flattenedButtons = spritesheetButtons?.SelectMany(row => row).ToArray();

			return
			[
				new UiGroupModel
				{
					UiGroupName = "foo",
					VisibilityGroupId = 1,
					IsVisible = true,
					UiZoneElements =
					[
						new UiZoneModel
						{
							UiZoneName = "foo1",
							UiZoneType = (int)UiScreenZoneTypes.Row2Col1,
							BackgroundTextureName = string.Empty,
							JustificationType = (int)UiZoneJustificationTypes.Bottom,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "foo1row1",
									TopPadding = 0,
									BottomPadding = 0,
									BackgroundTextureName = null,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Left,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Center,
									SubElements =
									[
										new UiTextModel
										{
											UiElementName = "foo1button1",
											LeftPadding = 0,
											RightPadding = 0,
											BackgroundTextureName = "white",
											Text = "Welcome to the level editor!",
											SizeType = (int)UiElementSizeTypes.ExtraLarge
										}
									]
								}
							]
						},
						new UiZoneModel
						{
							UiZoneName = "foo1",
							UiZoneType = (int)UiScreenZoneTypes.Row3Col1,
							BackgroundTextureName = "gray_transparent",
							JustificationType = (int)UiZoneJustificationTypes.Center,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "foo1row1",
									TopPadding = 4,
									BottomPadding = 4,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Right,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Top,
									SubElements =
									[
										new UiButtonModel
										{
											UiElementName = "foo1button2",
											LeftPadding = 0,
											RightPadding = 0,
											BackgroundTextureName = "gray",
											Text = "Push Me",
											SizeType = (int)UiElementSizeTypes.Fill,
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
									TopPadding = 4,
									BottomPadding = 4,
									HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Center,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Bottom,
									SubElements =
									[
										new UiButtonModel
										{
											UiElementName = "foo1button1",
											LeftPadding = 4,
											RightPadding = 2,
											BackgroundTextureName = "white",
											Text = "Push Me 1",
											SizeType = (int)UiElementSizeTypes.Fill,
											ClickableAreaScaler = new Vector2(1f, 1f),
										},
										new UiButtonModel
										{
											UiElementName = "foo1button2",
											LeftPadding = 2,
											RightPadding = 4,
											BackgroundTextureName = "black",
											Text = "Push Me 2",
											SizeType = (int)UiElementSizeTypes.ExtraLarge,
											ClickableAreaScaler = new Vector2(1f, 1f),
										}
									]
								}
							]
						},
						new UiZoneModel
						{
							UiZoneName = "foo1",
							UiZoneType = (int)UiScreenZoneTypes.Row3Col2,
							BackgroundTextureName = "gray_transparent",
							JustificationType = (int)UiZoneJustificationTypes.Top,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "foo1row3",
									TopPadding = 40,
									BottomPadding = 4,
									HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Center,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.None,
									SubElements = flattenedButtons
								}
							]
						}
					]
				}
			];
		}
	}
}
