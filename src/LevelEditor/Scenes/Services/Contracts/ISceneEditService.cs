using Common.Scenes.Models;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Engine.Core.Contracts;
using LevelEditor.Scenes.Models;
using Microsoft.Xna.Framework;

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
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void CreateSceneButtonClickEventProcessor(IAmAUiElement element, Vector2 elementLocation);

		/// <summary>
		/// The toggle tile grid click event processor.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void ToggleTileGridClickEventProcessor(IAmAUiElement element, Vector2 elementLocation);

		/// <summary>
		/// Gets the tile grid user interface zone.
		/// </summary>
		/// <returns>The user interface zone.</returns>
		public UiZone GetTileGridUserInterfaceZone();

		/// <summary>
		/// Creates a new scene.
		/// </summary>
		/// <param name="setCurrent">A value indicating whether to set the new scene as the current scene.</param>
		/// <param name="sceneName">The scene name.</param>
		/// <returns>The new scene.</returns>
		public Scene CreateNewScene(bool setCurrent, string sceneName = null);
	}
}
