using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumperLibrary;

public sealed class Windowbox
{
    private readonly Color _clearColor;
    private readonly Game _game;

    private bool _initilized;
    
    // Black bars (letterboxes and pillarboxes).
    private Rectangle _renderScaleRectangle;

    // Graphical buffer used to draw graphics
    // elements until we're ready to send it to the screen.
    // While we draw on the buffer, nothing appears on the screen
    // until we decide to draw the buffer to the screen.
    private RenderTarget2D _renderTarget;

    public Windowbox(Game game, int designedResolutionWidth, int designedResolutionHeight)
    {
        _game = game;
        DesignedResolutionWidth = designedResolutionWidth;
        DesignedResolutionHeight = designedResolutionHeight;

        _clearColor = Color.Black;

        game.Window.ClientSizeChanged += (_, _) => SetDesignResolution();
    }

    private GameWindow Window => _game.Window;

    private float DesignedResolutionAspectRatio =>
        DesignedResolutionWidth / (float)DesignedResolutionHeight;

    private int DesignedResolutionWidth { get; }

    private int DesignedResolutionHeight { get; }

    public void Draw(
        SpriteBatch spriteBatch,
        Action renderAction,
        /* === SpriteBatch.Begin() parameters === */
        SpriteSortMode sortMode = SpriteSortMode.Deferred,
        BlendState blendState = null,
        SamplerState samplerState = null,
        DepthStencilState depthStencilState = null,
        RasterizerState rasterizerState = null,
        Effect effect = null,
        Matrix? transformMatrix = null)
    {
        Draw(
            spriteBatch,
            renderAction,
            _clearColor,
            sortMode,
            blendState,
            samplerState,
            depthStencilState,
            rasterizerState,
            effect,
            transformMatrix);
    }

    public Point GetCorrectMousePos(MouseState mouseState)
    {
        return ((mouseState.Position - _renderScaleRectangle.Location).ToVector2() /
                (_renderScaleRectangle.Size.ToVector2() /
                 new Vector2(DesignedResolutionWidth, DesignedResolutionHeight))).ToPoint();
    }

    public Rectangle GetScaledRect()
    {
        return new Rectangle(Point.Zero,
            (_renderScaleRectangle.Size.ToVector2() / (_renderScaleRectangle.Size.ToVector2() /
                                                       new Vector2(DesignedResolutionWidth, DesignedResolutionHeight)))
            .ToPoint());
    }

    private void Draw(
        SpriteBatch spriteBatch,
        Action renderAction,
        Color clearColor,
        /* === SpriteBatch.Begin() parameters === */
        SpriteSortMode sortMode = SpriteSortMode.Deferred,
        BlendState blendState = null,
        SamplerState samplerState = null,
        DepthStencilState depthStencilState = null,
        RasterizerState rasterizerState = null,
        Effect effect = null,
        Matrix? transformMatrix = null)
    {
        if (!_initilized)
        {
            SetDesignResolution();
            _initilized = !_initilized;
        }

        // Draw on the graphics pad.
        _game.GraphicsDevice.SetRenderTarget(_renderTarget);
        _game.GraphicsDevice.Clear(Color.CornflowerBlue);

        spriteBatch.Begin(
            sortMode,
            blendState, samplerState,
            depthStencilState,
            rasterizerState,
            effect,
            transformMatrix);

        renderAction?.Invoke();

        spriteBatch.End();

        // Display the contents of the graphics buffer window-wide.
        _game.GraphicsDevice.SetRenderTarget(null);
        _game.GraphicsDevice.Clear(ClearOptions.Target, clearColor, 1.0f, 0);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, samplerState);
        spriteBatch.Draw(_renderTarget, _renderScaleRectangle, Color.White);
        spriteBatch.End();
    }
    
    // Provides black bars similar to your TV screen
    // based on actual resolution vs resolution
    // design (design resolution).
    private void SetDesignResolution()
    {
        _renderTarget = new RenderTarget2D(_game.GraphicsDevice,
            DesignedResolutionWidth, DesignedResolutionHeight,
            false,
            SurfaceFormat.Color, DepthFormat.None, 0,
            RenderTargetUsage.DiscardContents);

        _renderScaleRectangle = GetScaleRectangle();

        Rectangle GetScaleRectangle()
        {
            var variance = 0.5;
            var actualAspectRatio = Window.ClientBounds.Width / (float)Window.ClientBounds.Height;

            Rectangle scaleRectangle;

            if (actualAspectRatio <= DesignedResolutionAspectRatio)
            {
                var presentHeight = (int)(Window.ClientBounds.Width / DesignedResolutionAspectRatio + variance);
                var barHeight = (Window.ClientBounds.Height - presentHeight) / 2;
                scaleRectangle = new Rectangle(0, barHeight, Window.ClientBounds.Width, presentHeight);
            }
            else
            {
                var presentWidth = (int)(Window.ClientBounds.Height * DesignedResolutionAspectRatio + variance);
                var barWidth = (Window.ClientBounds.Width - presentWidth) / 2;
                scaleRectangle = new Rectangle(barWidth, 0, presentWidth, Window.ClientBounds.Height);
            }

            return scaleRectangle;
        }
    }
}