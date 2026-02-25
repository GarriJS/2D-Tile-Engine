using Common.DiskModels.UserInterface;

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
		required public float TopMargin { get; set; }

		/// <summary>
		/// Gets or sets the bottom margin.
		/// </summary>
		required public float BottomMargin { get; set; }

		/// <summary>
		/// Gets or sets the left margin.
		/// </summary>
		required public float LeftMargin { get; set; }

		/// <summary>
		/// Gets or sets the right margin.
		/// </summary>
		required public float RightMargin { get; set; }

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

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		readonly public UiMarginModel ToModel()
		{
			var result = new UiMarginModel
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
