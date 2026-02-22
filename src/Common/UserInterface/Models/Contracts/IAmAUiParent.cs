using Common.Controls.CursorInteraction.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using System.Collections.Generic;

namespace Common.UserInterface.Models.Contracts
{
	/// <summary>
	/// Represents a user interface parent.
	/// </summary>
	public interface IAmAUiParent : IHaveArea, IHaveAHoverCursor, IHaveACursorConfiguration
	{
		/// <summary>
		/// The user interface blocks.
		/// </summary>
		public List<UiBlock> Blocks { get; }

		/// <summary>
		/// Enumerates the Block blockLayout.
		/// </summary>
		/// <param name="includeScrollOffset">A value indicating whether to include the scroll offset.</param>
		/// <returns>The enumerated Block blockLayout.</returns>
		public IEnumerable<Vector2Extender<UiBlock>> EnumerateLayout(bool includeScrollOffset);
	}
}
