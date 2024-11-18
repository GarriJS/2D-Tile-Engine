using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Engine.Core.Initialization
{
	/// <summary>
	/// Represents launch settings.
	/// </summary>
	public class LaunchSettings
	{
		/// <summary>
		/// Gets or sets the content managers.
		/// </summary>
		public Dictionary<string, ContentManager> ContentManagers { get; set; }
	}
}
