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
        private Drawer _drawer;

        public Game(Canvas canvas)
        {
            _snake = new Snake((2, 2), Direction.RIGHT);
            _drawer = new Drawer(canvas, 30);
        }

        public void Start()
        {
            _snake = new Snake((10, 10), Direction.RIGHT);
        }

        public void Stop()
        {
            _drawer.ClearAllSquares();
        }

        public void Update()
        {
            UpdateSnake();
            Draw();
        }

        public void ChangeSnakeDirection(Direction direction)
        {
            _snake.changeDirection(direction);
        }

        private void UpdateSnake()
        {
            _snake.Move();
        }

        private void Draw()
        {
            DrawSnake();
        }

        private void DrawSnake()
        {
            _drawer.ClearAllSquares();
            foreach (var coordinates in _snake.Body)
            {
                _drawer.DrawSquare(coordinates, (SolidColorBrush)new BrushConverter().ConvertFromString("#33A133"));
            }
        }
    }
}
