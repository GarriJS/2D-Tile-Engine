using Engine.DiskModels.Physics;
using Engine.Physics.Models.SubAreas;

namespace Engine.DiskModels.Drawing.Abstract
{
	abstract public class GraphicBaseModel : BaseDiskModel
	{
		abstract public SubArea GetDimensions();

		abstract public void SetDrawDimensions(SubAreaModel dimensions);
	}
}
