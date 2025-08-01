using Engine.RunTime.Models.Contracts;

namespace Engine.RunTime.Services.Contracts
{
    /// <summary>
    /// Represents a runtime draw manager.
    /// </summary>
    public interface IRuntimeDrawService
	{
		/// <summary>
		/// Gets or sets a value indicating whether the draw is in progress.
		/// </summary>
		public bool DrawInProgress { get; set; } 

		/// <summary>
		/// Gets or sets a value indicating whether the overlaid draw is in progress.
		/// </summary>
		public bool OverlaidDrawInProgress { get; set; }

		/// <summary>
		/// Adds the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void AddDrawable(IAmDrawable drawable);

		/// <summary>
		/// Adds the overlaid drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void AddOverlaidDrawable(IAmDrawable drawable);

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawable(IAmDrawable drawable);

		/// <summary>
		/// Removes the overlaid drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveOverlaidDrawable(IAmDrawable drawable);

		/// <summary>
		/// Changes the drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeDrawableLayer(int layer, IAmDrawable drawable);

		/// <summary>
		/// Changes the overlaid drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeOverlaidDrawableLayer(int layer, IAmDrawable drawable);
	}
}
