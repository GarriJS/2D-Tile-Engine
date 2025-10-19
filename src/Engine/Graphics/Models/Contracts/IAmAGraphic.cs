using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Drawing.Contracts;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represent something that is a graphic.
	/// </summary>
	public interface IAmAGraphic : IAmSubDrawable, ICanBeSerialized<IAmAGraphicModel>, IDisposable
	{
		/// <summary>
		/// Gets or sets the texture name.
		/// </summary>
		public string TextureName { get; }

		/// <summary>
		/// Gets or sets the texture box.
		/// </summary>
		public Rectangle TextureBox { get; }

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		public Texture2D Texture { get; }
	}
}
