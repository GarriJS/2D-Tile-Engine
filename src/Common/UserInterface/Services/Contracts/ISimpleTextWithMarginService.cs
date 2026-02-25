using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a simple text with margin service.
	/// </summary>
	public interface ISimpleTextWithMarginService
	{
		/// <summary>
		/// Gets the simple text with margin from the model.
		/// </summary>
		/// <param name="model">The simple text with margin.</param>
		/// <returns>The simple text with margin.</returns>
		public SimpleTextWithMargin GetSimpleTextWithMarginFromModel(SinmpleTextWithMarginModel model);
	}
}
