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
		public void AddPrerender(IRequirePreRender prerender);

		/// <summary>
		/// Removes the prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void RemovePrerender(IRequirePreRender prerender);

		/// <summary>
		/// Adds the overlaid prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void AddOverlaidPrerender(IRequirePreRender prerender);

		/// <summary>
		/// Removes the overlaid prerender.
		/// </summary>
		/// <param name="prerender">The prerender.</param>
		public void RemoveOverlaidPrerender(IRequirePreRender prerender);
	}
}
