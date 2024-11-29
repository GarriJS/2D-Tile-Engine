using System;

namespace Engine.UI.Models.Contracts
{
	/// <summary>
	/// Represents a user interface element.
	/// </summary>
	public interface IAmAUiElement : IDisposable
    {
        /// <summary>
        /// Gets or sets the user interface element name.
        /// </summary>
        public string UiElementName { get; set; }
    }
}
