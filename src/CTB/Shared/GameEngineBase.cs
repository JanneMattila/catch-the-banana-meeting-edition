﻿using CTB.Shared.Interfaces;
using System;

namespace CTB.Shared;

public class GameEngineBase
{
    protected readonly Random _random = new();

    protected double FixAngle(double angle)
    {
        if (angle < 0)
        {
            angle += 2 * Math.PI;
        }
        else if (angle > 2 * Math.PI)
        {
            angle -= 2 * Math.PI;
        }
        return angle;
    }

    protected double CalculateAngle(Position from, Position to)
    {
        var dx = from.X - to.X;
        var dy = to.Y - from.Y;
        var angle = Math.Atan2(dy, dx);
        return Math.PI - angle;
    }

    protected double CalculateDistance(Position left, Position right)
    {
        var x1 = left.X;
        var y1 = left.Y;
        var x2 = right.X;
        var y2 = right.Y;

        var dx = x2 - x1;
        var dy = y2 - y1;
        var distance = Math.Sqrt(dx * dx + dy * dy);
        return distance;
    }

    protected bool CheckCollision(Position position1, Size size1, Position position2, Size size2)
    {
        var p1 = new Position()
        {
            X = position1.X - size1.Width / 2,
            Y = position1.Y - size1.Height / 2
        };
        var p2 = new Position()
        {
            X = position2.X - size2.Width / 2,
            Y = position2.Y - size2.Height / 2
        };

        if (p1.X > p2.X + size2.Width || p1.Y > p2.Y + size2.Height ||
            p2.X > p1.X + size1.Width || p2.Y > p1.Y + size1.Height)
        {
            return false;
        }
        return true;
    }

    protected bool IsInside(Position position1, Position position2, Position p)
    {
        var p1 = new Position()
        {
            X = Math.Min(position1.X, position2.X),
            Y = Math.Min(position1.Y, position2.Y)
        };
        var p2 = new Position()
        {
            X = Math.Max(position1.X, position2.X),
            Y = Math.Max(position1.Y, position2.Y)
        };

        if (p1.X <= p.X && p1.Y <= p.Y && p2.X >= p.X && p2.Y >= p.Y)
        {
            return true;
        }
        return false;
    }
}
