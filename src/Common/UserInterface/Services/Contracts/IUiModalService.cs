using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;
using System.Collections.Generic;

namespace Common.UserInterface.Services.Contracts
{   
	/// <summary>
	/// Represents a user interface modal service.
	/// </summary>
	public interface IUiModalService
	{
		/// <summary>
		/// Gets the active user interface modals.
		/// </summary>
		public List<UiModal> ActiveUiModals { get; }

		/// <summary>
		/// Gets the user interface modal from the model.
		/// </summary>
		/// <param name="uiModalModel">The user interface modal model.</param>
		/// <param name="makeActive">A value indicating whether to make the modal active.</param>
		/// <returns>The user interface modal.</returns>
		public UiModal GetUiModalFromModel(UiModalModel uiModalModel, bool makeActive);

		/// <summary>
		/// Add the active modal.
		/// </summary>
		/// <param name="uiModal">The user interface modal.</param>
		/// <returns>A value indicating whether the user interface modal was not active.</returns>
		public bool AddActiveModal(UiModal uiModal);

		/// <summary>
		/// Removes the active modal.
		/// </summary>
		/// <param name="uiModal">The user interface modal.</param>
		/// <returns>A value indicating whether the user interface modal was active.</returns>
		public bool RemoveActiveModal(UiModal uiModal);
	}
}
