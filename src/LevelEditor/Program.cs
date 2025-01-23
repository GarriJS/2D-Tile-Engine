
using LevelEditor;

// Create the game object
using var game = new Engine.Game1();

// Set the loading instructions
var loadingInstructions = GameContainer.GetLoadingInstructions(game._graphics);
game.SetLoadingInstructions(loadingInstructions);

// Add the external service providers
game.AddExternalServiceProvider(LevelEditor.Core.Initialization.ServiceExporter.GetServiceContractPairs);
game.AddExternalServiceProvider(Common.Core.Initialization.ServiceExporter.GetServiceContractPairs);

// Add the external model type mappings
game.AddExternalModelTypeMapProvider(Common.DiskModels.ModelMappingsExporter.GetModelTypeMappings);

// Add the external model processor mappings
game.AddModelProcessingMapProvider(Common.DiskModels.ModelMappingsExporter.GetModelProcessingMappings);

// Run the game
GameContainer.Game = game;
game.Run();
