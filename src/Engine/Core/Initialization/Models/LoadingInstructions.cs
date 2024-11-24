using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Engine.Core.Initialization.Models
{
	/// <summary>
	/// Represents loading instructions.
	/// </summary>
	public class LoadingInstructions
    {
        /// <summary>
        /// Gets or sets the content managers.
        /// </summary>
        public required Dictionary<string, ContentManager> ContentManagers { get; set; }

        /// <summary>
        /// Gets or sets tile set linkages.
        /// </summary>
        public required List<ContentManagerLinkage> TilesetLinkages { get; set; }

		/// <summary>
		/// Gets or sets spritesheet linkages.
		/// </summary>
		public required List<ContentManagerLinkage> ImageLinkages { get; set; }

		/// <summary>
		/// Gets or sets the control linkages.
		/// </summary>
		public required List<ContentManagerLinkage> ControlLinkages { get; set; }
	}
}
