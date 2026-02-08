using Microsoft.Xna.Framework;
using System.Collections.Generic;
using UserInterfaceTests.UiZoneJustificationTests;

namespace UserInterfaceTests
{
	static public class TestInitializer
	{
		/// <summary>
		/// Gets the initial user interface models.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The user interface models.</returns>
		static public IList<object> GetInitialUiModels(GameServiceContainer gameServices)
		{
			var testModels = CornersTest.GetCornersTest1();

			return [testModels];
		}
	}
}
