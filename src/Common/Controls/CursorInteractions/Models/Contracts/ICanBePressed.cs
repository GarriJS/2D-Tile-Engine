namespace Common.Controls.CursorInteractions.Models.Contracts
{
	/// <summary>
	/// Represents something that can be pressed.
	/// </summary>
	public interface ICanBePressed : IHaveACursorConfiguration
	{
		/// <summary>
		/// Raises the press event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaisePressEvent(CursorInteraction cursorInteraction);
    }
}
