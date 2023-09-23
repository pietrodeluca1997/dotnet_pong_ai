using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Core;
using Pong.Helpers;
using System;

namespace Pong.Characters;

public class Player : GameObject
{
    private int rectangleWidth = 180;
    private int rectangleHeight = 30;
    private int rectangleY;

    public Player()
    {
        int screenWidth = GraphicsHelper.GraphicsDeviceManager.GraphicsDevice.Viewport.Width;
        int screenHeight = GraphicsHelper.GraphicsDeviceManager.GraphicsDevice.Viewport.Height;


        int rectangleX = (screenWidth - rectangleWidth) / 2;
        rectangleY = screenHeight - rectangleHeight;

        Body = new(rectangleX, rectangleY, rectangleWidth, rectangleHeight);

        Position = new Vector2(rectangleX, rectangleY);
        Velocity = new Vector2(15, 0);
        Sprite = new Texture2D(GraphicsHelper.GraphicsDeviceManager.GraphicsDevice, 1, 1);
        Sprite.SetData(new Color[] { Color.White });
    }

    public void GoLeft()
    {
        Position -= Velocity;
    }

    public void GoRight()
    {
        Position += Velocity;
    }

    public override void OnEventTick(object sender, EventArgs args)
    {
        base.OnEventTick(sender, args);

        KeyboardState keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
        {
            Position -= Velocity;
        }

        if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
        {
            Position += Velocity;
        }

        Position = new Vector2(MathHelper.Clamp(Position.X, 0, GraphicsHelper.GraphicsDeviceManager.GraphicsDevice.Viewport.Width - rectangleWidth), rectangleY);

        Body = new((int)Position.X, (int)Position.Y, rectangleWidth, rectangleHeight);
        HitBox = Body;
    }

    public override void OnBeginPlay(object sender, EventArgs args)
    {

    }

    public override void OnDraw(object sender, EventArgs args)
    {
        GraphicsHelper.SpriteBatch.Draw(Sprite, Body, Color.White);
    }

    public bool CheckCollision(GameObject otherObject)
    {
        return otherObject.HitBox.Intersects(HitBox);
    }
}
