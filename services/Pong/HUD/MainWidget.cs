using Microsoft.Xna.Framework;
using Pong.Core;
using Pong.Helpers;
using System;

namespace Pong.HUD;

public class MainWidget : GameObject
{
    Vector2 posicaoHUD = new(10, 10);
    Color corTexto = Color.White;

    private int Score;
    private int ScoreRecord;
    private int Losses;

    public override void OnBeginPlay(object sender, EventArgs args)
    {
        base.OnBeginPlay(sender, args);
    }

    public override void OnDraw(object sender, EventArgs args)
    {
        base.OnDraw(sender, args);

        string textoHUD = $"Score: {Score} | ScoreRecord: {ScoreRecord} | Losses: {Losses}";

        GraphicsHelper.SpriteBatch.DrawString(GraphicsHelper.SpriteFont, textoHUD, posicaoHUD, corTexto);
    }

    public void Update(int score, int scoreRecord, int losses)
    {
        Score = score;
        ScoreRecord = scoreRecord;
        Losses = losses;
    }
}
