namespace Engine.Physics.Models.Contracts
{
	/// <summary>
	/// Represents a sub area. 
	/// </summary>
	public interface IHaveASubArea
	{
		/// <summary>
		/// Gets the area.
		/// </summary>
		public SubArea Area { get; set; }
	}
}
