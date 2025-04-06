using Common.Controls.Models;
using Common.Controls.Services.Contracts;
using Common.Tiling.Services.Contracts;
using Engine.Controls.Services.Contracts;
using Engine.Core.Constants;
using Engine.Core.Textures.Contracts;
using Engine.Drawables.Models;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.Services
{
	/// <summary>
	/// Represents a cursors service.
	/// </summary>
	/// <remarks>
	/// Initializes the cursor service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class CursorService(GameServiceContainer gameServices) : ICursorService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets a value indicating whether the cursor is active.
		/// </summary>
		public bool CursorIsActive { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the tile grid cursor is active.
		/// </summary>
		public bool TileGridCursorIsActive { get; private set; }

		/// <summary>
		/// Gets the primary cursor.
		/// </summary>
		public Cursor PrimaryCursor { get; private set; }

		/// <summary>
		/// Gets the tile grid cursor. 
		/// </summary>
		public TrailingCursor TileGridCursor { get; private set; }

		/// <summary>
		/// Loads the content.
		/// </summary>
		public void LoadContent()
		{
			var textureService = this._gameServices.GetService<ITextureService>();
			var runTimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			var runTimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();

			if (false == textureService.TryGetTexture("mouse", out var cursorTexture))
			{
				cursorTexture = textureService.DebugTexture;
			}

			if (false == textureService.TryGetTexture("tile_grid", out var tileGridTexture))
			{
				tileGridTexture = textureService.DebugTexture;
			}

			var position = new Position
			{
				Coordinates = new Vector2(0, 0)
			};

			this.PrimaryCursor = new Cursor
			{
				TextureName = cursorTexture.Name,
				TextureBox = new Rectangle(0, 0, 18, 28),
				Texture = cursorTexture,
				Position = position,
				DrawLayer = 1,
				TrailingCursors = []
			};

			var trailingCursorImage = new Image
			{
				TextureName = tileGridTexture.Name,
				TextureBox = new Rectangle(0, 0, 160, 160),
				Texture = tileGridTexture,
			};

			this.TileGridCursor = new TrailingCursor
			{
				Image = trailingCursorImage,
			};

			this.PrimaryCursor.TrailingCursors.Add(this.TileGridCursor);
			runTimeDrawService.AddOverlaidDrawable(this.PrimaryCursor.DrawLayer, PrimaryCursor);
			runTimeUpdateService.AddUpdateable(this.PrimaryCursor.DrawLayer, PrimaryCursor);
			
			this.CursorIsActive = true;
			this.TileGridCursorIsActive = true;
		}

		/// <summary>
		/// Updates the cursor position.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		public void UpdateCursorPosition(Cursor cursor)
		{
			var controlService = this._gameServices.GetService<IControlService>();

			cursor.Position.Coordinates = controlService.ControlState.MouseState.Position.ToVector2();

			if ((true == this.TileGridCursorIsActive) &&
				(true == cursor.TrailingCursors.Contains(this.TileGridCursor)))
			{ 
				var tileService = this._gameServices.GetService<ITileService>();

				var localTileLocation = tileService.GetLocalTileCoordinates(cursor.Position.Coordinates);
				this.TileGridCursor.Offset = new Vector2(localTileLocation.X - cursor.Position.X - ((this.TileGridCursor.Image.Texture.Width / 2) - (TileConstants.TILE_SIZE / 2)),
														 localTileLocation.Y - cursor.Position.Y - ((this.TileGridCursor.Image.Texture.Height / 2) - (TileConstants.TILE_SIZE / 2)));
			}
		}
	}
}
