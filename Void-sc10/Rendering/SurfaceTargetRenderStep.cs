using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using XnaRectangle = Microsoft.Xna.Framework.Rectangle;

namespace SadConsole.Renderers;

/// <summary>
/// Thanks Thraka
/// </summary>
internal class SurfaceTargetRenderStep : IRenderStep, IRenderStepTexture
{
    private Host.GameTexture? _cachedTexture;
    private IScreenSurface? _surface;

    public RenderTarget2D? BackingTexture { get; private set; }
    public ITexture CachedTexture => _cachedTexture!;
    public string Name => Constants.RenderStepNames.Surface;
    public uint SortOrder { get; set; } = Constants.RenderStepSortValues.Surface;
    public void SetData(object data)
    {
        if (data is IScreenSurface surface)
            _surface = surface;
        else
            throw new ArgumentException($"{nameof(SurfaceTargetRenderStep)} must have a {nameof(IScreenSurface)} passed to the {nameof(SetData)} method", nameof(data));
    }
    
    public void Reset()
    {
        BackingTexture?.Dispose();
        BackingTexture = null;
        _cachedTexture?.Dispose();
        _cachedTexture = null;
    }

    public bool Refresh(IRenderer renderer, IScreenSurface screenObject, bool backingTextureChanged, bool isForced)
    {
        // screenObject won't be used, instead we'll draw the _surface variable
        bool result = false;

        // Update texture if something is out of size.
        if (backingTextureChanged || BackingTexture == null || _surface!.AbsoluteArea.Width != BackingTexture.Width || _surface.AbsoluteArea.Height != BackingTexture.Height)
        {
            BackingTexture?.Dispose();
            BackingTexture = new RenderTarget2D(Host.Global.GraphicsDevice, _surface!.AbsoluteArea.Width, _surface.AbsoluteArea.Height, false, Host.Global.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
            _cachedTexture?.Dispose();
            _cachedTexture = new Host.GameTexture(BackingTexture);
            result = true;
        }

        var monoRenderer = (IRendererMonoGame)renderer;

        // Redraw is needed
        if (result || _surface.IsDirty || isForced)
        {
            Host.Global.GraphicsDevice.SetRenderTarget(BackingTexture);
            Host.Global.GraphicsDevice.Clear(Color.Transparent);
            Host.Global.SharedSpriteBatch.Begin(SpriteSortMode.Deferred, monoRenderer.MonoGameBlendState, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone);

            IFont font = _surface.Font;
            Texture2D fontImage = ((Host.GameTexture)font.Image).Texture;
            ColoredGlyphBase cell;

            if (_surface.Surface.DefaultBackground.A != 0)
                Host.Global.SharedSpriteBatch.Draw(fontImage, new XnaRectangle(0, 0, BackingTexture.Width, BackingTexture.Height), font.SolidGlyphRectangle.ToMonoRectangle(), _surface.Surface.DefaultBackground.ToMonoColor(), 0f, Vector2.Zero, SpriteEffects.None, 0.2f);

            int rectIndex = 0;

            for (int y = 0; y < _surface.Surface.View.Height; y++)
            {
                int i = ((y + _surface.Surface.ViewPosition.Y) * _surface.Surface.Width) + _surface.Surface.ViewPosition.X;

                for (int x = 0; x < _surface.Surface.View.Width; x++)
                {
                    cell = _surface.Surface[i];
                    cell.IsDirty = false;

                    if (cell.IsVisible)
                    {
                        if (cell.Background != SadRogue.Primitives.Color.Transparent && cell.Background != _surface.Surface.DefaultBackground)
                            Host.Global.SharedSpriteBatch.Draw(fontImage, monoRenderer.CachedRenderRects[rectIndex], font.SolidGlyphRectangle.ToMonoRectangle(), cell.Background.ToMonoColor(), 0f, Vector2.Zero, SpriteEffects.None, 0.3f);

                        if (cell.Glyph != 0 && cell.Foreground != SadRogue.Primitives.Color.Transparent && cell.Foreground != cell.Background)
                            Host.Global.SharedSpriteBatch.Draw(fontImage, monoRenderer.CachedRenderRects[rectIndex], font.GetGlyphSourceRectangle(cell.Glyph).ToMonoRectangle(), cell.Foreground.ToMonoColor(), 0f, Vector2.Zero, cell.Mirror.ToMonoGame(), 0.4f);

                        if (cell.Decorators != null)
                            for (int d = 0; d < cell.Decorators.Count; d++)
                                if (cell.Decorators[d].Color != SadRogue.Primitives.Color.Transparent)
                                    Host.Global.SharedSpriteBatch.Draw(fontImage, monoRenderer.CachedRenderRects[rectIndex], font.GetGlyphSourceRectangle(cell.Decorators[d].Glyph).ToMonoRectangle(), cell.Decorators[d].Color.ToMonoColor(), 0f, Vector2.Zero, cell.Decorators[d].Mirror.ToMonoGame(), 0.5f);
                    }

                    i++;
                    rectIndex++;
                }
            }

            Host.Global.SharedSpriteBatch.End();
            Host.Global.GraphicsDevice.SetRenderTarget(null);

            result = true;
            _surface.IsDirty = false;
        }

        return result;
    }
    
    public void Composing(IRenderer renderer, IScreenSurface screenObject)
    {
        Host.Global.SharedSpriteBatch.Draw(BackingTexture, Vector2.Zero, Color.White);
    }

    public void Render(IRenderer renderer, IScreenSurface screenObject) { }

    protected void Dispose(bool disposing)
    {
        Reset();
        _surface = null;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~SurfaceTargetRenderStep() =>
        Dispose(false);
}
