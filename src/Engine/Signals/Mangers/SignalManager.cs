using Engine.Signals.Models;
using Engine.Signals.Models.Contracts;
using Engine.Signals.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Signals.Mangers
{
	/// <summary>
	/// Represents a signal manager.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the signals manager.
	/// </remarks>
	/// <param name="game">The game.</param>
	public class SignalManager(Game game) : GameComponent(game), ISignalService
	{
		/// <summary>
		/// Gets or sets the signals.
		/// </summary>
		private Queue<Signal> Signals { get; set; } = [];

		/// <summary>
		/// Gets or sets the signal subscriptions.
		/// </summary>
		private Dictionary<string, List<SignalSubscription>> SignalSubscriptions { get; set; } = [];

		/// <summary>
		/// Initializes the signal manager.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
		}

		/// <summary>
		/// Adds the signals.
		/// </summary>
		/// <param name="signal">The signal.</param>
		public void AddSignal(Signal signal)
		{
			this.Signals.Enqueue(signal);
		}

		/// <summary>
		/// Adds the subscription.
		/// </summary>
		/// <param name="signalType">The signal type.</param>
		/// <param name="subscription">The subscription.</param>
		public void SubscribeToSignalType(string signalType, SignalSubscription subscription)
		{
			if (true == this.SignalSubscriptions.TryGetValue(signalType, out var subscriptionList))
			{
				subscriptionList.Add(subscription);
			}
			else
			{
				subscriptionList = [subscription];
				this.SignalSubscriptions.Add(signalType, subscriptionList);
			}
		}

		/// <summary>
		/// Removes the subscription.
		/// </summary>
		/// <param name="signalType">The signal type.</param>
		/// <param name="subscription">The subscription.</param>
		public void UnsubscribeToSignalType(string signalType, SignalSubscription subscription)
		{
			if (true == this.SignalSubscriptions.TryGetValue(signalType, out var subscriptionList))
			{
				subscriptionList.Remove(subscription);
			}
		}

		/// <summary>
		/// Changes the subscription.
		/// </summary>
		/// <param name="signalType">The signal type.</param>
		/// <param name="subscription">The subscription.</param>
		public void ChangeSubscription(string signalType, SignalSubscription subscription)
		{
			this.UnsubscribeToSignalType(signalType, subscription);
			this.SubscribeToSignalType(signalType, subscription);
		}

		/// <summary>
		/// Processes the signal responses.
		/// </summary>
		/// <param name="emitter"></param>
		public void ProcessSignalResponses(IEmitSignals emitter)
		{
			var service = this.Game.Services.GetService(emitter.SignalResponseProcessorType);

			if ((service is not IProcessSignals signalProcessor) ||
				(emitter is not IReceiveSignals signalReceiver))
			{
				return;
			}

			while (true == emitter.SignalResponses.TryDequeue(out var signal))
			{
				signalProcessor.ProcessSignal(signalReceiver, signal, false);
			}
		}

		/// <summary>
		/// Updates and applies the signals.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Update(GameTime gameTime)
		{
			while (true == this.Signals.TryDequeue(out var signal))
			{
				if (false == this.SignalSubscriptions.TryGetValue(signal.SignalType, out var subscriptionList))
				{
					continue;
				}

				foreach (var subscription in subscriptionList)
				{
					var service = this.Game.Services.GetService(subscription.SignalProcessorType);

					if (service is IProcessSignals signalProcessor)
					{
						signalProcessor.ProcessSignal(subscription.Subscriber, signal);
					}
				}
			}

			base.Update(gameTime);
		}
	}
}
