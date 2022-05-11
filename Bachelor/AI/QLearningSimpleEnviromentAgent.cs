using Bachelor.Snake;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Bachelor.AI
{
    public class QLearningSimpleEnviromentAgent : Agent
    {
        private readonly Random _random;
        private Dictionary<string, double[]> _qTable;

        public QLearningSimpleEnviromentAgent()
        {
            _random = new Random();
            _qTable = new Dictionary<string, double[]>();
            
        }

        public bool LoadTable()
        {
            if (File.Exists("q_table_simple"))
            {
                var lines = File.ReadAllLines("q_table_simple");
                foreach (var line in lines)
                {
                    var fields = line.Split('|');
                    double.TryParse(fields[1], out var field1);
                    double.TryParse(fields[2], out var field2);
                    double.TryParse(fields[3], out var field3);
                    _qTable.Add(fields[0], new double[] { field1, field2, field3 });
                }
                return true;
            }
            return false;
        }

        public override NextDirection GetNextDirection(string state)
        {
            if (!_qTable.ContainsKey(state))
            {
                return MapIntToNextDirection(_random.Next(3));
            }
            var actions = _qTable[state].ToList();
            return MapIntToNextDirection(actions.IndexOf(actions.Max()));
        }

        public override void Train(Game game, int episodeCount, ProgressBar progressBar)
        {
            Stopwatch sw = new Stopwatch();
            LinkedList<long> durationsInMsPer1000Episodes = new LinkedList<long>();
            LinkedList<int> scoresPerEpisode = new LinkedList<int>();

            int maxStepsPerEpisode = 500;

            double learningRate = 0.1;
            double discountRate = 0.99;

            double explorationRate = 1;
            double maxExplorationRate = 1;
            double minExplorationRate = 0.01;
            double explorationDecayRate = 0.0001;

            for (int episodeIndex = 0; episodeIndex < episodeCount; episodeIndex++)
            {
                if (episodeIndex == 0)
                {
                    sw.Start();
                }
                else if (episodeIndex % 1000 == 0)
                {
                    sw.Stop();
                    durationsInMsPer1000Episodes.AddLast(sw.ElapsedMilliseconds);
                    sw.Reset();
                    sw.Start();
                }

                game.Start(false);

                string state = game.SimpleEnviroment;
                int action;

                for (int stepIndex = 0; stepIndex < maxStepsPerEpisode; stepIndex++)
                {
                    if (!_qTable.ContainsKey(state))
                    {
                        _qTable.Add(state, new double[3]);
                    }

                    if (_random.NextDouble() > explorationRate)
                    {
                        var actions = _qTable[state].ToList();
                        action = actions.IndexOf(actions.Max());
                    }
                    else
                    {
                        action = _random.Next(3);
                    }

                    game.ChangeSnakeNextDirection(MapIntToNextDirection(action));
                    int reward = game.Update();
                    string newState = game.SimpleEnviroment;

                    if (!_qTable.ContainsKey(newState))
                    {
                        _qTable.Add(newState, new double[3]);
                    }

                                                                  // stará Q-hodnota  //
                    _qTable[state][action] = (1 - learningRate) * _qTable[state][action] +
                                             learningRate       * (reward + discountRate * _qTable[newState].Max());
                                                                  // nová Q-hodnota                              //

                    state = newState;

                    if (game.Lost)
                    {
                        break;
                    }

                    explorationRate = minExplorationRate + (maxExplorationRate - minExplorationRate) *
                                      Math.Exp(-explorationDecayRate * episodeIndex);
                }
                if (episodeIndex % 100 == 0)
                {
                    progressBar.Dispatcher.BeginInvoke(() => progressBar.Value = (double)episodeIndex / episodeCount * 100);
                }
                scoresPerEpisode.AddLast(game.Score);
            }
            sw.Stop();
            durationsInMsPer1000Episodes.AddLast(sw.ElapsedMilliseconds);

            progressBar.Dispatcher.BeginInvoke(() => progressBar.Value = 100);
            var lines = new List<string>();
            foreach (var item in _qTable)
            {
                lines.Add($"{item.Key}|{item.Value[0]}|{item.Value[1]}|{item.Value[2]}");
            }
            File.WriteAllLines("q_table_simple", lines);

            File.WriteAllLines("out/scores_mini.csv", scoresPerEpisode.Select(score => score.ToString()));
            File.WriteAllLines("out/durations_mini.csv", durationsInMsPer1000Episodes.Select(duration => duration.ToString()));
        }

        private static NextDirection MapIntToNextDirection(int integer)
        {
            switch (integer)
            {
                case 0:
                    return NextDirection.LEFT;
                case 1:
                    return NextDirection.STRAIGHT;
                case 2:
                    return NextDirection.RIGHT;
                default:
                    throw new Exception("Invalid direction!");
            }
        }

        private static int MapNextDirectionToInt(NextDirection nextDirection)
        {
            switch (nextDirection)
            {
                case NextDirection.LEFT:
                    return 0;
                case NextDirection.STRAIGHT:
                    return 1;
                case NextDirection.RIGHT:
                    return 2;
                default:
                    throw new Exception("Invalid direction!");
            }
        }
    }
}
