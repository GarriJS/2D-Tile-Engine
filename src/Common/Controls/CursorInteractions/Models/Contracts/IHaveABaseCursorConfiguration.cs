using Common.Controls.CursorInteractions.Models.Abstract;
using Engine.Physics.Models.Contracts;
using System;

namespace Common.Controls.CursorInteractions.Models.Contracts
{
	/// <summary>
	/// Represents something with a base cursor configuration.
	/// </summary>
	public interface IHaveABaseCursorConfiguration : IHaveASubArea, IDisposable
	{
        /// <summary>
        /// Gets the base cursor configuration.
        /// </summary>
        public BaseCursorConfiguration BaseCursorConfiguration { get; }
    }
}
