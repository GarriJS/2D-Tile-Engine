using Engine.Graphics.Models;

namespace Common.UserInterface.Models.Contracts
{
	/// <summary>
	/// Represents a user interface element with text
	/// </summary>
	public interface IAmAUiElementWithText : IAmAUiElement
	{
		/// <summary>
		/// Gets or sets the Graphic text.
		/// </summary>
		public GraphicalText GraphicText { get; set; }
	}
}
