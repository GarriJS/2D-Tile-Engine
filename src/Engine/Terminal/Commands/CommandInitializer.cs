using Engine.Terminal.Commands.Models;
using System.Collections.Generic;

namespace Engine.Terminal.Commands
{
	/// <summary>
	/// Provides methods for initializing commands.
	/// </summary>
	public static class CommandInitializer
	{
		/// <summary>
		/// Gets the command profiles.
		/// </summary>
		/// <returns></returns>
		public static List<CommandProfile> GetCommandProfiles()
		{
			var commandProfiles = new List<CommandProfile>()
			{
				GetTileCommandProfile()
			};

			return commandProfiles;
		}

		/// <summary>
		/// Gets the tile command profile.
		/// </summary>
		/// <returns></returns>
		private static CommandProfile GetTileCommandProfile()
		{
			var tileCommandProfile = new CommandProfile
			{
				Domain = "tile",
				Commands = new string[]
				{
					"add"
				},
				Parameters = new Dictionary<string, List<CommandArgumentDefinition>>()
			};

			tileCommandProfile.Parameters.Add("add", new List<CommandArgumentDefinition>
			{
				new()
				{
					ArgumentOrder = 0,
					ArgumentName = "layer",
					ArgumentType = typeof(int),
				},
				new()
				{
					ArgumentOrder = 1,
					ArgumentName = "col",
					ArgumentType = typeof(int),
				},
				new()
				{
					ArgumentOrder = 2,
					ArgumentName = "row",
					ArgumentType = typeof(int),
				},
				new()
				{
					ArgumentOrder = 3,
					ArgumentName = "spritesheet",
					ArgumentType = typeof(string),
				},
				new()
				{
					ArgumentOrder = 4,
					ArgumentName = "spritesheet_x",
					ArgumentType = typeof(int),
				},
				new()
				{
					ArgumentOrder = 5,
					ArgumentName = "spritesheet_y",
					ArgumentType = typeof(int),
				},
				new()
				{
					ArgumentOrder = 6,
					ArgumentName = "collision types",
					ArgumentType = typeof(int[]),
				}
			});

			tileCommandProfile.Parameters.Add("addTest", new List<CommandArgumentDefinition>
			{
				new()
				{
					ArgumentOrder = 0,
					ArgumentName = "layer",
					ArgumentType = typeof(int),
				},
				new()
				{
					ArgumentOrder = 1,
					ArgumentName = "col",
					ArgumentType = typeof(int),
				},
				new()
				{
					ArgumentOrder = 2,
					ArgumentName = "row",
					ArgumentType = typeof(int),
				},
				new()
				{
					ArgumentOrder = 3,
					ArgumentName = "spritesheet",
					ArgumentType = typeof(string),
				},
				new()
				{
					ArgumentOrder = 4,
					ArgumentName = "spritesheet_x",
					ArgumentType = typeof(int),
				},
				new()
				{
					ArgumentOrder = 5,
					ArgumentName = "spritesheet_y",
					ArgumentType = typeof(int),
				},
				new()
				{
					ArgumentOrder = 6,
					ArgumentName = "collision types",
					ArgumentType = typeof(int[]),
				}
			});

			return tileCommandProfile;
		}
	}
}
