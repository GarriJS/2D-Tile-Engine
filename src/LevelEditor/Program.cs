
using LevelEditor;
using Common.Core.Initialization;

using var game = new Engine.Game1();
var loadingInstructions = GameContainer.GetLoadingInstructions(game._graphics);
game.SetLoadingInstructions(loadingInstructions);
ServiceInitializer.InitializeServices(game);

GameContainer.Game = game;
game.Run();
