﻿using Engine.DiskModels.Drawing.Contracts;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class CompositeImageModel : IAmAImageModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get; set; }

		[JsonPropertyName("textureRegions")]
		public TextureRegionModel[][] TextureRegions { get; set; }
	}
}
