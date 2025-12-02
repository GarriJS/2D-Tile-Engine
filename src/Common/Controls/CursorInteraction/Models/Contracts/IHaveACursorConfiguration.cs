using Common.Controls.CursorInteraction.Models.Abstract;
using System;

namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents a base cursor configuration.
	/// </summary>
	public interface IHaveACursorConfiguration : IDisposable
	{
		/// <summary>
		/// Gets the base cursor configuration.
		/// </summary>
		public BaseCursorConfiguration BaseCursorConfiguration { get; }
	}
}
