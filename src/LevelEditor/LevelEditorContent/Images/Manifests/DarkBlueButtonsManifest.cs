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
		/// Creates the image model.
		/// </summary>
		/// <param name="x">The x value.</param>
		/// <param name="y">The y value.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The image model.</returns>
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
		/// <returns>The image by parts.</returns>
		static public ImageByPartsModel GetUnpressedEmptyButtonByParts(int width, int height)
		{
			const int edgeLength = 7;

			ImageModel[][] image =
			[
				[ Create(0, 0, edgeLength, edgeLength), Create(edgeLength, 0, width, edgeLength), Create(width + edgeLength, 0, edgeLength, edgeLength) ],
				[ Create(0, edgeLength, edgeLength, height), Create(edgeLength, edgeLength, width, height), Create(width + edgeLength, edgeLength, edgeLength, height) ],
				[ Create(0, height + edgeLength, edgeLength, edgeLength), Create(edgeLength, height + edgeLength, width, edgeLength), Create(57, height + edgeLength, edgeLength, edgeLength) ]
			];

			return new ImageByPartsModel
			{ 
				Images = image,
			};
		}

		/// <summary>
		/// Gets the unpressed empty button image model.
		/// </summary>
		static public ImageModel UnpressedEmptyButton { get => Create(0, 0); }

		/// <summary>
		/// Gets the pressed empty button image model.
		/// </summary>
		static public ImageModel PressedEmptyButton { get => Create(64, 0); }

		/// <summary>
		/// Gets the unpressed enter button image model.
		/// </summary>
		static public ImageModel UnpressedEnterButton { get => Create(0, 64); }

		/// <summary>
		/// Gets the pressed enter button image model.
		/// </summary>
		static public ImageModel PressedEnterButton { get => Create(64, 64); }

		/// <summary>
		/// Gets the unpressed tab button image model.
		/// </summary>
		static public ImageModel UnpressedTabButton { get => Create(0, 128); }

		/// <summary>
		/// Gets the pressed tab button image model.
		/// </summary>
		static public ImageModel PressedTabButton { get => Create(64, 128); }

		/// <summary>
		/// Gets the unpressed plus button image model.
		/// </summary>
		static public ImageModel UnpressedPlusButton { get => Create(0, 192); }

		/// <summary>
		/// Gets the pressed plus button image model.
		/// </summary>
		static public ImageModel PressedPlusButton { get => Create(64, 192); }

		/// <summary>
		/// Gets the unpressed minus button image model.
		/// </summary>
		static public ImageModel UnpressedMinusButton { get => Create(0, 256); }

		/// <summary>
		/// Gets the pressed minus button image model.
		/// </summary>
		static public ImageModel PressedMinusButton { get => Create(64, 256); }
	}
}
