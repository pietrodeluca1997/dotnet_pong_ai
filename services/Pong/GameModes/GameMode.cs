using IA;
using Newtonsoft.Json;
using Pong.Actors;
using Pong.Characters;
using Pong.Core;
using Pong.HUD;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace Pong.GameModes;

public class GameMode
{
    public Player Player { get; set; }
    public Ball Ball { get; set; }
    public MainWidget Widget { get; set; }
    public NeuralNetwork NeuralNetwork { get; set; }

    private int Score;
    private int ScoreRecord;
    private int Losses;

    public GameMode()
    {
        Score = 0;
        ScoreRecord = 0;
        Losses = 0;
    }

    public void Initialize(List<GameObject> gameObjects)
    {
        NeuralNetwork = new NeuralNetwork();

        Player = (Player)gameObjects.First(gameObject => gameObject.GetType() == typeof(Player));

        Ball = (Ball)gameObjects.First(gameObject => gameObject.GetType() == typeof(Ball));

        Widget = (MainWidget)gameObjects.First(gameObject => gameObject.GetType() == typeof(MainWidget));

        Ball.Lose += OnLose;
        Ball.Score += OnScore;
        Ball.CheckCollision += Player.CheckCollision;
    }

    public void OnLose(object sender, EventArgs eventArgs)
    {
        Losses++;

        if (Score > ScoreRecord)
        {
            ScoreRecord = Score;

            NeuralNetwork.bestInputWeights = NeuralNetwork.inputWeights;
            NeuralNetwork.bestHiddenWeights = NeuralNetwork.hiddenWeights;
        }

        if (NeuralNetwork.currentGenome >= NeuralNetwork.MAX_GENOMES)
        {
            dynamic fileType = new ExpandoObject();
            fileType.input_weight = NeuralNetwork.bestInputWeights;
            fileType.hidden_weight = NeuralNetwork.bestHiddenWeights;

            string json = JsonConvert.SerializeObject(fileType);

            File.WriteAllText(@"best_genome.json", json);
        }

        if (NeuralNetwork.currentGenome >= NeuralNetwork.MAX_GENOMES && ScoreRecord > 0)
        {
            NeuralNetwork.inputWeights = NeuralNetwork.bestInputWeights;
            NeuralNetwork.hiddenWeights = NeuralNetwork.bestHiddenWeights;

            NeuralNetwork.MakeMutation();
        }
        else
        {
            NeuralNetwork.GenerateWeights();
        }

        Score = 0;
        Widget.Update(Score, ScoreRecord, Losses);
    }

    public void OnScore(object sender, EventArgs eventArgs)
    {
        Score++;

        Widget.Update(Score, ScoreRecord, Losses);
    }
}
