using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pong.Core;

public class GameObject
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 Direction { get; set; }
    public Texture2D Sprite { get; set; }
    public Rectangle Body { get; set; }
    public Rectangle HitBox { get; set; }

    public virtual void OnBeginPlay(object sender, EventArgs args)
    {

    }

    public virtual void OnEventTick(object sender, EventArgs args)
    {

    }

    public virtual void OnDraw(object sender, EventArgs args)
    {

    }
}
