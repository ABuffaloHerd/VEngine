using VEngine;
using SadConsole.Configuration;
using VEngine.Scenes.Combat;
using VEngine.Scenes;

Settings.WindowTitle = "VEngine-sc10";

Builder startup = new Builder()
    .SetScreenSize(128, 72)
    .SetStartingScreen<CombatScene>()
    .IsStartingScreenFocused(true)
    .ConfigureFonts((config, game) => config.UseCustomFont("Resources/Font/Cheepicus12.font"))
    .AddFrameUpdateEvent(KeyboardHandler.HandleKeyboard)
    ;

Game.Create(startup);
Game.Instance.Run();
Game.Instance.Dispose();