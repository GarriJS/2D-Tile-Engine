using Common.Controls.CursorInteraction.Models.Abstract;
using System;

namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents something with a hover configuration.
	/// </summary>
	public interface IHaveAHoverConfiguration : IDisposable
	{
		/// <summary>
		/// Gets the base hover configuration.
		/// </summary>
		public BaseHoverConfiguration BaseHoverConfig { get; }
	}
}
