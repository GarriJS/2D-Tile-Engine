using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Engine.Core.Initialization.Models
{
	/// <summary>
	/// Represents loading instructions.
	/// </summary>
	sealed public class LoadingInstructions
    {
		/// <summary>
		/// Gets or sets the content managers.
		/// </summary>
		required public Dictionary<string, ContentManager> ContentManagers { get; set; }

		/// <summary>
		/// Gets or sets the control linkages.
		/// </summary>
		required public List<ContentManagerLinkage> ControlLinkages { get; set; }

		/// <summary>
		/// Gets or sets the font linkages.
		/// </summary>
		required public List<ContentManagerLinkage> FontLinkages { get; set; }

		/// <summary>
		/// Gets or sets the tile set linkages.
		/// </summary>
		required public List<ContentManagerLinkage> TilesetLinkages { get; set; }

		/// <summary>
		/// Gets or sets the spritesheet linkages.
		/// </summary>
		required public List<ContentManagerLinkage> ImageLinkages { get; set; }
	}
}
