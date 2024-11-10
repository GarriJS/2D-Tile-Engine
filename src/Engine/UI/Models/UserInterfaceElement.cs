using Engine.Drawing.Models;
using Engine.Drawing.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.Signals.Models;
using Engine.Signals.Models.Contracts;
using System;
using System.Collections.Generic;

namespace Engine.UI.Models
{
	/// <summary>
	/// Represents a user interface element.
	/// </summary>
	public class UserInterfaceElement : IAmDrawable, IHaveArea, IEmitSignals, IReceiveSignals
	{
		/// <summary>
		/// Gets or sets a value indicating whether the user interface element is visible. 
		/// </summary>
		public bool IsVisible { get; set; }

		/// <summary>
		/// Gets or sets the user interface element name.
		/// </summary>
		public string UserInterfaceElementName { get; set; }

		/// <summary>
		/// Gets or sets the sprite.
		/// </summary>
		public Sprite Sprite { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }

		/// <summary>
		/// Gets or sets the signal response processor type.
		/// </summary>
		public Type SignalResponseProcessorType { get; set; }

		/// <summary>
		/// Gets the queue of received signals.
		/// </summary>
		public Queue<Signal> SignalResponses { get; set; }

		/// <summary>
		/// Gets the active signal subscriptions.
		/// </summary>
		public IList<SignalSubscription> ActiveSignalSubscriptions { get; set; }

		/// <summary>
		/// Disposes of the user interface element.
		/// </summary>
		public void Dispose()
		{
			this.Sprite.Dispose();
		}
	}
}
