using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bachelor.Snake
{
    internal class Game
    {
        private Snake _snake;
        private (int X, int Y) _food;
        private bool _foodEaten;
        private int _sideSquareCount;
        private Drawer _drawer;
        private Random _random;

        public Game(Canvas canvas)
        {
            _snake = new Snake((2, 2), Direction.RIGHT);
            _food = (4, 2);
            _foodEaten = false;
            _sideSquareCount = 30;
            _drawer = new Drawer(canvas, _sideSquareCount);
            _random = new Random();
        }

        public void ChangeSnakeDirection(Direction direction)
        {
            _snake.ChangeDirection(direction);
        }

        public void Start()
        {
            _snake = new Snake((2, 2), Direction.RIGHT);
            _food = (4, 2);
        }

        public void Stop()
        {
            _drawer.ClearAllSquares();
        }

        public void Tick()
        {
            Update();
            Draw();
        }

        public void Update()
        {
            UpdateSnake();
            UpdateFood();
        }

        private void UpdateSnake()
        {
            _snake.Move();

            if (_snake.Head == _food)
            {
                _foodEaten = true;
                _snake.Eat();
            }
        }

        private void UpdateFood()
        {
            if (_foodEaten)
            {
                _food = (_random.Next(_sideSquareCount), _random.Next(_sideSquareCount));
                _foodEaten = false;
            }
        }

        private void Draw()
        {
            _drawer.ClearAllSquares();

            DrawSnake();
            DrawFood();
        }

        private void DrawSnake()
        {
            foreach (var coordinates in _snake.Body)
            {
                _drawer.DrawSquare(coordinates, (SolidColorBrush)new BrushConverter().ConvertFromString("#33A133"));
            }
        }

        private void DrawFood()
        {
            _drawer.DrawSquare(_food, Brushes.DarkRed);
        }
    }
}
