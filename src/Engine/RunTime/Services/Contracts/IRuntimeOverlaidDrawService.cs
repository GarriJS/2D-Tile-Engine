using Engine.RunTime.Models.Contracts;

namespace Engine.RunTime.Services.Contracts
{
    /// <summary>
    /// Represents a runtime overlaid draw manager.
    /// </summary>
    public interface IRuntimeOverlaidDrawService
	{
		/// <summary>
		/// Adds the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void AddDrawable(IAmDrawable drawable);

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawable(IAmDrawable drawable);

		/// <summary>
		/// Changes the drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeDrawableLayer(int layer, IAmDrawable drawable);
	}
}
