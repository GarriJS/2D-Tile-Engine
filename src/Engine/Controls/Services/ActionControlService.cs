using Engine.Controls.Enums;
using Engine.Controls.Models;
using Engine.Controls.Services.Contracts;
using Engine.Core.Files.Services.Contracts;
using Engine.Core.Initialization.Services;
using Engine.DiskModels;
using Engine.DiskModels.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Engine.Controls.Services
{
	/// <summary>
	/// Represents a action actionControlModel service.
	/// </summary>
	/// <remarks>
	/// Initializes the action actionControlModel service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class ActionControlService(GameServiceContainer gameServices) : IActionControlServices
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the controls file name.
		/// </summary>
		private string ControlsFileName { get; } = "Controls";

		/// <summary>
		/// Gets the action actionControlModels.
		/// </summary>
		public List<ActionControl> GetActionControls()
		{
			var jsonService = this._gameServices.GetService<IJsonService>();

			var actionControls = new List<ActionControl>();
			var contentManagerNames = LoadingInstructionsContainer.GetContentManagerNames();
			var serializer = new ModelSerializer<ActionControlModel>();

			foreach (var contentManagerName in contentManagerNames)
			{
				if (false == LoadingInstructionsContainer.TryGetContentManager(contentManagerName, out var contentManager))
				{
					continue;
				}

				var controlNames = LoadingInstructionsContainer.GetControlNamesForContentManager(contentManagerName);

				foreach (var controlName in controlNames)
				{
					using var stream = jsonService.GetJsonFileStream(contentManagerName, this.ControlsFileName, controlName);
					var actionControlModels = serializer.DeserializeList(stream);

					foreach (var actionControlModel in actionControlModels)
					{
						var actionControl = this.GetActionControlFromModel(actionControlModel);
						actionControls.Add(actionControl);
					}
				}
			}

			return actionControls;
		}

		/// <summary>
		/// Gets the action actionControlModel from the model.
		/// </summary>
		/// <param name="actionControlModel">The action actionControlModel model.</param>
		/// <returns>The action actionControlModel.</returns>
		public ActionControl GetActionControlFromModel(ActionControlModel actionControlModel)
		{
			Keys[] controlKeys = null;
			MouseButtonTypes[] controlMouseButtons = null;

			if ((null != actionControlModel.ControlKeys) &&
				(0 < actionControlModel.ControlKeys.Length))
			{
				controlKeys = new Keys[actionControlModel.ControlKeys.Length];

				for (int i = 0; i < actionControlModel.ControlKeys.Length; i++)
				{
					controlKeys[i] = (Keys)actionControlModel.ControlKeys[i];
				}
			}

			if ((null != actionControlModel.ControlMouseButtons) &&
				(0 < actionControlModel.ControlMouseButtons.Length))
			{
				controlMouseButtons = new MouseButtonTypes[actionControlModel.ControlMouseButtons.Length];

				for (int i = 0; i < actionControlModel.ControlMouseButtons.Length; i++)
				{
					controlMouseButtons[i] = (MouseButtonTypes)actionControlModel.ControlMouseButtons[i];
				}
			}

			return new ActionControl
			{ 
				ActionName = actionControlModel.ActionName,
				ControlKeys = controlKeys,
				ControlMouseButtons = controlMouseButtons
			};
		}

		/// <summary>
		/// Gets the action actionControlModel model.
		/// </summary>
		/// <param name="actionControl">The action actionControlModel.</param>
		/// <returns>The action actionControlModel model.</returns>
		private ActionControlModel GetActionControlModel(ActionControl actionControl)
		{
			int[] controlKeys = null;
			int[] controlMouseButtons = null;

			if ((null != actionControl.ControlKeys) &&
				(0 < actionControl.ControlKeys.Length))
			{
				controlKeys = new int[actionControl.ControlKeys.Length];

				for (int i = 0; i < actionControl.ControlKeys.Length; i++)
				{
					controlKeys[i] = (int)actionControl.ControlKeys[i];
				}
			}

			if ((null != actionControl.ControlMouseButtons) &&
				(0 < actionControl.ControlMouseButtons.Length))
			{
				controlMouseButtons = new int[actionControl.ControlMouseButtons.Length];

				for (int i = 0; i < actionControl.ControlMouseButtons.Length; i++)
				{
					controlMouseButtons[i] = (int)actionControl.ControlMouseButtons[i];
				}
			}

			return new ActionControlModel
			{ 
				ActionName = actionControl.ActionName,
				ControlKeys = controlKeys,
				ControlMouseButtons = controlMouseButtons
			};
		}
	}
}
