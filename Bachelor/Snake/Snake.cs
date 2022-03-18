using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Bachelor.Snake
{
    internal class Snake
    {
        public LinkedList<(int X, int Y)> Body { get; private set; }
        private Direction _direction;

        public Snake((int X, int Y) startingCoordinates, Direction direction)
        {
            Body = new LinkedList<(int X, int Y)>();
            Body.AddLast(startingCoordinates);
            Body.AddLast((startingCoordinates.X - 1, startingCoordinates.Y));
            Body.AddLast((startingCoordinates.X - 2, startingCoordinates.Y));
            Body.AddLast((startingCoordinates.X - 3, startingCoordinates.Y));
            Body.AddLast((startingCoordinates.X - 4, startingCoordinates.Y));
            Body.AddLast((startingCoordinates.X - 5, startingCoordinates.Y));
            Body.AddLast((startingCoordinates.X - 6, startingCoordinates.Y));
            _direction = direction;
        }

        public void Move()
        {
            (int X, int Y) head = Body.Last.Value;
            Body.RemoveFirst();
            switch (_direction)
            {
                case Direction.UP:
                    Body.AddLast((head.X, head.Y - 1));
                    break;
                case Direction.DOWN:
                    Body.AddLast((head.X, head.Y + 1));
                    break;
                case Direction.LEFT:
                    Body.AddLast((head.X - 1, head.Y));
                    break;
                case Direction.RIGHT:
                    Body.AddLast((head.X + 1, head.Y));
                    break;
                default:
                    break;
            }
        }

        public void changeDirection(Direction direction)
        {
            _direction = direction;
        }
    }
}
