using Engine.Controls.Enums;
using Engine.Controls.Models;
using Engine.Controls.Services.Contracts;
using Engine.Core.Initialization;
using Engine.DiskModels.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Engine.Controls.Services
{
	/// <summary>
	/// Represents a action control service.
	/// </summary>
	/// <remarks>
	/// Initializes the action control service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class ActionControlService(GameServiceContainer gameServices) : IActionControlServices
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the action controls.
		/// </summary>
		public List<ActionControl> GetActionControls()
		{
			var actionControls = new List<ActionControl>();
			var contentManagerNames = LoadingInstructionsContainer.GetContentManagerNames();

			foreach (var contentManagerName in contentManagerNames)
			{
				if (false == LoadingInstructionsContainer.TryGetContentManager(contentManagerName, out var contentManager))
				{
					continue;
				}

				var managerFontNames = LoadingInstructionsContainer.GetControlNamesForContentManager(contentManagerName);
				var serializer = new DataContractJsonSerializer(typeof(List<ActionControlModel>));

				foreach (var managerFontName in managerFontNames)
				{
					var controlFilePath = Path.Combine(Directory.GetCurrentDirectory(), contentManagerName, "Controls", $"{managerFontName}.json");
					var controlJson = File.ReadAllText(controlFilePath);

					using var stream = new MemoryStream(Encoding.UTF8.GetBytes(controlJson));
					{
						var controls = (List<ActionControlModel>)serializer.ReadObject(stream);

						foreach (var control in controls)
						{
							actionControls.Add(GetActionControl(control));
						}
					}
				}
			}

			return actionControls;
		}

		/// <summary>
		/// Gets the action control.
		/// </summary>
		/// <param name="actionControlModel">The action control model.</param>
		/// <returns>The action control.</returns>
		private ActionControl GetActionControl(ActionControlModel actionControlModel)
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
				ActionType = (ActionTypes)actionControlModel.ActionType,
				ControlKeys = controlKeys,
				ControlMouseButtons = controlMouseButtons
			};
		}

		/// <summary>
		/// Gets the action control model.
		/// </summary>
		/// <param name="actionControl">The action control.</param>
		/// <returns>The action control model.</returns>
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
				ActionControlDescription = actionControl.ActionControlDescription,
				ActionType = (int)actionControl.ActionType,
				ControlKeys = controlKeys,
				ControlMouseButtons = controlMouseButtons
			};
		}
	}
}
