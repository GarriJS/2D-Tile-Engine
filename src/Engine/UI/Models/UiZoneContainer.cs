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
	/// Represents a user interface zone element container.
	/// </summary>
	public class UiZoneContainer : IAmDrawable, IHaveArea, IDisposable
	{
		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the user interface zone container justification type. 
		/// </summary>
		public UiZoneContainerJustificationTypes ZoneContainerJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the background.
		/// </summary>
		public Sprite Background { get; set; }

		/// <summary>
		/// Gets or sets the sprite.
		/// </summary>
		public Sprite Sprite { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.UserInterfaceZone.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get => this.UserInterfaceZone.Area; }

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
