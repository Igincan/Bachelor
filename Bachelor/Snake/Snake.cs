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
    public class Snake
    {
        public LinkedList<(int X, int Y)> Body { get; private set; }
        public (int X, int Y) Head
        {
            get
            {
                return Body.Last!.Value;
            }
        }

        private Direction _direction;
        private bool _eaten;
        private NextDirection _nextDirection;

        public Snake((int X, int Y) startingCoordinates, Direction direction)
        {
            (int X, int Y) = startingCoordinates;

            Body = new LinkedList<(int X, int Y)>();
            Body.AddLast(startingCoordinates);
            _direction = direction;
        }

        public void Move()
        {
            (int X, int Y) head = Body.Last!.Value;

            if (_eaten)
            {
                _eaten = false;
            }
            else
            {
                Body.RemoveFirst();
            }

            int newDirection = ((int)_direction + (int)_nextDirection) % 4;
            if (newDirection == -1)
            {
                newDirection = 3;
            }
            _direction = (Direction)newDirection;
            _nextDirection = NextDirection.STRAIGHT;

            switch (_direction)
            {
                case Direction.LEFT:
                    Body.AddLast((head.X - 1, head.Y));
                    break;
                case Direction.UP:
                    Body.AddLast((head.X, head.Y - 1));
                    break;
                case Direction.RIGHT:
                    Body.AddLast((head.X + 1, head.Y));
                    break;
                case Direction.DOWN:
                    Body.AddLast((head.X, head.Y + 1));
                    break;
                default:
                    break;
            }
        }

        public void ChangeNextDirection(NextDirection nextDirection)
        {
            _nextDirection = nextDirection;
        }

        public void Eat()
        {
            _eaten = true;
        }
    }
}
