using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;

namespace Common.UserInterface.Services.Contracts
{   
	/// <summary>
	/// Represents a user interface row service.
	/// </summary>
	public interface IUiRowService
	{
		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface block row.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRowFromModel(UiRowModel uiRowModel);

		/// <summary>
		/// Splits the row to accommodate the max width.
		/// </summary>
		/// <param name="uiRow">The user interface row.</param>
		/// <param name="maxWidth">The max width.</param>
		/// <param name="targetWidth">The target width.</param>
		/// <returns>The split rows.</returns>
		public UiRow[] SplitRow(UiRow uiRow, float maxWidth, float? targetWidth = null);
	}
}
