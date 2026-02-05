using Engine.RunTime.Models.Contracts;

namespace Engine.RunTime.Services.Contracts
{
	/// <summary>
	/// Represents a prerender service.
	/// </summary>
	public interface IPreRenderService
	{
		/// <summary>
		/// Adds the prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void AddPrerender(IAmPreRenderable prerender);

		/// <summary>
		/// Removes the prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void RemovePrerender(IAmPreRenderable prerender);

		/// <summary>
		/// Adds the overlaid prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void AddOverlaidPrerender(IAmPreRenderable prerender);

		/// <summary>
		/// Removes the overlaid prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void RemoveOverlaidPrerender(IAmPreRenderable prerender);
	}
}
