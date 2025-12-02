namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents something that can be pressed.
	/// </summary>
	/// <typeparam name="T">The type being pressed.</typeparam>
	public interface ICanBePressed<T> : IHaveATypedCursorConfiguration<T>
	{
		/// <summary>
		/// Raises the press event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaisePressEvent(CursorInteraction<T> cursorInteraction);
    }
}
