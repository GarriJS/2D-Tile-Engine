using System;
using System.Collections.Generic;
using Engine.Drawing.Models;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Enums;

namespace Engine.UI.Models
{
    /// <summary>
    /// Represents a user interface row.
    /// </summary>
    public class UiRow : IDisposable
	{
		/// <summary>
		/// Gets or sets the user interface row name.
		/// </summary>
		public string UiRowName { get; set; }

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public float Height { get; set; }

		/// <summary>
		/// Gets or sets the top padding.
		/// </summary>
		public float TopPadding { get; set; }

		/// <summary>
		/// Gets or sets the bottom padding.
		/// </summary>
		public float BottomPadding { get; set; }

		/// <summary>
		/// Gets or sets the user interface row horizontal justification type. 
		/// </summary>
		public UiRowHorizontalJustificationTypes HorizontalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the user interface row vertical justification type.
		/// </summary>
		public UiRowVerticalJustificationTypes VerticalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		public Image Image { get; set; }

		/// <summary>
		/// Gets or sets the sub elements.
		/// </summary>
		public List<IAmAUiElement> SubElements { get; set; }

		/// <summary>
		/// Disposes of the user interface row.
		/// </summary>
		public void Dispose()
		{
			foreach (var subElement in this.SubElements)
			{ 
				subElement.Dispose();
			}
		}
	}
}
