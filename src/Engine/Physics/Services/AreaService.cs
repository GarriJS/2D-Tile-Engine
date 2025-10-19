using Engine.DiskModels.Physics;
using Engine.DiskModels.Physics.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Engine.Physics.Services
{
	/// <summary>
	/// Represents a area service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the animation service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class AreaService(GameServiceContainer gameServices) : IAreaService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the area from the model.
		/// </summary>
		/// <param name="areaModel">The area model.</param>
		/// <returns>The area.</returns>
		public IAmAArea GetAreaFromModel(IAmAAreaModel areaModel)
		{
			return this.GetAreaFromModel<IAmAArea>(areaModel);
		}

		/// <summary>
		/// Gets the area from the model.
		/// </summary>
		/// <param name="areaModel">The area model.</param>
		/// <param name="position">The position</param>
		/// <returns>The area.</returns>
		public T GetAreaFromModel<T>(IAmAAreaModel areaModel, Position position = null) where T : IAmAArea
		{
			var positionService = this._gameServices.GetService<IPositionService>();

			position ??= positionService.GetPositionFromModel(areaModel.Position);

			switch (areaModel)
			{
				case AreaCollectionModel areaCollectionModel:

					var width = 0f; // TODO calculate width and length
					var height = 0f;
					var areas = new OffsetSubArea[areaCollectionModel.SubAreas?.Length ?? 0];

					for (int i = 0; i < areas.Length; i++)
					{
						areas[i] = this.GetOffSetSubArea(areaCollectionModel.SubAreas[i]);
					}

					var areaCollection = new AreaCollection(width, height)
					{
						Position = position,
						SubAreas = areas
					};

					if (areaCollection is T resultCollection)
					{
						return resultCollection;
					}

					break;

				case OffsetAreaModel offsetAreaModel:

					var offsetArea = new OffsetArea
					{
						Width = offsetAreaModel.Width,
						Height = offsetAreaModel.Height,
						Position = position,
						VerticalOffset = offsetAreaModel.VerticalOffset,
						HorizontalOffset = offsetAreaModel.HorizontalOffset
					};

					if (offsetArea is T resultOffsetArea)
					{
						return resultOffsetArea;
					}

					break;

				case SimpleAreaModel simpleAreaModel:

					var simpleArea = new SimpleArea
					{
						Width = simpleAreaModel.Width,
						Height = simpleAreaModel.Height,
						Position = position,
					};

					if (simpleArea is T resultSimpleArea)
					{
						return resultSimpleArea;
					}

					break;
			}

			throw new InvalidCastException($"Cannot cast the area model of type {areaModel.GetType()} to {typeof(T)}.");
		}

		/// <summary>
		/// Gets the sub area.
		/// </summary>
		/// <param name="subAreaModel">The sub area model.</param>
		/// <returns>The sub area.</returns>
		public SubArea GetSubArea(SubAreaModel subAreaModel)
		{
			return new SubArea
			{
				Width = subAreaModel.Width,
				Height = subAreaModel.Height
			};
		}

		/// <summary>
		/// Gets the offset sub area.
		/// </summary>
		/// <param name="offsetSubAreaModel">The off set sub area model.</param>
		/// <returns>The offset sub area.</returns>
		public OffsetSubArea GetOffSetSubArea(OffsetSubAreaModel offsetSubAreaModel)
		{
			return new OffsetSubArea
			{
				Width = offsetSubAreaModel.Width,
				Height = offsetSubAreaModel.Height,
				HorizontalOffset = offsetSubAreaModel.HorizontalOffset,
				VerticalOffset = offsetSubAreaModel.VerticalOffset
			};
		}
	}
}
