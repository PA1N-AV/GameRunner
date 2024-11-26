using Game;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Runer
{
    class Game
    {

        private Label[,] labels;
        private Label _scoreLabel;
        private Border _gameOver;
        private readonly Random _random;
        private Field _field;
        private Player _player;
        private MapSize _mapSize;
        private readonly int height;
        private readonly int width;
        private List<TypeAttack> _typeAttacks;
        private int _difficulty;
        private int _score;
        private bool _isGameOver;

        public bool IsGameOver
        {
            get => _isGameOver;
            private set
            {
                if (value == true && value != _isGameOver)

                    _gameOver.Visibility = Visibility.Visible;


                _isGameOver = value;
            }
        }

        private int Score
        {
            get => _score;
            set
            {
                if (value == _score) return;
                _score = value;
                _scoreLabel.Content = _score;
            }
        }

        public Game(int width, int height, Label score, Label[,] labels, Border onGameOver)
        {
            _gameOver = onGameOver;
            this.labels = labels;
            _scoreLabel = score;
            this.width = width;
            this.height = height;
            _random = new Random();

            Reset();
        }

        public void Reset()
        {
            _mapSize = new MapSize(width, height);
            _field = new Field(_mapSize);
            _player = new Player(_mapSize);
            _typeAttacks = new List<TypeAttack>();
            _difficulty = 1000;
            Score = 0;
            IsGameOver = false;
            _gameOver.Visibility = Visibility.Hidden;
        }

        public void SimulateAndRender()
        {
            if (IsGameOver)
                return;

            Simulate();

            bool death = false;

            for (int y = 0; y < _mapSize.Height; y++)
            {
                for (int x = 0; x < _mapSize.Width; x++)
                {
                    string str;
                    Visible visible = _field.Look(x, y);
                    if ((_player.X == x) && (_player.Y == y))
                    {
                        str = "\u26D1";
                        if (visible.Color == Colors.Red)
                        {
                            death = true;
                        }
                    }
                    else
                    {
                        if (visible.Time != 0)
                        {
                            str = visible.Time.ToString();
                        }
                        else
                        {
                            str = string.Empty;
                        }
                    }

                    //OnCellChanged?.Invoke(this, new CellChangeEventArgs(x, y, str, visible.Color));
                    labels[y,x].Content = str;
                    labels[y,x].Background = new SolidColorBrush(visible.Color);
                }
            }

            if (death)
            {
                IsGameOver = true;
            }
        }

        public void Move(Offset moveOffset)
        {
            if (IsGameOver)
                return;

            _player.Move(moveOffset);

            _field.Tact();

            Score++;
        }

        private void Simulate()
        {
            _difficulty -= 2;
            for (int y = 0; y < _mapSize.Height; y++)
            {
                for (int x = 0; x < _mapSize.Width; x++)
                {
                    if (_random.Next(1, _difficulty + 1) < 18)
                    {
                        _field.AddAttack(x, y, new Attack(_random.Next(3, 5), _random.Next(1, 3)));
                    }
                }
            }

            for (int y = 0; y < _mapSize.Height; y++)
            {
                if (_random.Next(1, _difficulty + 1) < 7)
                {
                    for (int x = 0; x < _mapSize.Width; x++)
                    {
                        _field.AddAttack(x, y, new Attack(_random.Next(3, 5), _random.Next(1, 3)));
                    }
                }
            }

            for (int x = 0; x < _mapSize.Width; x++)
            {
                if (_random.Next(1, _difficulty + 1) < 7)
                {
                    for (int y = 0; y < _mapSize.Height; y++)
                    {
                        _field.AddAttack(x, y, new Attack(_random.Next(3, 5), _random.Next(1, 3)));
                    }
                }
            }

            for (int d = 0; d < 2 * _mapSize.Height; d++)
            {
                if (_random.Next(1, _difficulty + 1) < 7)
                {
                    for (int y = 0; y < _mapSize.Height; y++)
                    {
                        _field.AddAttack(d - y, y, new Attack(_random.Next(3, 5), _random.Next(1, 3)));
                    }
                }
            }

            for (int d = 0; d < 2 * _mapSize.Height; d++)
            {
                if (_random.Next(1, _difficulty + 1) < 7)
                {
                    for (int y = 0; y < _mapSize.Height; y++)
                    {
                        _field.AddAttack(d + y - _mapSize.Width - 1, y,
                            new Attack(_random.Next(3, 5), _random.Next(1, 3)));
                    }
                }
            }

            if (_random.Next(1, _difficulty + 1) < 10)
            {
                _typeAttacks.Add(new TypeAttack(_random.Next(-5, 5) + _player.X, _random.Next(-5, 5) + _player.Y,
                    new Attack(_random.Next(2, 3), _random.Next(1, 4)), "cube", _random.Next(0, 3),
                    _random.Next(2, 5)));
            }

            if (_random.Next(1, _difficulty + 1) < 10)
            {
                _typeAttacks.Add(new TypeAttack(_random.Next(-5, 5) + _player.X, _random.Next(-5, 5) + _player.Y,
                    new Attack(_random.Next(2, 3), _random.Next(1, 4)), "cursor1", _random.Next(0, 3),
                    _random.Next(2, 5)));
            }

            if (_random.Next(1, _difficulty + 1) < 10)
            {
                _typeAttacks.Add(new TypeAttack(_random.Next(-5, 5) + _player.X, _random.Next(-5, 5) + _player.Y,
                    new Attack(_random.Next(2, 3), _random.Next(1, 4)), "cursor2", _random.Next(0, 3),
                    _random.Next(2, 5)));
            }

            if (_random.Next(1, _difficulty + 1) < 10)
            {
                _typeAttacks.Add(new TypeAttack(_random.Next(-5, 5) + _player.X, _random.Next(-5, 5) + _player.Y,
                    new Attack(_random.Next(2, 3), _random.Next(1, 4)), "pelustka", 0, _random.Next(2, 5)));
            }

            for (int i = 0; i < _typeAttacks.Count; i++)
            {
                if (_typeAttacks[i].Time > _typeAttacks[i].AllTime)
                {
                    _typeAttacks.RemoveAt(i);
                    i--;
                    continue;
                }

                if (_typeAttacks[i].Type == "cube")
                {
                    for (int l = 0; l <= _typeAttacks[i].Time; l++)
                    {
                        _field.AddAttack(_typeAttacks[i].X + l, _typeAttacks[i].Y + _typeAttacks[i].Time,
                            new Attack(_typeAttacks[i].Attack));
                        _field.AddAttack(_typeAttacks[i].X - l, _typeAttacks[i].Y - _typeAttacks[i].Time,
                            new Attack(_typeAttacks[i].Attack));
                        _field.AddAttack(_typeAttacks[i].X + _typeAttacks[i].Time, _typeAttacks[i].Y - l,
                            new Attack(_typeAttacks[i].Attack));
                        _field.AddAttack(_typeAttacks[i].X - _typeAttacks[i].Time, _typeAttacks[i].Y + l,
                            new Attack(_typeAttacks[i].Attack));
                        _field.AddAttack(_typeAttacks[i].X - l, _typeAttacks[i].Y + _typeAttacks[i].Time,
                            new Attack(_typeAttacks[i].Attack));
                        _field.AddAttack(_typeAttacks[i].X + l, _typeAttacks[i].Y - _typeAttacks[i].Time,
                            new Attack(_typeAttacks[i].Attack));
                        _field.AddAttack(_typeAttacks[i].X + _typeAttacks[i].Time, _typeAttacks[i].Y + l,
                            new Attack(_typeAttacks[i].Attack));
                        _field.AddAttack(_typeAttacks[i].X - _typeAttacks[i].Time, _typeAttacks[i].Y - l,
                            new Attack(_typeAttacks[i].Attack));
                    }
                }

                if (_typeAttacks[i].Type == "pelustka")
                {
                    for (int l = 0; l <= _typeAttacks[i].Time; l++)
                    {
                        if (_random.Next(0, 2) == 0)
                        {
                            _field.AddAttack(_typeAttacks[i].X + l, _typeAttacks[i].Y + _typeAttacks[i].Time,
                                new Attack(_typeAttacks[i].Attack));
                            _field.AddAttack(_typeAttacks[i].X + _typeAttacks[i].Time, _typeAttacks[i].Y + l,
                                new Attack(_typeAttacks[i].Attack));
                        }

                        if (_random.Next(0, 2) == 0)
                        {
                            _field.AddAttack(_typeAttacks[i].X - l, _typeAttacks[i].Y - _typeAttacks[i].Time,
                                new Attack(_typeAttacks[i].Attack));
                            _field.AddAttack(_typeAttacks[i].X - _typeAttacks[i].Time, _typeAttacks[i].Y - l,
                                new Attack(_typeAttacks[i].Attack));
                        }

                        if (_random.Next(0, 2) == 0)
                        {
                            _field.AddAttack(_typeAttacks[i].X - l, _typeAttacks[i].Y + _typeAttacks[i].Time,
                                new Attack(_typeAttacks[i].Attack));
                            _field.AddAttack(_typeAttacks[i].X - _typeAttacks[i].Time, _typeAttacks[i].Y + l,
                                new Attack(_typeAttacks[i].Attack));
                        }

                        if (_random.Next(0, 2) == 0)
                        {
                            _field.AddAttack(_typeAttacks[i].X + l, _typeAttacks[i].Y - _typeAttacks[i].Time,
                                new Attack(_typeAttacks[i].Attack));
                            _field.AddAttack(_typeAttacks[i].X + _typeAttacks[i].Time, _typeAttacks[i].Y - l,
                                new Attack(_typeAttacks[i].Attack));
                        }
                    }

                    if (_typeAttacks[i].Type == "cursor1")
                    {
                        for (int l = 0; l <= _typeAttacks[i].Time; l++)
                        {
                            _field.AddAttack(_typeAttacks[i].X + _typeAttacks[i].Time + l,
                                _typeAttacks[i].Y - _typeAttacks[i].Time, new Attack(_typeAttacks[i].Attack));
                            _field.AddAttack(_typeAttacks[i].X + _typeAttacks[i].Time,
                                _typeAttacks[i].Y - _typeAttacks[i].Time - l, new Attack(_typeAttacks[i].Attack));

                            _field.AddAttack(_typeAttacks[i].X - _typeAttacks[i].Time - l,
                                _typeAttacks[i].Y - _typeAttacks[i].Time, new Attack(_typeAttacks[i].Attack));
                            _field.AddAttack(_typeAttacks[i].X - _typeAttacks[i].Time,
                                _typeAttacks[i].Y - _typeAttacks[i].Time - l, new Attack(_typeAttacks[i].Attack));
                        }
                    }

                    if (_typeAttacks[i].Type == "cursor2")
                    {
                        for (int l = 0; l <= _typeAttacks[i].Time; l++)
                        {
                            _field.AddAttack(_typeAttacks[i].X - _typeAttacks[i].Time - l,
                                _typeAttacks[i].Y + _typeAttacks[i].Time, new Attack(_typeAttacks[i].Attack));
                            _field.AddAttack(_typeAttacks[i].X - _typeAttacks[i].Time,
                                _typeAttacks[i].Y + _typeAttacks[i].Time + l, new Attack(_typeAttacks[i].Attack));

                            _field.AddAttack(_typeAttacks[i].X + _typeAttacks[i].Time + l,
                                _typeAttacks[i].Y + _typeAttacks[i].Time, new Attack(_typeAttacks[i].Attack));
                            _field.AddAttack(_typeAttacks[i].X + _typeAttacks[i].Time,
                                _typeAttacks[i].Y + _typeAttacks[i].Time + l, new Attack(_typeAttacks[i].Attack));
                        }
                    }
                }

                _typeAttacks[i].Time++;
            }
        }

    }
}