using Engine.Drawing.Models.Contracts;

namespace Engine.RunTime.Services.Contracts
{   
	/// <summary>
	/// Represents a runtime draw manager.
	/// </summary>
	public interface IRuntimeDrawService
	{
		/// <summary>
		/// Adds the drawable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void AddDrawable(int layer, IAmDrawable drawable);

		/// <summary>
		/// Adds the overlaid drawable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void AddOverlaidDrawable(int layer, IAmDrawable drawable);

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawable(int layer, IAmDrawable drawable);

		/// <summary>
		/// Removes the overlaid drawable.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void RemoveOverlaidDrawable(int layer, IAmDrawable drawable);

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
