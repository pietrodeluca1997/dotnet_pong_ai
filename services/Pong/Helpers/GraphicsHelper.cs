using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Helpers;

public static class GraphicsHelper
{
    public static GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    public static SpriteFont SpriteFont { get; private set; }

    public static void InitializeGraphics(GraphicsDeviceManager graphicsDeviceManager)
    {
        GraphicsDeviceManager = graphicsDeviceManager;
    }

    public static void InitializeSpriteBatch()
    {
        SpriteBatch = new SpriteBatch(GraphicsDeviceManager.GraphicsDevice);
    }

    public static void InitializeSpriteFont(SpriteFont spriteFont)
    {
        SpriteFont = spriteFont;
    }
}
