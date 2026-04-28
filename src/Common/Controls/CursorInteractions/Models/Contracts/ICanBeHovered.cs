namespace Common.Controls.CursorInteractions.Models.Contracts
{
	/// <summary>
	/// Represents something that can be hovered.
	/// </summary>
	public interface ICanBeHovered : IHaveACursorConfiguration
	{
		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseHoverEvent(CursorInteraction cursorInteraction);
    }
}
