namespace Engine.RunTime.Constants
{
	/// <summary>
	/// Contains constants for the manager order.
	/// </summary>
	public static class ManagerOrderConstants
	{
		/// <summary>
		/// Gets the unused order.
		/// </summary>
		public static int UnusedOrder { get; } = 999;

		/// <summary>
		/// Gets the control manager update order.
		/// </summary>
		public static int ControlManagerUpdateOrder { get; } = 1;

		/// <summary>
		/// Gets the update manager update order.
		/// </summary>
		public static int UpdateManagerUpdateOrder { get; } = 2;

		/// <summary>
		/// Gets the draw manager draw order.
		/// </summary>
		public static int DrawMangerDrawOrder { get; } = 1;

		/// <summary>
		/// Gets the overlaid draw manager draw order.
		/// </summary>
		public static int OverlaidDrawMangerDrawOrder { get; } = 2;
	}
}
