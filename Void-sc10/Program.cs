using VEngine;

Settings.WindowTitle = "VEngine-sc10";

Game.Configuration gameStartup = new Game.Configuration()
    .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<VEngine.Scenes.TitleScene>()
    .ConfigureFonts((f) => f.UseCustomFont("Resources/Font/Cheepicus12.font"))
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();
