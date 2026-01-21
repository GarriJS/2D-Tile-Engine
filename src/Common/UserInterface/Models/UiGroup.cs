using System;
using System.Collections.Generic;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user interface group.
	/// </summary>
	public class UiGroup : IDisposable
	{
		/// <summary>
		/// Gets or sets the user interface group name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the visibility group id.
		/// </summary>
		public int VisibilityGroupId { get; set; }

		/// <summary>
		/// Gets or sets the user interface zones.
		/// </summary>
		public IList<UiZone> Zones { get; set; }

		/// <summary>
		/// Disposes of the user interface group.
		/// </summary>
		public void Dispose()
		{
			foreach (var uiZone in this.Zones ?? [])
			{
				uiZone?.Dispose();
			}
		}
	}
}
