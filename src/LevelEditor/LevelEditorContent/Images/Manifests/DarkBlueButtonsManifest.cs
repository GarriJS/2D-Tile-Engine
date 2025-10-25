using Engine.DiskModels.Drawing;
using Microsoft.Xna.Framework;

namespace LevelEditor.LevelEditorContent.Images.Manifests
{
	/// <summary>
	/// Represents a manifest for the dark blue buttons spritesheet.
	/// </summary>
	static public class DarkBlueButtonsManifest
	{
		/// <summary>
		/// Gets the button width.
		/// </summary>
		public const int ButtonWidth = 64;

		/// <summary>
		/// Gets the button height.
		/// </summary>
		public const int ButtonHeight = 64;

		/// <summary>
		/// Creates the textureRegions model.
		/// </summary>
		/// <param name="x">The x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The textureRegions model.</returns>
		private static ImageModel Create(int x, int y, int width = ButtonWidth, int height = ButtonHeight) => new()
		{
			TextureName = SpritesheetName,
			TextureBox = new Rectangle
			{
				X = x,
				Y = y,
				Width = width,
				Height = height
			}
		};

		/// <summary>
		/// Gets the spritesheet name.
		/// </summary>
		static public string SpritesheetName { get; } = "dark_blue_buttons";

		/// <summary>
		/// Gets the unpressed empty button by parts.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The textureRegions by parts.</returns>
		static public ImageByPartsModel GetUnpressedEmptyButtonByParts(int width, int height)
		{
			const int edgeLength = 7;

			Rectangle[][] textureRegions =
			[
				[
					new Rectangle(0, 0, edgeLength, edgeLength),
					new Rectangle(edgeLength, 0, width, edgeLength),
					new Rectangle(width + edgeLength, 0, edgeLength, edgeLength)
				],
				[
					new Rectangle(0, edgeLength, edgeLength, height),
					new Rectangle(edgeLength, edgeLength, width, height),
					new Rectangle(width + edgeLength, edgeLength, edgeLength, height)
				],
				[
					new Rectangle(0, height + edgeLength, edgeLength, edgeLength),
					new Rectangle(edgeLength, height + edgeLength, width, edgeLength),
					new Rectangle(width + edgeLength, height + edgeLength, edgeLength, edgeLength)
				]
			];

			return new ImageByPartsModel
			{
				//TextureRegions = textureRegions,
			};
		}

		/// <summary>
		/// Gets the unpressed empty button textureRegions model.
		/// </summary>
		static public ImageModel UnpressedEmptyButton { get => Create(0, 0); }

		/// <summary>
		/// Gets the pressed empty button textureRegions model.
		/// </summary>
		static public ImageModel PressedEmptyButton { get => Create(64, 0); }

		/// <summary>
		/// Gets the unpressed enter button textureRegions model.
		/// </summary>
		static public ImageModel UnpressedEnterButton { get => Create(0, 64); }

		/// <summary>
		/// Gets the pressed enter button textureRegions model.
		/// </summary>
		static public ImageModel PressedEnterButton { get => Create(64, 64); }

		/// <summary>
		/// Gets the unpressed tab button textureRegions model.
		/// </summary>
		static public ImageModel UnpressedTabButton { get => Create(0, 128); }

		/// <summary>
		/// Gets the pressed tab button textureRegions model.
		/// </summary>
		static public ImageModel PressedTabButton { get => Create(64, 128); }

		/// <summary>
		/// Gets the unpressed plus button textureRegions model.
		/// </summary>
		static public ImageModel UnpressedPlusButton { get => Create(0, 192); }

		/// <summary>
		/// Gets the pressed plus button textureRegions model.
		/// </summary>
		static public ImageModel PressedPlusButton { get => Create(64, 192); }

		/// <summary>
		/// Gets the unpressed minus button textureRegions model.
		/// </summary>
		static public ImageModel UnpressedMinusButton { get => Create(0, 256); }

		/// <summary>
		/// Gets the pressed minus button textureRegions model.
		/// </summary>
		static public ImageModel PressedMinusButton { get => Create(64, 256); }
	}
}
