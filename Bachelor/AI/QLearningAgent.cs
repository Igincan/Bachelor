using Bachelor.Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bachelor.AI
{
    public class QLearningAgent : Agent
    {
        private Random _random;

        public QLearningAgent()
        {
            _random = new Random();
        }

        public override NextDirection GetNextDirection(int state)
        {
            switch (_random.Next(6))
            {
                case 0:
                    return NextDirection.LEFT;
                case 1:
                    return NextDirection.RIGHT;
                default:
                    return NextDirection.STRAIGHT;
            }
        }

        public override void Train(Game game)
        {
            int episodeCount = 1000;
            int maxStepsPerEpisode = 500;

            double learningRate = 0.1;
            double discountRate = 0.99;

            double explorationRate = 1;
            double maxExplorationRate = 1;
            double minExplorationRate = 0.01;
            double explorationDecayRate = 0.001;

            for (int episodeIndex = 0; episodeIndex < episodeCount; episodeIndex++)
            {

            }
        }
    }
}
