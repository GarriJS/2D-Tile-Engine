using Common.Controls.CursorInteractions.Models.Abstract;
using Common.Controls.CursorInteractions.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Models.Contracts;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models
{
    /// <summary>
    /// Represents a user interface location descent.
    /// </summary>
    sealed public class UiLocationDescent
    {
        /// <summary>
        /// Gets the user interface block.
        /// </summary>
        public UiBlock UiBlock { get => this.UiLocatedBlock?.Subject; }

        /// <summary>
        /// Gets the user interface row.
        /// </summary>
        public UiRow UiRow { get => this.UiLocatedRow?.Subject; }

        /// <summary>
        /// Gets the user interface element.
        /// </summary>
        public IAmAUiElement UiElement { get => this.UiLocatedElement?.Subject; }

        /// <summary>
        /// Gets the top cursor configuration.
        /// </summary>
        public BaseCursorConfiguration TopCursorConfiguration 
        { 
            get => this.UiParent?.BaseCursorConfiguration ?? this.UiBlock?.BaseCursorConfiguration ?? this.UiRow?.BaseCursorConfiguration ?? this.UiElement?.BaseCursorConfiguration; 
        }

        /// <summary>
        /// Gets the top hover cursor.
        /// </summary>
        public Cursor TopHoverCursor
        {
            get => this.UiParent?.HoverCursor ?? this.UiBlock?.HoverCursor ?? this.UiRow?.HoverCursor ?? this.UiElement?.HoverCursor;
        }

        /// <summary>
        /// Gets the bottom scrollable.
        /// </summary>
        public IAmScrollable BottomScrollable
        {
            get => null == this.UiBlock ? this.UiParent : this.UiBlock;
        }

        /// <summary>
        /// Gets the primary user interface object.
        /// </summary>
        public ICanBeHovered PrimaryUiObject 
        { 
            get => this.UiElement ?? this.UiRow ?? this.UiBlock ?? (ICanBeHovered)this.UiParent;
        }

        /// <summary>
        /// Gets the primary user interface object location.
        /// </summary>
        public Vector2 PrimaryUiObjectLocation
        { 
            get => this.UiLocatedElement?.Vector2 ?? this.UiLocatedRow?.Vector2 ?? this.UiLocatedBlock?.Vector2 ?? this.UiParent?.Position.Coordinates ?? default;
        }

        /// <summary>
        /// Gets or Initializes the user interface parent.
        /// </summary>
        public IAmAUiParent UiParent { get; init; }

        /// <summary>
        /// Initializes the user interface located block.
        /// </summary>
        public Vector2Extender<UiBlock>? UiLocatedBlock { private get; init; }

        /// <summary>
        /// Initializes the user interface located row.
        /// </summary>
        public Vector2Extender<UiRow>? UiLocatedRow { private get; init; }

        /// <summary>
        /// Initializes the user interface located element.
        /// </summary>
        public Vector2Extender<IAmAUiElement>? UiLocatedElement { private get; init; }
    }
}
