namespace Engine.Core.Contracts
{
	/// <summary>
	/// Represents something that does post load initialization.
	/// </summary>
	public interface IPostLoadInitialize
	{
		/// <summary>
		/// Does the post load initialization.
		/// </summary>
		public void PostLoadInitialize();
	}
}
