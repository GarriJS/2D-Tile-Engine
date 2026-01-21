using Common.Controls.CursorInteraction.Models;
using Common.DiskModels.Tiling;
using Common.DiskModels.UserInterface;
using Common.Scenes.Models;
using Common.Tiling.Models;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Engine.Core.Contracts;
using LevelEditor.Scenes.Models;

namespace LevelEditor.Scenes.Services.Contracts
{
	/// <summary>
	/// Represents a scene edit service.
	/// </summary>
	public interface ISceneEditService : ILoadContent
	{
		/// <summary>
		/// Gets the current scene.
		/// </summary>
		public Scene CurrentScene { get; }

		/// <summary>
		/// Gets the add tile Block.
		/// </summary>
		public AddTileComponent AddTileComponent { get; }

		/// <summary>
		/// The create scene button click event processor.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void CreateSceneButtonClickEventProcessor(CursorInteraction<IAmAUiElement> cursorInteraction);

		/// <summary>
		/// The load scene button click event processor.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void LoadSceneButtonClickEventProcessor(CursorInteraction<IAmAUiElement> cursorInteraction);

		/// <summary>
		/// The toggle tile grid click event processor.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void ToggleTileGridClickEventProcessor(CursorInteraction<IAmAUiElement> cursorInteraction);

		/// <summary>
		/// Gets the tile grid user interface zone.
		/// </summary>
		/// <returns>The user interface zone.</returns>
		public UiZone GetTileGridUserInterfaceZone();

		/// <summary>
		/// Gets the saved tile map user interface rows.
		/// </summary>
		/// <returns>The saved tile map user interface rows.</returns>
		public UiRowModel[] GetSavedTileMapUserInterfaceRows();

		/// <summary>
		/// Creates a new scene.
		/// </summary>
		/// <param name="setCurrent">A value indicating whether to set the new scene as the current scene.</param>
		/// <param name="sceneName">The scene name.</param>
		/// <returns>The new scene.</returns>
		public Scene CreateNewScene(bool setCurrent, string sceneName = null);

		/// <summary>
		/// Creates a new scene.
		/// </summary>
		/// <param name="setCurrent">A value indicating whether to set the new scene as the current scene.</param>
		/// <param name="tileMap">The tile map of the scene.</param>
		/// <param name="sceneName">The scene name.</param>
		/// <returns>The new scene.</returns>
		public Scene CreateNewScene(bool setCurrent, TileMap tileMap, string sceneName = null);

		/// <summary>
		/// Sets the current scene.
		/// </summary>
		/// <param name="scene"></param>
		public void SetCurrentScene(Scene scene);

		/// <summary>
		/// Saves the scene.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void SaveScene(CursorInteraction<IAmAUiElement> cursorInteraction);

		/// <summary>
		/// Loads the tile map model.
		/// </summary>
		/// <param name="tileMapName">The tile map name.</param>
		/// <returns>The tile map model.</returns>
		public TileMapModel LoadTileMapModel(string tileMapName);
	}
}
