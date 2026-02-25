using Engine.Graphics.Models;

namespace Common.UserInterface.Models.Contracts
{
	/// <summary>
	/// Represents something with a click animation.
	/// </summary>
	public interface IHaveAClickAnimation
	{
		/// <summary>
		/// Gets or sets the clickable animation.
		/// </summary>
		public TriggeredAnimation ClickAnimation { get; set; }
	}
}
