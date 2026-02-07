using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;

namespace Common.UserInterface.Services.Contracts
{   
	/// <summary>
	/// Represents a user interface row service.
	/// </summary>
	public interface IUserInterfaceRowService
	{
		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface block row.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRowFromModel(UiRowModel uiRowModel);

		/// <summary>
		/// Splits the block to accommodate  
		/// </summary>
		/// <param name="uiRow">The user interface block.</param>
		/// <param name="originalModel">The original row model.</param>
		/// <param name="maxWidth">The max width.</param>
		/// <returns>The user interface rows.</returns>
		public UiRow[] SplitRow(UiRow uiRow, UiRowModel originalModel, float maxWidth);

		/// <summary>
		/// Updates the block dynamic height.
		/// </summary>
		/// <param name="uiRow">The user interface block.</param>
		/// <param name="dynamicHeight">The dynamic height.</param>
		public void UpdateRowDynamicHeight(UiRow uiRow, float dynamicHeight);
	}
}
