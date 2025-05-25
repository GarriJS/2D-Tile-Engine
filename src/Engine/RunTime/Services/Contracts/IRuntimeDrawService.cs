using Engine.Drawables.Models.Contracts;

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
		/// <param name="drawable">The drawable.</param>
		public void AddDrawable(ICanBeDrawn drawable);

		/// <summary>
		/// Adds the overlaid drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void AddOverlaidDrawable(ICanBeDrawn drawable);

		/// <summary>
		/// Removes the drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveDrawable(ICanBeDrawn drawable);

		/// <summary>
		/// Removes the overlaid drawable.
		/// </summary>
		/// <param name="drawable">The drawable.</param>
		public void RemoveOverlaidDrawable(ICanBeDrawn drawable);

		/// <summary>
		/// Changes the drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeDrawableLayer(int layer, ICanBeDrawn drawable);

		/// <summary>
		/// Changes the overlaid drawable layer.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="drawable">The drawable.</param>
		public void ChangeOverlaidDrawableLayer(int layer, ICanBeDrawn drawable);
	}
}
