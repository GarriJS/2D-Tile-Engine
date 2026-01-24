namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user interface margin.
	/// </summary>
	public struct UiMargin
	{
		/// <summary>
		/// Gets or sets the top margin.
		/// </summary>
		public float TopMargin { get; set; }

		/// <summary>
		/// Gets or sets the bottom margin.
		/// </summary>
		public float BottomMargin { get; set; }

		/// <summary>
		/// Gets or sets the left margin.
		/// </summary>
		public float LeftMargin { get; set; }

		/// <summary>
		/// Gets or sets the right margin.
		/// </summary>
		public float RightMargin { get; set; }

		/// <summary>
		/// Gets a copy of the user interface margin.
		/// </summary>
		/// <returns>A copy of the user interface margin.</returns>
		public UiMargin Copy() 
		{
			var result = new UiMargin
			{ 
				TopMargin = this.TopMargin,
				BottomMargin = this.BottomMargin,	
				LeftMargin = this.LeftMargin,
				RightMargin = this.RightMargin
			};

			return result;
		}
	}
}
