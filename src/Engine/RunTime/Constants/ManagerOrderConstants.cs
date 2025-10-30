namespace Engine.RunTime.Constants
{
	/// <summary>
	/// Contains constants for the manager order.
	/// </summary>
	static public class ManagerOrderConstants
	{
		/// <summary>
		/// Gets the early update order.
		/// </summary>
		static public int EarlyUpdateOrder { get; } = -100;

		/// <summary>
		/// Gets the unused order.
		/// </summary>
		static public int UnusedOrder { get; } = 9999;

		/// <summary>
		/// Gets the control manager update order.
		/// </summary>
		static public int ControlManagerUpdateOrder { get; } = 1;

		/// <summary>
		/// Gets the update manager update order.
		/// </summary>
		static public int UpdateManagerUpdateOrder { get; } = 2;

		/// <summary>
		/// Gets the draw manager draw order.
		/// </summary>
		static public int DrawMangerDrawOrder { get; } = 1;

		/// <summary>
		/// Gets the overlaid draw manager draw order.
		/// </summary>
		static public int OverlaidDrawMangerDrawOrder { get; } = 2;
	}
}
