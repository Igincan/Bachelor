﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        private bool _lost;
        private Action _stopSnake;

        public Game(Canvas canvas, Action stopSnake)
        {
            _snake = new Snake((0, 0), Direction.RIGHT);
            _food = (0, 0);
            _foodEaten = false;
            _sideSquareCount = 20;
            _drawer = new Drawer(canvas, _sideSquareCount);
            _random = new Random();
            _lost = false;
            _stopSnake = stopSnake;
        }

        public void ChangeSnakeNextDirection(NextDirection nextDirection)
        {
            _snake.ChangeNextDirection(nextDirection);
        }

        public void Start()
        {
            _snake = new Snake((_sideSquareCount / 2, _sideSquareCount / 2), Direction.RIGHT);
            _food = getRandomCoordinates();
            _lost = false;
        }

        public void Stop()
        {
            _drawer.ClearAllSquares();
            _snake = new Snake((0, 0), Direction.RIGHT);
            _food = (0, 0);
        }

        public void Tick()
        {
            Update();
            if (_lost)
            {
                _stopSnake();
            }
            else
            {
                Draw();
            }
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
            else if
            (
                _snake.Head.X < 0 || _snake.Head.X >= _sideSquareCount ||
                _snake.Head.Y < 0 || _snake.Head.Y >= _sideSquareCount ||
                _drawer.CheckSquare(_snake.Head)
            )
            {
                _lost = true;
            }
        }

        private void UpdateFood()
        {
            if (_foodEaten)
            {
                _food = getRandomCoordinates();
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
                _drawer.DrawSquare(coordinates, (SolidColorBrush)new BrushConverter().ConvertFromString("#33A133")!);
            }

            _drawer.DrawSquare(_snake.Head, (SolidColorBrush)new BrushConverter().ConvertFromString("#226B22")!);
        }

        private void DrawFood()
        {
            _drawer.DrawSquare(_food, Brushes.DarkRed);
        }

        private (int X, int Y) getRandomCoordinates()
        {
            (int X, int Y) coordinates;
            do
            {
                coordinates = (_random.Next(_sideSquareCount), _random.Next(_sideSquareCount));
            } while (_drawer.CheckSquare(coordinates));
            return coordinates;
        }
    }
}
