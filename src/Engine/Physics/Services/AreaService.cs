using DiscModels.Engine.Physics;
using DiscModels.Engine.Physics.Contracts;
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
		/// Gets the area.
		/// </summary>
		/// <param name="areaModel">The area model.</param>
		/// <param name="position">The position.</param>
		/// <returns>The area.</returns>
		public IAmAArea GetArea(IAmAAreaModel areaModel, Position position = null)
		{ 
			return this.GetArea<IAmAArea>(areaModel, position);
		}

		/// <summary>
		/// Gets the area.
		/// </summary>
		/// <param name="areaModel">The area model.</param>
		/// <param name="position">The position.</param>
		/// <returns>The area.</returns>
		public T GetArea<T>(IAmAAreaModel areaModel, Position position = null) where T : IAmAArea 
		{
			var positionService = this._gameServices.GetService<IPositionService>();
			position ??= positionService.GetPosition(areaModel.Position);

			switch (areaModel)
			{
				case AreaCollectionModel areaCollectionModel:

					var areas = new OffsetArea[areaCollectionModel.Areas?.Length ?? 0];
					var width = 0f;
					var height = 0f;

					for (int i = 0; i < areaCollectionModel.Areas?.Length; i++)
					{
						areas[i] = this.GetArea<OffsetArea>(areaCollectionModel.Areas[i], position);
						
						var subAreaWidth = areas[i].Width;
						var subAreaHeight = areas[i].Height;

						if (areas[i] is OffsetArea subOffsetArea)
						{
							subAreaWidth += subOffsetArea.HorizontalOffset;
							subAreaHeight += subOffsetArea.VerticalOffset;
						}

						if (subAreaWidth > width)
						{ 
							width = subAreaWidth;
						}

						if (subAreaHeight > height)
						{ 
							height = subAreaHeight;
						}
					}

					var areaCollection = new AreaCollection(width, height)
					{
						Position = position,
						Areas = areas
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
	}
}
