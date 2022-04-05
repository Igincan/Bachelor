using Bachelor.Snake;

namespace Bachelor.AI
{
    public abstract class Agent
    {
        public abstract NextDirection GetNextDirection(int state);

        public abstract void Train(Game game);
    }
}