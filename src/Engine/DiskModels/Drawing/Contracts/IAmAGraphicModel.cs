using Engine.DiskModels.Physics;
using Engine.Physics.Models.SubAreas;

namespace Engine.DiskModels.Drawing.Contracts
{
	public interface IAmAGraphicModel
	{
		public SubArea GetDimensions();

		public void SetDrawDimensions(SubAreaModel dimensions);
	}
}
