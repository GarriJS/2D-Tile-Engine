using Common.Controls.CursorInteractions.Models;

namespace LevelEditor.Scenes.Services.Contracts
{
    /// <summary>
    /// Represents cursor interaction scene service.
    /// </summary>
    public interface ISceneCursorInteractionService
    {
        /// <summary>
        /// Process the create scene button click event.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void PrcoessCreateSceneButtonClickEvent(CursorInteraction cursorInteraction);

        /// <summary>
        /// Process the load scene button click event.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void ProcessLoadSceneButtonClickEvent(CursorInteraction cursorInteraction);

        /// <summary>
        /// Process the toggle tile grid click event.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void ProcessToggleTileGridClickEvent(CursorInteraction cursorInteraction);

        /// <summary>
        /// Process the save scene button click event.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void ProcessSaveSceneButtonClickEvent(CursorInteraction cursorInteraction);

        /// <summary>
        /// Saves the scene.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void ProcessOpenSaveSceneModalClickEvent(CursorInteraction cursorInteraction);
    }
}
