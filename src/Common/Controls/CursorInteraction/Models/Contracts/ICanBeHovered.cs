namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents something that can be hovered.
	/// </summary>
	/// <typeparam name="T">The type being hovered.</typeparam>
	public interface ICanBeHovered<T> : IHaveATypedCursorConfiguration<T>
	{
		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseHoverEvent(CursorInteraction<T> cursorInteraction);
    }
}
