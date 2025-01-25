using Common.DiskModels.Common.Tiling;
using Engine.DiskModels.Engine.Drawing;
using Engine.DiskModels.Engine.Physics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LevelEditor.Core.Initialization
{
	/// <summary>
	/// Represents a level editor initializer.
	/// </summary>
	public static class LevelEditorInitializer
	{
		public static IList<object> GetInitialDiscModels()
		{
			return 
			[
				new TileModel
				{
					Row = 1,
					Column = 1,
					Area = new SimpleAreaModel
					{ 
						Height = 64,
						Width = 64,
					},
					Sprite = new SpriteModel
					{ 
						SpritesheetBox = new Rectangle(0, 0, 64, 64),
						TextureName = "grass_tileset"
					}
				}
			];	
		}
	}
}
