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
    public class Drawer
    {
        private Canvas _canvas;
        private int _sideSquareCount;
        private Rectangle?[,] _squares;
        private double _squareSize;

        public Drawer(Canvas canvas, int sideSquareCount)
        {
            _canvas = canvas;
            _sideSquareCount = sideSquareCount;
            _squares = new Rectangle?[sideSquareCount, sideSquareCount];
            _squareSize = canvas.ActualWidth / _sideSquareCount;
        }

        public void DrawSquare((int X, int Y) coordinates, Brush color)
        {
            Rectangle square = new Rectangle()
            {
                Fill = color,
                Width = _squareSize,
                Height = _squareSize
            };
            Canvas.SetLeft(square, coordinates.X * _squareSize);
            Canvas.SetTop(square, coordinates.Y * _squareSize);
            _canvas.Children.Add(square);
            _squares[coordinates.X, coordinates.Y] = square;
        }

        public void RemoveSquare((int X, int Y) coordinates)
        {
            Rectangle? square = _squares[coordinates.X, coordinates.Y];
            _canvas.Children.Remove(square);
            _squares[coordinates.X, coordinates.Y] = null;
        }

        public void ClearAllSquares()
        {
            _canvas.Children.Clear();
            _squares = new Rectangle?[_sideSquareCount, _sideSquareCount];
        }

        public bool CheckSquare((int X, int Y) coordinates)
        {
            return _squares[coordinates.X, coordinates.Y] != null;
        }
    }
}
