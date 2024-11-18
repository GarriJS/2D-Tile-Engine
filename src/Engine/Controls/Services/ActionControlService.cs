using DiscModels.Engine.Controls;
using Engine.Controls.Models;
using Engine.Controls.Models.Enums;
using Engine.Controls.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			var contentManager = this._gameServices.GetService<ContentManager>();

			var actionControls = new List<ActionControl>();
			var controlsPath = $@"{contentManager.RootDirectory}\Controls";
			string[] controlFiles = Directory.GetFiles(controlsPath);

			if (false == controlFiles.Any())
			{
				return actionControls;
			}

			foreach (string controlFile in controlFiles)
			{
				var jsonContent = File.ReadAllText(controlFile);
				var serializer = new DataContractJsonSerializer(typeof(List<ActionControlModel>));
				using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));
				{
					var controls = (List<ActionControlModel>)serializer.ReadObject(stream);
					foreach (var control in controls)
					{
						var actionControl = GetActionControl(control);
						actionControls.Add(actionControl);
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

			if (true == actionControlModel.ControlKeys?.Any())
			{
				controlKeys = new Keys[actionControlModel.ControlKeys.Length];

				for (int i = 0; i < actionControlModel.ControlKeys.Length; i++)
				{
					controlKeys[i] = (Keys)actionControlModel.ControlKeys[i];
				}
			}

			if (true == actionControlModel.ControlMouseButtons?.Any())
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

			if (true == actionControl.ControlKeys?.Any())
			{
				controlKeys = new int[actionControl.ControlKeys.Length];

				for (int i = 0; i < actionControl.ControlKeys.Length; i++)
				{
					controlKeys[i] = (int)actionControl.ControlKeys[i];
				}
			}

			if (true == actionControl.ControlMouseButtons?.Any())
			{
				controlMouseButtons = new int[actionControl.ControlMouseButtons.Length];

				for (int i = 0; i < actionControl.ControlMouseButtons.Length; i++)
				{
					controlMouseButtons[i] = (int)actionControl.ControlMouseButtons[i];
				}
			}

			return new ActionControlModel
			{
				ActionType = (int)actionControl.ActionType,
				ControlKeys = controlKeys,
				ControlMouseButtons = controlMouseButtons
			};
		}
	}
}
