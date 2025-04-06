using Common.Controls.Models;
using Engine.Core.Contracts;

namespace Common.Controls.Services.Contracts
{
    /// <summary>
    /// Represents a cursors service.
    /// </summary>
    public interface ICursorService : ILoadContent
    {
		/// <summary>
		/// Updates the cursor position.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		public void UpdateCursorPosition(Cursor cursor);
	}
}
