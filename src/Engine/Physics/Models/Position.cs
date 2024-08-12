using Microsoft.Xna.Framework;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a position.
	/// </summary>
	public class Position
	{
		private Vector2 _coordinates;

		/// <summary>
		/// Gets or sets the coordinates.
		/// </summary>
		public Vector2 Coordinates { get => this._coordinates; set => this._coordinates = value; }

		/// <summary>
		/// Get the position as a point.
		/// </summary>
		public Point ToPoint { get => this._coordinates.ToPoint(); }

		/// <summary>
		/// Gets or sets the X coordinate.
		/// </summary>
		public float X { get => this._coordinates.X; set => this._coordinates.X = value; }

		/// <summary>
		/// Gets or sets the Y coordinate.
		/// </summary>
		public float Y { get => this._coordinates.Y; set => this._coordinates.Y = value; }
	}
}

