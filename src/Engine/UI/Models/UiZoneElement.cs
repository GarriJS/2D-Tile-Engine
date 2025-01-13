using Engine.Drawing.Models;
using Engine.Drawing.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.UI.Models.Enums;
using System;
using System.Collections.Generic;

namespace Engine.UI.Models
{
	/// <summary>
	/// Represents a user interface zone element.
	/// </summary>
	public class UiZoneElement : IAmDrawable, IHaveArea, IDisposable
	{
		/// <summary>
		/// Gets or sets the user interface zone element name.
		/// </summary>
		public string UiZoneElementName { get; set; }

		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the user interface row justification type. 
		/// </summary>
		public UiZoneElementJustificationTypes JustificationType { get; set; }

		/// <summary>
		/// Gets or sets the background.
		/// </summary>
		public Image Background { get; set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		public Image Image { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.UserInterfaceZone?.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }

		/// <summary>
		/// Gets or sets the user interface zone.
		/// </summary>
		public UiZone UserInterfaceZone { get; set; }

		/// <summary>
		/// Gets or sets the element rows.
		/// </summary>
		public List<UiRow> ElementRows { get; set; }

		/// <summary>
		/// Disposes of the user interface element container.
		/// </summary>
		public void Dispose()
		{
			this.Background.Dispose();

			foreach (var elementRow in this.ElementRows)
			{ 
				elementRow.Dispose();
			}
		}
	}
}
