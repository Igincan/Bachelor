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
        public (int X, int Y) Head
        {
            get
            {
                return Body.Last.Value;
            }
        }

        private Direction _direction;
        private bool _eaten;

        public Snake((int X, int Y) startingCoordinates, Direction direction)
        {
            Body = new LinkedList<(int X, int Y)>();
            Body.AddLast(startingCoordinates);
            _direction = direction;
        }

        public void Move()
        {
            (int X, int Y) head = Body.Last.Value;
            if (_eaten)
            {
                _eaten = false;
            }
            else
            {
                Body.RemoveFirst();
            }
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

        public void ChangeDirection(Direction direction)
        {
            _direction = direction;
        }

        public void Eat()
        {
            _eaten = true;
        }
    }
}
