using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a position.
	/// </summary>
	sealed public class Position : ICanBeSerialized<PositionModel>
	{
		/// <summary>
		/// Gets or sets the coordinates.
		/// </summary>
		required public Vector2 Coordinates { get; set; }

		/// <summary>
		/// Get the position as a point.
		/// </summary>
		public Point ToPoint { get => this.Coordinates.ToPoint(); }

		/// <summary>
		/// Gets or sets the X coordinate.
		/// </summary>
		public float X
		{
			get => this.Coordinates.X;
			set => this.Coordinates = new Vector2(value, this.Coordinates.Y);
		}

		/// <summary>
		/// Gets or sets the Y coordinate.
		/// </summary>
		public float Y
		{
			get => this.Coordinates.Y;
			set => this.Coordinates = new Vector2(this.Coordinates.X, value);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public PositionModel ToModel()
		{
			var result = new PositionModel
			{
				X = this.X,
				Y = this.Y,
			};

			return result;
		}
	}
}

