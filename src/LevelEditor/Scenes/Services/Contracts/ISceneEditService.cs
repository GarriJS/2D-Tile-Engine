using Common.DiskModels.UI;
using Common.Scenes.Models;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Engine.Core.Contracts;
using Engine.Physics.Models;
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
		/// Gets the add tile component.
		/// </summary>
		public AddTileComponent AddTileComponent { get; }

		/// <summary>
		/// The create scene button click event processor.
		/// </summary>
		/// <param name="elementWithLocation">The element with location.</param>
		public void CreateSceneButtonClickEventProcessor(LocationExtender<IAmAUiElement> elementWithLocation);

		/// <summary>
		/// The toggle tile grid click event processor.
		/// </summary>
		/// <param name="elementWithLocation">The element with location.</param>
		public void ToggleTileGridClickEventProcessor(LocationExtender<IAmAUiElement> elementWithLocation);

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
		/// Saves the scene.
		/// </summary>
		/// <param name="elementWithLocation">The element with location.</param>
		public void SaveScene(LocationExtender<IAmAUiElement> elementWithLocation);
	}
}
