using DiscModels.Engine.Drawing;
using DiscModels.Engine.Physics;
using DiscModels.Engine.Tiling;
using Engine.Core.Constants;
using Engine.Terminal.Commands.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Terminal.Commands
{
	/// <summary>
	/// Provides methods for executing code.
	/// </summary>
	public static class CommandExecuter
	{
		/// <summary>
		/// Executes the arguments.
		/// </summary>
		/// <param name="commandProfiles">The command profiles.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The response.</returns>
		public static string ExecuteArguments(List<CommandProfile> commandProfiles, string[] args)
		{
			if (args.Length <= 2)
			{
				return "Missing Arguments";
			}

			var domain = args[0];
			var command = args[1];

			switch (domain.ToLower())
			{
				case "tile": return ExecuteTileArguments(commandProfiles, args, command);
			}

			return "Invalid Domain";
		}

		private static string ExecuteTileArguments(List<CommandProfile> commandProfiles, string[] args, string command)
		{
			var tileProfile = commandProfiles.FirstOrDefault(e => e.Domain == "tile");

			if (null == tileProfile)
			{
				return "Domain Not Found";
			}

			if (false == tileProfile.Parameters.TryGetValue(command, out var profileArguments))
			{
				return "Command Not Found";
			}

			if (profileArguments.Count + 2 != args.Length)
			{
				return "Invalid Number of Arguments";
			}

			switch (command.ToLower())
			{
				case "add":
					int spritesheetX = 0;
					int spritesheetY = 0;
					var tileModel = new TileModel
					{
						Sprite = new SpriteModel()
					};

					foreach (var argument in profileArguments)
					{
						object argValue;

						try
						{
							var argString = args[argument.ArgumentOrder + 2];
							var argumentType = argument.ArgumentType;
							argValue = Convert.ChangeType(argString, argumentType);
						}
						catch 
						{
							return $"Error Parsing {argument.ArgumentName}";
						}

						switch (argument.ArgumentName)
						{
							case "layer":

								break;
							case "col":
								tileModel.Column = (int)argValue;
								break;
							case "row":
								tileModel.Row = (int)argValue;
								break;
							case "spritesheet":
								tileModel.Sprite.SpritesheetName = (string)argValue;
								break;
							case "spritesheet_x":
								spritesheetX = (int)argValue;
								break;
							case "spritesheet_y":
								spritesheetY = (int)argValue;
								break;
						}
					}

					tileModel.Sprite.SpritesheetBox = new Rectangle
					{
						X = spritesheetX,
						Y = spritesheetY,
						Width = TileConstants.TILE_SIZE,
						Height = TileConstants.TILE_SIZE
					};

					tileModel.Area = new SimpleAreaModel
					{
						Width = TileConstants.TILE_SIZE,
						Height = TileConstants.TILE_SIZE,
						Position = new PositionModel
						{
							X = tileModel.Column * TileConstants.TILE_SIZE,
							Y = tileModel.Row * TileConstants.TILE_SIZE
						},
						CollisionTypes = Array.Empty<string>()
					};

					return "Tile Added";
			}

			return "Invalid Command";
		}
	}
}
