using SadConsole.UI;
using SadConsole.UI.Controls;
using VEngine.Scenes;

namespace GlyphViewer.Scenes
{
    internal class GlyphScene : Scene
    {
        private ScreenSurface glyphDisplay;
        private ScreenSurface allGlyphDisplay;
        private ControlsConsole controls;
        private char selectedChar = (char)0;
        public GlyphScene()
        {
            Surface.Print(0, 0, "test");

            glyphDisplay = new(1, 1);
            glyphDisplay.FontSize = (256, 256);
            glyphDisplay.Position = (2, 0);
            Children.Add(glyphDisplay);

            controls = new(25, 25);
            controls.Position = (0, 45);

            Button right = new("->")
            {
                Position = (0, 4)
            };
            right.Click += (s, e) =>
            {
                selectedChar++;
            };

            Button left = new("<-")
            {
                Position = (4, 4)
            };
            left.Click += (s, e) =>
            {
                selectedChar--;
            };

            controls.Controls.Add(right); 
            controls.Controls.Add(left);

            Children.Add(controls);

            allGlyphDisplay = new(16, 16);
            int charcounter = 0;
            for(int y = 0; y < 16; y++)
            {
                for(int x = 0; x < 16; x++)
                {
                    allGlyphDisplay.SetGlyph(x, y, charcounter++);
                }
            }
            allGlyphDisplay.Position = (0, 4);
            allGlyphDisplay.FontSize = (24, 24);
            allGlyphDisplay.MouseButtonClicked += MouseButtonClicked;
            Children.Add(allGlyphDisplay);
        }

        private void MouseButtonClicked(object? sender, SadConsole.Input.MouseScreenObjectState e)
        {
            Point index = e.SurfaceCellPosition;
            selectedChar = (char)((index.Y * 16) + index.X);
        }

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);

            glyphDisplay.SetGlyph(0, 0, selectedChar);
            Surface.Clear();
            Surface.Print(0, 2, $"char: {selectedChar} int: {(int)selectedChar}");
        }
    }
}
