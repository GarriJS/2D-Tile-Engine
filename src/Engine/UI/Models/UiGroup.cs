using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.UI.Models
{
	/// <summary>
	/// Represents a user interface group.
	/// </summary>
	public class UiGroup : IDisposable
	{
		/// <summary>
		/// Gets or sets the user interface group name.
		/// </summary>
		public string UiGroupName { get; set; }

		/// <summary>
		/// Gets or sets the visibility group id.
		/// </summary>
		public int VisibilityGroupId { get; set; }

		/// <summary>
		/// Gets or sets the user interface zones.
		/// </summary>
		public IList<UiZone> UiZones { get; set; }

		/// <summary>
		/// Disposes of the user interface group.
		/// </summary>
		public void Dispose()
		{
			if (true != this.UiZones?.Any())
			{ 
				return;
			}

			foreach (var uiZone in this.UiZones)
			{
				uiZone?.Dispose();
			}
		}
	}
}
