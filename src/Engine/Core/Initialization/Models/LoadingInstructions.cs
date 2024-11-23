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
        public Dictionary<string, ContentManager> ContentManagers { get; set; }

        /// <summary>
        /// Gets or sets spritesheet linkages.
        /// </summary>
        public List<ContentManagerLinkage> SpritesheetLinkages { get; set; }

		/// <summary>
		/// Gets or sets spritesheet linkages.
		/// </summary>
		public List<ContentManagerLinkage> ImageLinkages { get; set; }
	}
}
