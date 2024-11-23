
using LevelEditor;

using var game = new Engine.Game1();
var loadingInstructions = GameContainer.GetLoadingInstructions(game._graphics);
game.SetLoadingInstructions(loadingInstructions);

GameContainer.Game = game;
game.Run();
