using Engine.Drawing.Models;
using System.Collections.Generic;

namespace Engine.Terminal.Models
{
	/// <summary>
	/// Represents a console.
	/// </summary>
	public class Console
	{
		/// <summary>
		/// Gets or sets the selected recommended argument index.
		/// </summary>
		public int SelectedRecommendedArgumentIndex { get; set; }

		/// <summary>
		/// Gets or sets the console background.
		/// </summary>
		public Image ConsoleBackground { get; set; }

		/// <summary>
		/// Gets or sets the active console line background.
		/// </summary>
		public Image ActiveConsoleLineBackground { get; set; }

		/// <summary>
		/// Gets or sets the recommended arguments background.
		/// </summary>
		public Image RecommendedArgumentsBackground { get; set; }

		/// <summary>
		/// Gets or sets the selected recommended argument background.
		/// </summary>
		public Image SelectedRecommendedArgumentBackground { get; set; }

		/// <summary>
		/// Gets or sets the selected recommended argument selector.
		/// </summary>
		public DrawableText SelectedRecommendedArgumentSelector { get; set; }

		/// <summary>
		/// Gets or sets the active console line.
		/// </summary>
		public ConsoleLine ActiveConsoleLine { get; set; }

		/// <summary>
		/// Gets or sets the cursor.
		/// </summary>
		public Animation Cursor { get; set; }

		/// <summary>
		/// Gets or sets the console lines.
		/// </summary>
		public List<ConsoleLine> ConsoleLines { get; set; }

		/// <summary>
		/// Gets or sets the recommended arguments.
		/// </summary>
		public List<DrawableText> RecommendedArguments { get; set; }
	}
}
