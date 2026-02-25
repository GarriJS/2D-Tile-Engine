using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Engine.Core.Fonts.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a simple text with margin service.
	/// </summary>
	/// <remarks>
	/// Initializes the simple text with margin service.
	/// </remarks>
	/// <param name="gameServices">The game service.</param>
	sealed public class GraphicalTextWithMarginService(GameServiceContainer gameServices) : ISimpleTextWithMarginService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the simple text with margin from the model.
		/// </summary>
		/// <param name="model">The simple text with margin.</param>
		/// <returns>The simple text with margin.</returns>
		public SimpleTextWithMargin GetSimpleTextWithMarginFromModel(SinmpleTextWithMarginModel model)
		{
			if (model is null)
				return null;

			var fontService = this._gameServices.GetService<IFontService>();
			var uiMarginService = this._gameServices.GetService<IUiMarginService>();
			var font = fontService.GetSpriteFont(model.FontName);
			font ??= fontService.DebugSpriteFont;
			var margin = uiMarginService.GetUiMarginFromModel(model.Margin);
			var result = new SimpleTextWithMargin
			{
				MaxLineWidth = null,
				FontName = model.FontName,
				Text = model.Text,
				TextColor = model.TextColor,
				Font = font,
				Margin = margin
			};

			return result;
		}
	}
}
