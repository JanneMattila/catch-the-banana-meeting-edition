using CTB.Shared;
using CTB.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CTB.Client.Logic;

public class GameEngineClient : GameEngineBase
{
    private Game _game = new();
    private Action<Game> _executeDraw;
    private Action<Position> _executePlayerUpdated;
    private HashSet<int> _keys = new();
    private double _positionUpdated = 0;

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

    public void SetPlayer(Monkey monkey)
    {
        _game.Me.Name = monkey.Name;
        _game.Me.UI = monkey.UI;
        _game.Me.Score = monkey.Score;
        _game.Me.Position = monkey.Position;
    }

    public void OnClearInput()
    {
        _keys.Clear();
        KeyboardStateChanged();
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
        _game.Me.Position.Speed = movePlayer ? 1 : 0;
        _game.Me.Position.Rotation = rotation;

        if (changed)
        {
            _executePlayerUpdated(_game.Me.Position);
            _positionUpdated = 0;
        }
    }

    public void Update(double delta)
    {
        _game.Me.Update(delta);

        _positionUpdated += delta;
        if (_positionUpdated > 60)
        {
            _executePlayerUpdated(_game.Me.Position);
            _positionUpdated -= 60;
        }

        foreach (var monkey in _game.Monkeys)
        {
            monkey.Update(delta);
        }

        foreach (var shark in _game.Sharks)
        {
            var monkey = GetMonkey(shark.Follows);
            if (monkey != null)
            {
                shark.Position.Rotation = CalculateAngle(shark.Position, monkey.Position);
                shark.Update(delta);
            }
        }

        _executeDraw(_game);
    }

    private Monkey GetMonkey(string id)
    {
        if (_game.Me.ID == id)
        {
            return _game.Me;
        }
        else
        {
            return _game.Monkeys.FirstOrDefault(m => m.ID == id);
        }
    }

    private (double, bool, bool) HandleKeyboard()
    {
        var rotation = _game.Me.Position.Rotation;
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
            rotation = 0;
            movePlayer = true;
        }
        var changed =
            previousMovePlayer != movePlayer ||
            previousRotation != rotation;
        return (rotation, movePlayer, changed);
    }

    public void MonkeyUpdate(Monkey monkey)
    {
        MonkeyDelete(monkey);

        UpdateScoreBoard(monkey);

        if (_game.Me.ID == monkey.ID)
        {
            SetPlayer(monkey);
        }
        else
        {
            _game.Monkeys.Add(monkey);
        }
    }

    private void UpdateScoreBoard(Monkey monkey)
    {
        _game.ScoreBoard.RemoveAll(m => m.ID == monkey.ID);
        _game.ScoreBoard.Add(ScoreBoard.FromMonkey(monkey));
        _game.ScoreBoard.Sort((left, right) => right.Score.CompareTo(left.Score));
    }

    public void MonkeyDelete(Monkey monkey)
    {
        _game.Monkeys.RemoveAll(m => m.ID == monkey.ID);
    }

    public void BananaUpdate(Banana banana)
    {
        Console.WriteLine($"Update Banana ID: {banana.ID}, {banana.Position}");

        _game.Bananas.RemoveAll(m => m.ID == banana.ID);
        _game.Bananas.Add(banana);
    }

    public void BananaDelete(string id, List<string> monkeyPoints)
    {
        _game.Bananas.RemoveAll(m => m.ID == id);
        foreach (var monkeyPoint in monkeyPoints)
        {
            var monkey = GetMonkey(monkeyPoint);
            if (monkey != null)
            {
                monkey.Score++;
                UpdateScoreBoard(monkey);
            }
        }
    }

    public void SharkUpdate(Shark shark)
    {
        Console.WriteLine($"Update Shark ID: {shark.ID}, {shark.Position}");
        _game.Sharks.RemoveAll(m => m.ID == shark.ID);
        _game.Sharks.Add(shark);
    }

    public void SharkDelete(string id)
    {
        _game.Sharks.RemoveAll(m => m.ID == id);
    }

    public void OnCanvasTouch(CanvasTouch leftTouchStart, CanvasTouch leftTouchCurrent, CanvasTouch rightTouchCurrent)
    {
        var previousRotation = _game.Me.Position.Rotation;
        var previousSpeed = _game.Me.Position.Speed;

        if (leftTouchStart != null && leftTouchCurrent != null)
        {
            var touchx = leftTouchCurrent.X - leftTouchStart.X;
            var touchy = leftTouchCurrent.Y - leftTouchStart.Y;
            var angle = Math.Atan2(touchy, touchx);
            angle = angle < 0 ? Math.PI * 2 + angle : angle;
            _game.Me.Position.Rotation = angle;
            //var rotation = _game.Me.Position.Rotation;
            //var deltaLeft = FixAngle(rotation - angle);
            //var deltaRight = FixAngle(angle - _game.Me.Position.Rotation);
            var len = Math.Sqrt(touchx * touchx + touchy * touchy);
            _game.Me.Position.Speed = len > 5 ? 1 : 0;
        }
        else
        {
            _game.Me.Position.Rotation = 0;
            _game.Me.Position.Speed = 0;
        }

        if (previousRotation != _game.Me.Position.Rotation ||
            previousSpeed != _game.Me.Position.Speed)
        {
            _executePlayerUpdated(_game.Me.Position);
        }
    }
}
