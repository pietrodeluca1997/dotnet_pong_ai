using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Core;
using Pong.Helpers;
using System;
using System.Collections.Generic;

namespace Pong.Actors;

public class Ball : GameObject
{
    private static readonly List<float> RandomDirectionOptions = new() { 1.0f, -1.0f };

    public event EventHandler Lose;
    public event EventHandler Score;

    public event CollisionHelper.Check CheckCollision;

    private readonly int ballRadius = 20;
    private bool isCalculatingCollision = false;
    private bool isReseting = false;

    public Ball()
    {
        Reset();

        Sprite = new Texture2D(GraphicsHelper.GraphicsDeviceManager.GraphicsDevice, 1, 1);
        Sprite.SetData(new Color[] { Color.White });
    }

    public override void OnEventTick(object sender, EventArgs args)
    {
        if (isCalculatingCollision || isReseting) return;

        base.OnEventTick(sender, args);

        Position += new Vector2(Velocity.X * Direction.X, Velocity.Y * Direction.Y);

        Body = new((int)Position.X - ballRadius, (int)Position.Y - ballRadius, ballRadius * 2, ballRadius * 2);
        HitBox = Body;

        if (Position.Y + ballRadius > GraphicsHelper.GraphicsDeviceManager.GraphicsDevice.Viewport.Height)
        {
            Lose?.Invoke(this, EventArgs.Empty);

            Reset();
        }

        if (Position.Y - ballRadius < 0)
        {
            Direction = new Vector2(Direction.X, Direction.Y * -1);
        }

        if (Position.X - ballRadius < 0 || Position.X + ballRadius > GraphicsHelper.GraphicsDeviceManager.GraphicsDevice.Viewport.Width)
        {
            Direction = new Vector2(Direction.X * -1, Direction.Y);
        }

        if (CheckCollision.Invoke(this))
        {
            Direction = new Vector2(Direction.X, Direction.Y * -1);
            isCalculatingCollision = true;
            Score?.Invoke(this, EventArgs.Empty);

            RandomVelocity();

            isCalculatingCollision = false;
            return;
        }
    }

    public override void OnBeginPlay(object sender, EventArgs args)
    {

    }

    public override void OnDraw(object sender, EventArgs args)
    {
        GraphicsHelper.SpriteBatch.Draw(Sprite, Body, Color.White);
    }

    public void Reset()
    {
        isReseting = true;
        RandomVelocity();
        RandomDirection();

        int rectangleX = (GraphicsHelper.GraphicsDeviceManager.GraphicsDevice.Viewport.Width - ballRadius) / 2;
        int rectangleY = ((GraphicsHelper.GraphicsDeviceManager.GraphicsDevice.Viewport.Height - ballRadius) / 2) - 100;

        Body = new(rectangleX, rectangleY, ballRadius, ballRadius);
        HitBox = new(rectangleX, rectangleY, ballRadius, ballRadius);

        Position = new Vector2(rectangleX, rectangleY);

        isReseting = false;
    }

    private void RandomVelocity()
    {
        Random random = new();

        int randomVelocity = random.Next(10, 18);
        Velocity = new Vector2(randomVelocity, randomVelocity);
    }

    private void RandomDirection()
    {
        Random random = new();

        int randomDirectionX = random.Next(RandomDirectionOptions.Count);
        int randomDirectionY = random.Next(RandomDirectionOptions.Count);

        Direction = new Vector2(RandomDirectionOptions[randomDirectionX], RandomDirectionOptions[randomDirectionY]);
    }
}
