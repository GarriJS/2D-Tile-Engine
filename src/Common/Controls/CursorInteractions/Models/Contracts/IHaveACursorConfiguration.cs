using System;

namespace Common.Controls.CursorInteractions.Models.Contracts
{
	/// <summary>
	/// Represents something with a cursor configuration.
	/// </summary>
	public interface IHaveACursorConfiguration : IHaveABaseCursorConfiguration, IDisposable
	{
        /// <summary>
        /// Initializes the cursor configuration.
        /// </summary>
        public CursorConfiguration CursorConfiguration { init; }
    }
}
