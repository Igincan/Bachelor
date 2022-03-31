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

        public override NextDirection GetNextDirection()
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
    }
}
