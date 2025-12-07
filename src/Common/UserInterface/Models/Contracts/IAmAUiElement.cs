using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Enums;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models.Contracts;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.UserInterface.Models.Contracts
{
    /// <summary>
    /// Represents a user interface element.
    /// </summary>
    public interface IAmAUiElement : IAmSubDrawable, IHaveASubArea, IHaveAHoverCursor, ICanBeHovered<IAmAUiElement>, ICanBePressed<IAmAUiElement>, IDisposable
	{
		/// <summary>
        /// Gets or sets the user interface element name.
        /// </summary>
        public string UiElementName { get; set; }

		/// <summary>
		/// Gets the total width.
		/// </summary>
		public float TotalWidth { get; }

		/// <summary>
		/// Gets the total height.
		/// </summary>
		public float TotalHeight { get; }

		/// <summary>
		/// Gets the inside width.
		/// </summary>
		public float InsideWidth { get; }

		/// <summary>
		/// Gets the inside height.
		/// </summary>
		public float InsideHeight { get; }

		/// <summary>
		/// Gets or sets the horizontal user interface size type.
		/// </summary>
		public UiElementSizeType HorizontalSizeType { get; set; }

		/// <summary>
		/// Gets or sets the vertical user interface size type.
		/// </summary>
		public UiElementSizeType  VerticalSizeType { get; set; }

		/// <summary>
		/// Gets or sets the cached element offset.
		/// </summary>
		public Vector2? CachedElementOffset { get; set; }

		/// <summary>
		/// Gets or sets the user interface margin.
		/// </summary>
		public UiMargin Margin { get; set; }

		/// <summary>
		/// Gets the Graphic.
		/// </summary>
		public IAmAGraphic Graphic { get; set; }
	}
}
