namespace Engine.Core.Initialization.Models
{
	/// <summary>
	/// Represents a content manager linkage.
	/// </summary>
	sealed public class ContentManagerLinkage
	{
		/// <summary>
		/// Gets or sets the content manager name.
		/// </summary>
		required public string ContentManagerName { get; set; }

		/// <summary>
		/// Gets or sets the content name.
		/// </summary>
		required public string ContentName { get; set; }
	}
}
