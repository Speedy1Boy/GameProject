﻿using System;
using System.Collections.Generic;

namespace GameProject;

public class Utils
{
    private static readonly Random r = new();

    public static readonly (int x, int y)[] moves = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

    public static bool CheckBounds(int x, int y, int width, int height) => x >= 0 && x < width && y >= 0 && y < height;

    public static bool Rand(double prob) => r.NextDouble() < prob / 100;

    public static int RandNumber(int n) => r.Next(n);

    public static (int, int) RandCell(int x, int y) => (r.Next(x), r.Next(y));

    public static (int, int) Neighbor(int x, int y)
    {
        var move = moves[r.Next(4)];
        return (x + move.x, y + move.y);
    }

    public static (int, int) RandBorderCell(int w, int h, Direction dir)
    {
        if (dir == Direction.Up) return (r.Next(w), h - 1);
        if (dir == Direction.Down) return (r.Next(w), 0);
        if (dir == Direction.Left) return (w - 1, r.Next(h));
        return (0, r.Next(h));
    }

    public static Direction RandDir() => (Direction)r.Next(4);

    public static (int, int) RiverNeighbor(int x, int y, int ban)
    {
        var bannedMove = new List<(int x, int y)>(moves);
        bannedMove.RemoveAt(ban);
        var move = bannedMove[r.Next(3)];
        return (x + move.x, y + move.y);
    }

    public static Direction GetOppositDir(Direction dir)
    {
        if (dir == Direction.Up) return Direction.Down;
        if (dir == Direction.Down) return Direction.Up;
        if (dir == Direction.Left) return Direction.Right;
        return Direction.Left;
    }
}
