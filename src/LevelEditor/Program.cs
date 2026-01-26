using Engine.DiskModels;
using LevelEditor;
using LevelEditor.Controls.Contexts;
using LevelEditor.Core.Initialization;
using System.Reflection;

// Create the game object
using var game = new Engine.Engine();

// Debug
game.InDebugMode = true;
game.DebugSpriteFontName = "Monolight";

// Registers the assemblies
DiskModelTypeResolver.RegisterAssembly(Assembly.GetAssembly(typeof(BaseDiskModel)));
DiskModelTypeResolver.RegisterAssembly(Assembly.GetAssembly(typeof(Common.DiskModels.ModelProcessorExporter)));

// Set the loading instructions
var loadingInstructions = GameContainer.GetLoadingInstructions(game._graphics);
game.SetLoadingInstructions(loadingInstructions);
game.SetInitialControlContextType<LevelEditorStartControlContext>();

// Add the external service providers
game.AddExternalServiceProvider(ServiceExporter.GetServiceContractPairs);
game.AddExternalServiceProvider(Common.Core.Initialization.ServiceExporter.GetServiceContractPairs);

// Add the external model processor mappings
game.AddModelProcessingMapProvider(Common.DiskModels.ModelProcessorExporter.GetModelProcessingMappings);

// Add the initial models
game.AddInitialModelsProvider(LevelEditorInitializer.GetInitialUiModels);

// Add the function providers
game.AddFunctionProvider(LevelEditorInitializer.GetFunctionProviders);
game.AddFunctionProvider(Common.Core.Initialization.CommonInitializer.GetFunctionProviders);

// Run the game
GameContainer.Game = game;
game.Run();
