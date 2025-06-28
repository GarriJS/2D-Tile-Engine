
using LevelEditor;
using LevelEditor.Core.Initialization;

// Create the game object
using var game = new Engine.Engine();

// Debug
game.LaunchInDebugMode = true;
game.DebugSpriteFontName = "Monolight";

// Set the loading instructions
var loadingInstructions = GameContainer.GetLoadingInstructions(game._graphics);
game.SetLoadingInstructions(loadingInstructions);

// Add the external service providers
game.AddExternalServiceProvider(LevelEditor.Core.Initialization.ServiceExporter.GetServiceContractPairs);
game.AddExternalServiceProvider(Common.Core.Initialization.ServiceExporter.GetServiceContractPairs);

// Add the external model processor mappings
game.AddModelProcessingMapProvider(Common.DiskModels.ModelProcessorExporter.GetModelProcessingMappings);

// Add the initial models
game.AddInitialModelsProvider(LevelEditorInitializer.GetInitialUiModels);

// Add the user interface event processors
game.AddFunctionProvider(LevelEditorInitializer.GetInitialHoverEventProcessors);
game.AddFunctionProvider(LevelEditorInitializer.GetInitialPressEventProcessors);
game.AddFunctionProvider(LevelEditorInitializer.GetInitialClickEventProcessors);

// Add the initial user interface models
//game.AddInitialUserInterfaceProvider(LevelEditorInitializer.GetInitialUiModels);

// Run the game
GameContainer.Game = game;
game.Run();
