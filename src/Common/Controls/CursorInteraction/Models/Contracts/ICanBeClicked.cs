namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents something that can be clicked.
	/// </summary>
	/// <typeparam name="T">The type being clicked.</typeparam>
	public interface ICanBeClicked<T> : IHaveATypedCursorConfiguration<T>
	{
		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseClickEvent(CursorInteraction<T> cursorInteraction);
    }
}
