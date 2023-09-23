using IA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core;
using Pong.Helpers;
using Pong.Levels;
using System;

namespace Pong;

public class EntryPong : Game
{
    private const string CONTENT_PATH = "Content";

    private readonly Level _startLevel;

    private readonly NeuralNetwork neuralNetwork = new();

    public EntryPong()
    {
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1 / 60.0f);

        GraphicsDeviceManager graphicsDeviceManager = new(this)
        {
            PreferredBackBufferWidth = 800,
            PreferredBackBufferHeight = 600
        };

        GraphicsHelper.InitializeGraphics(graphicsDeviceManager);

        Content.RootDirectory = CONTENT_PATH;

        IsMouseVisible = true;

        _startLevel = new MainLevel();
    }

    protected override void Initialize()
    {
        base.Initialize();

        _startLevel.Initialize();

        _startLevel.HandleBeginPlay();
    }

    protected override void LoadContent()
    {
        GraphicsHelper.InitializeSpriteBatch();
        GraphicsHelper.InitializeSpriteFont(Content.Load<SpriteFont>("Fonts/DefaultFont"));
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        base.Update(gameTime);

        _startLevel.HandleUpdate(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsHelper.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Gray);

        GraphicsHelper.SpriteBatch.Begin();

        _startLevel.HandleDraw(gameTime);

        GraphicsHelper.SpriteBatch.End();

        base.Draw(gameTime);
    }
}