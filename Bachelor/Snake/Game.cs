﻿using Bachelor.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Bachelor.Snake
{
    public class Game
    {
        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                if (_mainWindow != null && _withGraphics)
                {
                    _mainWindow.ScoreTextBlock.Text = value.ToString();
                }
            }
        }

        public bool Lost {
            get => _lost;
        }

        public string Enviroment
        {
            get
            {
                LinkedList<(int X, int Y)> allCoordinates = new LinkedList<(int X, int Y)>();
                int viewDistance = 3;

                foreach (var coordinates in _snake.Body.Reverse())
                {
                    (int X, int Y) newCoordinates = (coordinates.X - _snake.Head.X, coordinates.Y - _snake.Head.Y);
                    if (Math.Abs(newCoordinates.X) <= viewDistance && Math.Abs(newCoordinates.Y) <= viewDistance)
                    {
                        allCoordinates.AddLast(newCoordinates);
                    }
                }

                string enviroment = string.Join("/", allCoordinates.Select((coords) => $"{coords.X},{coords.Y}"));

                string[] borders = new string[4] { "-", "-", "-", "-" };

                if (_snake.Head.X < viewDistance)
                {
                    borders[0] = (_snake.Head.X + 1).ToString();
                }
                if (_snake.Head.Y < viewDistance)
                {
                    borders[1] = (_snake.Head.Y + 1).ToString();
                }
                if (_sideSquareCount - _snake.Head.X <= viewDistance)
                {
                    borders[2] = (_sideSquareCount - _snake.Head.X).ToString();
                }
                if (_sideSquareCount - _snake.Head.Y <= viewDistance)
                {
                    borders[2] = (_sideSquareCount - _snake.Head.Y).ToString();
                }

                (int X, int Y) newFood = (_food.X - _snake.Head.X, _food.Y - _snake.Head.Y);
                string foodStr = "-";
                if (Math.Abs(newFood.X) <= viewDistance && Math.Abs(newFood.Y) <= viewDistance)
                {
                    foodStr = $"{newFood.X},{newFood.Y}";
                }

                return $"{borders[0]}{borders[1]}{borders[2]}{borders[3]}/{foodStr}/{enviroment}";
            }
        }

        public string SimpleEnviroment
        {
            get
            {
                int left = getCoordinatesValue((_snake.Head.X - 1, _snake.Head.Y));
                int right = getCoordinatesValue((_snake.Head.X + 1, _snake.Head.Y));
                int up = getCoordinatesValue((_snake.Head.X, _snake.Head.Y - 1));
                int down = getCoordinatesValue((_snake.Head.X, _snake.Head.Y + 1));

                switch (_snake.Direction)
                {
                    case Direction.LEFT:
                        return $"{down}{left}{up}";
                    case Direction.UP:
                        return $"{left}{up}{right}";
                    case Direction.RIGHT:
                        return $"{up}{right}{down}";
                    case Direction.DOWN:
                        return $"{right}{down}{left}";
                    default:
                        return string.Empty;
                }
            }
        }

        private Snake _snake;
        private (int X, int Y) _food;
        private bool _foodEaten;
        private int _sideSquareCount;
        private Drawer _drawer;
        private Random _random;
        private bool _lost;
        private MainWindow _mainWindow;
        private int _score;
        private Agent? _agent;
        private bool _withGraphics;
        private Canvas _canvas;

        public Game(Canvas canvas, MainWindow mainWindow)
        {
            Score = 0;
            _snake = new Snake((0, 0), Direction.RIGHT);
            _food = (0, 0);
            _foodEaten = false;
            _sideSquareCount = 10;
            _drawer = new Drawer(canvas, _sideSquareCount);
            _random = new Random();
            _lost = false;
            _mainWindow = mainWindow;
            _withGraphics = false;
            _canvas = canvas;
        }

        public void ChangeSnakeNextDirection(NextDirection nextDirection)
        {
            _snake.ChangeNextDirection(nextDirection);
        }

        public void Start(bool withGraphics = true, Agent? agent = null)
        {
            _snake = new Snake((_sideSquareCount / 2, _sideSquareCount / 2), Direction.RIGHT);
            _food = getRandomCoordinates();
            _lost = false;
            _withGraphics = withGraphics;
            Score = 0;
            _agent = agent;
        }

        public void Stop()
        {
            _drawer.ClearAllSquares();
            _snake = new Snake((0, 0), Direction.RIGHT);
            _food = (0, 0);
        }

        public void Tick()
        {
            if (_agent != null)
            {
                ChangeSnakeNextDirection(_agent.GetNextDirection(Enviroment));
            }
            Update();
            if (_lost)
            {
                _mainWindow.StopSnake();
            }
            else
            {
                Draw();
            }
        }

        public int Update()
        {
            int reward = UpdateSnake();
            UpdateFood();
            return reward;
        }

        public async void TrainAgent(Agent agent, int generationCount, ProgressBar progressBar)
        {
            if (agent is QLearningAgent)
            {
                _mainWindow.TrainQLearningButton.IsEnabled = false;
                _mainWindow.QLearningGenerationCount.IsEnabled = false;
            }
            else if (agent is QLearningSimpleEnviromentAgent)
            {
                _mainWindow.TrainQLearningSimpleEnviromentButton.IsEnabled = false;
                _mainWindow.QLearningSimpleEnviromentGenerationCount.IsEnabled = false;
            }
            await Task.Run(() =>
            {
                agent.Train(new Game(_canvas, _mainWindow), generationCount, progressBar);
            });
            if (agent is QLearningAgent)
            {
                _mainWindow.TrainQLearningButton.IsEnabled = true;
                _mainWindow.QLearningGenerationCount.IsEnabled = true;
            }
            else if (agent is QLearningSimpleEnviromentAgent)
            {
                _mainWindow.TrainQLearningSimpleEnviromentButton.IsEnabled = true;
                _mainWindow.QLearningSimpleEnviromentGenerationCount.IsEnabled = true;
            }
        }

        private int UpdateSnake()
        {
            _snake.Move();

            if (_snake.Head == _food)
            {
                _foodEaten = true;
                _snake.Eat();
                Score++;
                return 1;
            }
            else if
            (
                _snake.Head.X < 0 || _snake.Head.X >= _sideSquareCount ||
                _snake.Head.Y < 0 || _snake.Head.Y >= _sideSquareCount ||
                _drawer.CheckSquare(_snake.Head)
            )
            {
                _lost = true;
                return -1;
            }
            return 0;
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

        private int getCoordinatesValue((int X, int Y)  coordinates)
        {
            if (_food == coordinates)
            {
                return 2;
            }
            else if (_drawer.CheckSquare(coordinates))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
