using SadConsole.Configuration;

Settings.WindowTitle = "GlyphViewer";

Builder gameStartup = new Builder()
    .SetScreenSize(GameSettings.GAME_WIDTH, GameSettings.GAME_HEIGHT)
    .SetStartingScreen<GlyphViewer.Scenes.GlyphScene>()
    .IsStartingScreenFocused(true)
    .ConfigureFonts((config, game) => config.UseCustomFont("Resources/Font/Cheepicus12.font"))
    ;

Game.Create(gameStartup);
Game.Instance.Run();
Game.Instance.Dispose();