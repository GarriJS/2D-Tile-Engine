using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Enums;
using Engine.Graphics.Models;
using Engine.Physics.Models.Contracts;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.UserInterface.Models.Contracts
{
    /// <summary>
    /// Represents a user interface element.
    /// </summary>
    public interface IAmAUiElement : IAmSubDrawable, IHaveASubArea, ICanBeHovered<IAmAUiElement>, ICanBePressed<IAmAUiElement>, IDisposable
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
		new public Vector2 Area { get; set; }

		/// <summary>
		/// Gets the graphic.
		/// </summary>
		public Image Graphic { get; set; }
	}
}
