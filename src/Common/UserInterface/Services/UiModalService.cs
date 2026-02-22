using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.Core.Constants;
using Common.DiskModels.UserInterface;
using Common.UserInterface.Constants;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Engine.Core.Initialization.Services.Contracts;
using Engine.DiskModels.Physics;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a user interface modal service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface modal service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class UiModalService(GameServiceContainer gameServices) : IUiModalService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// The active user interface modals.
		/// </summary>
		readonly private List<UiModal> _activeUiModals = [];

		/// <summary>
		/// Gets the active user interface modals.
		/// </summary>
		public List<UiModal> ActiveUiModals { get => this._activeUiModals; }

		/// <summary>
		/// Gets the active user interface modal from the model.
		/// </summary>
		/// <param name="uiModalModel">The user interface modal model.</param>
		/// <returns>The user interface modal.</returns>
		public UiModal GeActivetUiModalFromModel(UiModalModel uiModalModel) => this.GetUiModalFromModel(uiModalModel, true);

		/// <summary>
		/// Gets the user interface modal from the model.
		/// </summary>
		/// <param name="uiModalModel">The user interface modal model.</param>
		/// <param name="makeActive">A value indicating whether to make the modal active.</param>
		/// <returns>The user interface modal.</returns>
		public UiModal GetUiModalFromModel(UiModalModel uiModalModel, bool makeActive = true)
		{
			var uiZoneService = this._gameServices.GetService<IUiScreenService>();
			var uiElementService = this._gameServices.GetService<IUiElementService>();
			var functionService = this._gameServices.GetService<IFunctionService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var cursorService = this._gameServices.GetService<ICursorService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var scrollStateService = this._gameServices.GetService<IScrollStateService>();
			var uiBlockService = this._gameServices.GetService<IUiBlockService>();
			var uiRowService = this._gameServices.GetService<IUiRowService>();
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var area = this.GetModalArea(uiModalModel);
			var blocks = new List<UiBlock>();

			foreach (var uiBlockModel in uiModalModel.Blocks ?? [])
			{
				var block = uiBlockService.GetUiBlockFromModel(uiBlockModel, area.ToSubArea);
				blocks.Add(block);
			}

			var contentHeight = blocks.Sum(e => e.TotalHeight);
			var rows = blocks.Where(e => e._rows.Count != 0)
							 .SelectMany(e => e._rows)
							 .ToArray();
			var dynamicRows = rows.Where(r => r._elements.Any(e => true == UiGroupService._dynamicSizedTypes.Contains(e.VerticalSizeType)))
								  .ToArray();
			var remainingHeight = area.Height - contentHeight;
			var dynamicHeight = remainingHeight / dynamicRows.Length;

			if (area.Height * ElementSizesScalars.ExtraSmall.Y > dynamicHeight)
			{
				// LOGGING
				dynamicHeight = area.Height * ElementSizesScalars.ExtraSmall.Y;
			}

			foreach (var dynamicRow in dynamicRows ?? [])
				uiRowService.UpdateRowDynamicHeight(dynamicRow, dynamicHeight);

			if (((uiModalModel.ScrollStateModel is null) ||
				 (true == uiModalModel.ScrollStateModel.DisableScrolling)) &&
				(contentHeight > area.Height))
			{
				var exessHeight = contentHeight - area.Height;
				var scrollableBlocks = blocks.Where(e => false == e.ScrollState?.DisableScrolling)
											 .ToArray();
				var splitExessHeight = exessHeight / scrollableBlocks.Length;

				foreach (var scrollableBlock in scrollableBlocks)
					scrollableBlock.ScrollState.MaxVisibleHeight -= splitExessHeight;
			}

			IAmAGraphic background = null;

			if (uiModalModel.BackgroundTexture is not null)
			{
				background = imageService.GetImageFromModel(uiModalModel.BackgroundTexture);

				if ((true == uiModalModel.ResizeTexture) ||
					(background is CompositeImage))
					background.SetDrawDimensions(area.ToSubArea);
			}

			Cursor hoverCursor = null;

			if ((false == string.IsNullOrEmpty(uiModalModel.HoverCursorName)) &&
				(false == cursorService.Cursors.TryGetValue(uiModalModel.HoverCursorName, out hoverCursor)))
			{
				// LOGGING
			}

			var scrollState = scrollStateService.GetScrollStateFromModel(uiModalModel.ScrollStateModel);
			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<UiZone>(area.ToSubArea, null);
			var result = new UiModal
			{
				ResetCalculateCachedOffsets = true,
				Name = uiModalModel.Name,
				DrawLayer = RunTimeConstants.BaseUiDrawLayer,
				VerticalJustificationType = uiModalModel.VerticalJustificationType,
				HorizontalModalSizeType = uiModalModel.HorizontalModalSizeType,
				VerticalModalSizeType = uiModalModel.VerticalModalSizeType,
				Graphic = background,
				Area = area,
				ScrollState = scrollState,
				HoverCursor = hoverCursor,
				CursorConfiguration = cursorConfiguration
			};
			result.Blocks.AddRange(blocks);

			if (true == makeActive)
				this.AddActiveModal(result);

			return result;
		}

		/// <summary>
		/// Add the active modal.
		/// </summary>
		/// <param name="uiModal">The user interface modal.</param>
		/// <returns>A value indicating whether the user interface modal was not active.</returns>
		public bool AddActiveModal(UiModal uiModal)
		{
			if (true == this._activeUiModals.Contains(uiModal))
				return false;

			var runtimeDrawingService = this._gameServices.GetService<IRuntimeDrawService>();
			runtimeDrawingService.AddDrawable(uiModal);
			this._activeUiModals.Add(uiModal);

			return true;
		}

		/// <summary>
		/// Removes the active modal.
		/// </summary>
		/// <param name="uiModal">The user interface modal.</param>
		/// <returns>A value indicating whether the user interface modal was active.</returns>
		public bool RemoveActiveModal(UiModal uiModal)
		{ 
			if (false == this._activeUiModals.Contains(uiModal))
				return false;

			var runtimeDrawingService = this._gameServices.GetService<IRuntimeDrawService>();
			runtimeDrawingService.RemoveDrawable(uiModal);
			this._activeUiModals.Remove(uiModal);

			return true;
		}

		/// <summary>
		/// Gets the modal area.
		/// </summary>
		/// <param name="uiModalModel">The user interface modal model.</param>
		/// <returns>The modal area.</returns>
		private SimpleArea GetModalArea(UiModalModel uiModalModel)
		{
			if (true == uiModalModel.FixedSized.HasValue)
			{
				var fixedSize = uiModalModel.FixedSized.Value;

				if (0 > fixedSize.X)
					fixedSize.X = 0;

				if (0 > fixedSize.Y)
					fixedSize.Y = 0;

				var fixedPosition = this.GetModalPostion(uiModalModel, fixedSize.X, fixedSize.Y);
				var fixedResult = new SimpleArea
				{
					Width = fixedSize.X,
					Height = fixedSize.Y,
					Position = fixedPosition
				};

				return fixedResult;
			}

			var uiScreenService = this._gameServices.GetService<IUiScreenService>();
			var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();

			if (uiScreenService?.ScreenZoneSize is null)
				return default;

			var uiElementHorizontalSize = uiModalModel.HorizontalModalSizeType switch
			{
				UiModalSizeType.ExtraSmall => uiScreenService.ScreenZoneSize.Width * ModalSizesScalars.ExtraSmall.X,
				UiModalSizeType.Small => uiScreenService.ScreenZoneSize.Width * ModalSizesScalars.Small.X,
				UiModalSizeType.Medium => uiScreenService.ScreenZoneSize.Width * ModalSizesScalars.Medium.X,
				UiModalSizeType.Large => uiScreenService.ScreenZoneSize.Width * ModalSizesScalars.Large.X,
				UiModalSizeType.ExtraLarge => uiScreenService.ScreenZoneSize.Width * ModalSizesScalars.ExtraLarge.X,
				UiModalSizeType.Fullscreen => graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferWidth,
				_ => 0
			};
			var uiElementVerticalSize = uiModalModel.VerticalModalSizeType switch
			{
				UiModalSizeType.ExtraSmall => uiScreenService.ScreenZoneSize.Height * ModalSizesScalars.ExtraSmall.Y,
				UiModalSizeType.Small => uiScreenService.ScreenZoneSize.Height * ModalSizesScalars.Small.Y,
				UiModalSizeType.Medium => uiScreenService.ScreenZoneSize.Height * ModalSizesScalars.Medium.Y,
				UiModalSizeType.Large => uiScreenService.ScreenZoneSize.Height * ModalSizesScalars.Large.Y,
				UiModalSizeType.ExtraLarge => uiScreenService.ScreenZoneSize.Height * ModalSizesScalars.ExtraLarge.Y,
				UiModalSizeType.Fullscreen => graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferHeight,
				_ => 0
			};
			var position = this.GetModalPostion(uiModalModel, uiElementHorizontalSize, uiElementVerticalSize);
			var result = new SimpleArea
			{
				Width = uiElementHorizontalSize,
				Height = uiElementVerticalSize,
				Position = position
			};

			return result;
		}

		/// <summary>
		/// Gets the modal position.
		/// </summary>
		/// <param name="uiModalModel">The user interface modal model.</param>
		/// <param name="width">The modal width.</param>
		/// <param name="height">The modal height.</param>
		/// <returns>The modal position.</returns>
		private Position GetModalPostion(UiModalModel uiModalModel, float width, float height)
		{
			var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			var positionService = this._gameServices.GetService<IPositionService>();
			var x = uiModalModel.HorizontalLocationType switch
			{
				UiModalHorizontalLocationType.Left => 0f,
				UiModalHorizontalLocationType.Center => (graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferWidth - width) / 2,
				UiModalHorizontalLocationType.Right => graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferWidth - width,
				_ => 0
			};
			var y = uiModalModel.VerticalLocationType switch
			{
				UiModalVerticalLocationType.Top => 0f,
				UiModalVerticalLocationType.Center => (graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferHeight - height) / 2,
				UiModalVerticalLocationType.Bottom => graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferHeight - height,
				_ => 0
			};
			var positionModel = new PositionModel
			{
				X = x,
				Y = y
			};
			var result = positionService.GetPositionFromModel(positionModel);

			return result;
		}
	}
}
