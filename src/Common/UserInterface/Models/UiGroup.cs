using System;
using System.Collections.Generic;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user interface group.
	/// </summary>
	sealed public class UiGroup : IDisposable
	{
		/// <summary>
		/// Gets or sets the user interface group name.
		/// </summary>
		required public string Name { get; set; }

		/// <summary>
		/// Gets or sets the visibility group id.
		/// </summary>
		required public int VisibilityGroupId { get; set; }

		/// <summary>
		/// The user interface zones.
		/// </summary>
		readonly public List<UiZone> _zones = [];

		/// <summary>
		/// Disposes of the user interface group.
		/// </summary>
		public void Dispose()
		{
			foreach (var uiZone in this._zones ?? [])
			{
				uiZone?.Dispose();
			}
		}
	}
}
