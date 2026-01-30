namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represents something that is scrollable.
	/// </summary>
	public interface IAmScrollable
	{
		/// <summary>
		/// Gets or sets a value indicating whether the parent is scrollable.
		/// </summary>
		public bool IsScrollable { get; set; }

		/// <summary>
		/// Gets the scroll state.
		/// </summary>
		public ScrollState ScrollState { get; set; }
	}
}
