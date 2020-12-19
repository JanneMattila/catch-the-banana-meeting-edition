using CTB.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTB.Shared
{
    public class GameEngine
    {
        private Game _game = new();
        private Action<Game> _executeDraw;
        private Action<Position> _executePlayerUpdated;
        private HashSet<int> _keys = new();

        public Game Game { get => _game; }

        public void SetExecuteDraw(Action<Game> executeDraw)
        {
            _executeDraw = executeDraw;
        }

        public void SetExecutePlayerUpdated(Action<Position> executePlayerUpdated)
        {
            _executePlayerUpdated = executePlayerUpdated;
        }

        public void SetPlayerID(string playerID)
        {
            _game.Me.ID = playerID;
        }

        public void SetPlayerName(string playerName)
        {
            _game.Me.Name = playerName;
        }

        public void OnKeyDown(int keyCode)
        {
            _keys.Add(keyCode);
            KeyboardStateChanged();
        }

        public void OnKeyUp(int keyCode)
        {
            _keys.Remove(keyCode);
            KeyboardStateChanged();
        }

        private void KeyboardStateChanged()
        {
            (var rotation, var movePlayer, var changed) = HandleKeyboard();
            if (!movePlayer)
            {
                _game.Me.Position.Rotation = rotation;
                _game.Me.Position.Speed = 0;
            }

            if (changed)
            {
                _executePlayerUpdated(_game.Me.Position);
            }
        }

        public void Update(double delta)
        {
            if (_game.Me.Position.Speed > 0)
            {
                _game.Me.Position.X += (int)Math.Round(delta / 6_000 * Math.Cos(_game.Me.Position.Rotation));
                _game.Me.Position.Y += (int)Math.Round(delta / 6_000 * Math.Sin(_game.Me.Position.Rotation));

                Console.WriteLine($"{_game.Me.Position.X}, {_game.Me.Position.Y}");
            }

            _executeDraw(_game);
        }

        private (double, bool, bool) HandleKeyboard()
        {
            var rotation = 0d;
            var movePlayer = false;

            var previousMovePlayer = _game.Me.Position.Speed > 0;
            var previousRotation = _game.Me.Position.Rotation;
            if (_keys.Contains(Keys.Up) || _keys.Contains(Keys.KeyW))
            {
                movePlayer = true;
                rotation = 3 * Math.PI / 2;
                if (_keys.Contains(Keys.Left) || _keys.Contains(Keys.KeyA))
                {
                    rotation = 4 * Math.PI / 3;
                }
                else if (_keys.Contains(Keys.Right) || _keys.Contains(Keys.KeyD))
                {
                    rotation = 5 * Math.PI / 3;
                }
            }
            else if (_keys.Contains(Keys.Down) || _keys.Contains(Keys.KeyS))
            {
                movePlayer = true;
                rotation = Math.PI / 2;
                if (_keys.Contains(Keys.Left) || _keys.Contains(Keys.KeyA))
                {
                    rotation = 2 * Math.PI / 3;
                }
                else if (_keys.Contains(Keys.Right) || _keys.Contains(Keys.KeyD))
                {
                    rotation = Math.PI / 3;
                }
            }
            else if (_keys.Contains(Keys.Left) || _keys.Contains(Keys.KeyA))
            {
                rotation = Math.PI;
                movePlayer = true;
            }
            else if (_keys.Contains(Keys.Right) || _keys.Contains(Keys.KeyD))
            {
                movePlayer = true;
            }
            var changed = 
                previousMovePlayer != movePlayer ||
                previousRotation != rotation;
            return (rotation, movePlayer, changed);
        }
    }
}
