using Engine.Drawables.Models;
using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.UI.Models.Enums;
using System;
using System.Collections.Generic;

namespace Engine.UI.Models
{
	/// <summary>
	/// Represents a user interface zone.
	/// </summary>
	public class UiZone : IAmDrawable, IHaveArea, IDisposable
	{
		/// <summary>
		/// Gets or sets the user interface zone name.
		/// </summary>
		public string UiZoneName { get; set; }

		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the user interface row justification type. 
		/// </summary>
		public UiZoneJustificationTypes JustificationType { get; set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		public Image Image { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.UserInterfaceScreenZone?.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get => this.UserInterfaceScreenZone?.Area; }

		/// <summary>
		/// Gets or sets the user interface screen zone.
		/// </summary>
		public UiScreenZone UserInterfaceScreenZone { get; set; }

		/// <summary>
		/// Gets or sets the element rows.
		/// </summary>
		public List<UiRow> ElementRows { get; set; }

		/// <summary>
		/// Disposes of the user interface container.
		/// </summary>
		public void Dispose()
		{
			this.Image.Dispose();

			foreach (var elementRow in this.ElementRows)
			{ 
				elementRow.Dispose();
			}
		}
	}
}
