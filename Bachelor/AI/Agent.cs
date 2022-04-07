using Bachelor.Snake;

namespace Bachelor.AI
{
    public abstract class Agent
    {
        public abstract NextDirection GetNextDirection(string state);

        public abstract void Train(Game game);
    }
}