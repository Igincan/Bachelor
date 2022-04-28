using Bachelor.Snake;
using System;
using System.Windows.Controls;

namespace Bachelor.AI
{
    public abstract class Agent
    {
        public abstract NextDirection GetNextDirection(string state);

        public abstract void Train(Game game, int generationCount, ProgressBar progressBar);
    }
}