using Newtonsoft.Json;
using System.Text;

namespace IA;

public class NeuralNetwork
{
    const int INPUT_SIZE = 2;
    const int HIDDEN_LAYER_SIZE = 3;

    public const int MAX_GENOMES = 2000;

    public int currentGenome = 0;

    private bool useSpecificGenome = false;

    public List<List<double>> inputWeights;
    public List<double> hiddenWeights;

    public List<List<double>> bestInputWeights;
    public List<double> bestHiddenWeights;

    public NeuralNetwork()
    {
        inputWeights = new();
        hiddenWeights = new();
        bestInputWeights = new();
        bestHiddenWeights = new();

        GenerateWeights();
    }

    public void GenerateWeights()
    {
        if (!useSpecificGenome)
        {
            for (int i = 0; i < INPUT_SIZE; i++)
            {
                List<double> matrixRow = new();

                for (int j = 0; j < HIDDEN_LAYER_SIZE; j++)
                {
                    matrixRow.Add(RandomNumber(-1, 1));
                }

                inputWeights.Add(matrixRow);
            }

            hiddenWeights = new List<double>();

            for (int i = 0; i < HIDDEN_LAYER_SIZE; i++)
            {
                hiddenWeights.Add(RandomNumber(-1, 1));
            }

            currentGenome++;
        }
        else
        {
            string path = @"pong_genome.json";

            string json = File.ReadAllText(path, Encoding.UTF8);

            var genomeObject = new { input_weight = new List<List<double>>(), hidden_weight = new List<double>() };

            var genome = JsonConvert.DeserializeAnonymousType(json, genomeObject);

            hiddenWeights = genome.hidden_weight;
            inputWeights = genome.input_weight;
        }
    }

    public double GetOutput(double input1, double input2)
    {
        List<double> calculatedWeights = new(HIDDEN_LAYER_SIZE)
        {
            Sigmoid((input1 * inputWeights[0][0]) + (input2 * inputWeights[1][0])),
            Sigmoid((input1 * inputWeights[0][1]) + (input2 * inputWeights[1][1])),
            Sigmoid((input1 * inputWeights[0][2]) + (input2 * inputWeights[1][2]))
        };

        double output = Sigmoid(
            (calculatedWeights[0] * hiddenWeights[0]) +
            (calculatedWeights[1] * hiddenWeights[1]) +
            (calculatedWeights[2] * hiddenWeights[2])
        );

        return output;
    }

    public void MakeMutation()
    {
        Random random = new();

        for (int i = 0; i < random.Next(1, 10); i++)
        {
            int weightIndex = random.Next(1, 10);
            if (weightIndex <= 3)
            {
                inputWeights[0][weightIndex - 1] = RandomNumber(-1, 1);
            }
            else if (weightIndex <= 6)
            {
                inputWeights[1][weightIndex - 4] = RandomNumber(-1, 1);
            }
            else if (weightIndex <= 9)
            {
                hiddenWeights[weightIndex - 7] = RandomNumber(-1, 1);
            }
        }
    }

    private static double RandomNumber(double minValue, double maxValue)
    {
        Random random = new();

        return random.NextDouble() * (maxValue - minValue) + minValue;
    }

    private static double Sigmoid(double value)
    {
        try
        {
            return 1 / (1 + Math.Exp(-value));
        }
        catch
        {
            return 0;
        }
    }
}