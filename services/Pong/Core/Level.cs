using Microsoft.Xna.Framework;
using Pong.GameModes;
using Pong.Helpers;
using System;
using System.Collections.Generic;

namespace Pong.Core;

public class Level
{
    protected event EventHandler BeginPlay;
    protected event EventHandler EventTick;
    protected event EventHandler EventDraw;
    protected GameMode GameMode { get; set; }

    protected List<GameObject> gameObjects = new();

    public virtual void Initialize()
    {
        SubscribeGameObjects();

        GameMode = new GameMode();
        GameMode.Initialize(gameObjects);
    }

    public void HandleBeginPlay()
    {
        BeginPlay?.Invoke(this, EventArgs.Empty);
    }

    public void HandleUpdate(GameTime gameTime)
    {
        double AIResult = GameMode.NeuralNetwork.GetOutput(GameMode.Ball.Body.X, GameMode.Player.Body.X);

        if (AIResult >= 0.6f) GameMode.Player.GoRight();
        if (AIResult <= 0.4) GameMode.Player.GoLeft();

        EventTick?.Invoke(this, EventArgs.Empty);
    }

    public void HandleDraw(GameTime gameTime)
    {
        EventDraw?.Invoke(this, EventArgs.Empty);
    }

    protected void SubscribeGameObjects()
    {
        foreach (GameObject gameObject in AssemblyHelper.ScanGameObjects())
        {
            if (AssemblyHelper.ChildClassOverrides(typeof(GameObject), gameObject.GetType(), nameof(GameObject.OnBeginPlay)))
            {
                BeginPlay += gameObject.OnBeginPlay;
            }

            if (AssemblyHelper.ChildClassOverrides(typeof(GameObject), gameObject.GetType(), nameof(GameObject.OnEventTick)))
            {
                EventTick += gameObject.OnEventTick;
            }

            if (AssemblyHelper.ChildClassOverrides(typeof(GameObject), gameObject.GetType(), nameof(GameObject.OnDraw)))
            {
                EventDraw += gameObject.OnDraw;
            }

            gameObjects.Add(gameObject);
        }
    }
}
