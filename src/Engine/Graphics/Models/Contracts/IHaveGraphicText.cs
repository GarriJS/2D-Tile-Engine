namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represents something that has graphic text.
	/// </summary>
	public interface IHaveGraphicText
	{
		/// <summary>
		/// Get the graphic text.
		/// </summary>
		public IAmGraphicText GraphicText { get; }
	}
}
