using Common.UI.Models.Enums;
using Engine.Graphics.Models;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.UI.Models.Contracts
{
    /// <summary>
    /// Represents a user interface element.
    /// </summary>
    public interface IAmAUiElement : IAmSubDrawable, ICanBeHovered<IAmAUiElement>, ICanBePressed, IDisposable
	{
        /// <summary>
        /// Gets or sets the user interface element name.
        /// </summary>
        public string UiElementName { get; set; }

		/// <summary>
		/// Gets or sets the visibility group.
		/// </summary>
		public int VisibilityGroup { get; set; }

		/// <summary>
		/// Gets or sets the left padding.
		/// </summary>
		public float LeftPadding { get; set; }

		/// <summary>
		/// Gets or sets the right padding.
		/// </summary>
		public float RightPadding { get; set; }

		/// <summary>
		/// Gets or sets the user interface element type.
		/// </summary>
		public UiElementTypes ElementType { get; set; }

		/// <summary>
		/// Gets or sets the user interface size type.
		/// </summary>
		public UiElementSizeTypes SizeType { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public Vector2 Area { get; set; }

		/// <summary>
		/// Gets the graphic.
		/// </summary>
		public new Image Graphic { get; set; }
	}
}
