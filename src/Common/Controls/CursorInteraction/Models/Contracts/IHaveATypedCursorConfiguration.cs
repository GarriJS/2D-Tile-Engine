namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents something with a typed cursor configuration.
	/// </summary>
	/// <typeparam name="T">The parent type.</typeparam>
	public interface IHaveATypedCursorConfiguration<T> : IHaveACursorConfiguration
	{
		/// <summary>
		/// Gets or sets the hover configuration.
		/// </summary>
		public CursorConfiguration<T> CursorConfiguration { get; set; }
	}
}
