Settings.WindowTitle = "My SadConsole Game";

Game.Configuration gameStartup = new Game.Configuration()
    .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<Void_sc10.Scenes.RootScene>()
    .ConfigureFonts((f) => f.UseCustomFont("Resources/Font/Cheepicus12.font"))
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();
