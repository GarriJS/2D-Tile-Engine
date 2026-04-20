using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models
{
    /// <summary>
    /// Represents a user interface layout cache.
    /// </summary>
    sealed public class UiLayoutCache
    {
        /// <summary>
        /// Gets or sets the fixed width.
        /// </summary>
        required public float FixedSizedWidth { get; init; }

        /// <summary>
        /// Gets or sets the fixed sized height.
        /// </summary>
        required public float FixedSizedHeight { get; init; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public Vector2 Offset { get; set; }
        
        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        public SubArea InsideArea { get; set; }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        public SubArea TotalArea { get; set; }
    }
}
