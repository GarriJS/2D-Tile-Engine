using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Drawing.Contracts;
using Engine.Physics.Models.SubAreas;
using Engine.RunTime.Models.Contracts;
using System;

namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represent something that is a graphic.
	/// </summary>
	public interface IAmAGraphic : IAmSubDrawable, ICanBeSerialized<IAmAGraphicModel>, IDisposable
	{
		/// <summary>
		/// Sets the draw dimensions.
		/// </summary>
		/// <param name="dimensions">The dimensions.</param>
		public void SetDrawDimensions(SubArea dimensions);
	}
}
