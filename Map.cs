using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ReworkedGame;

struct Tile
{
    public int imageId;
}

internal class Map
{
    public int Size;
    public int TileSize;
    public List<Texture2D> Tiles;
    public Tile[][] map;
    private readonly Random r;

    public Map(int size, List<Texture2D> tiles)
    {
        Size = size;
        Tiles = tiles;
        TileSize = 16;

        r = new Random();

        map = new Tile[size][];
        for (var x = 0; x < size; x++)
        {
            map[x] = new Tile[size];
            for (var y = 0; y < size; y++)
                map[x][y].imageId = 0;
        }
        GenerateForest(85);
        for (var i = 0; i < 5; i++) GenerateLake(100);
        for (var i = 0; i < 7; i++) GenerateMountain(50);
        GenerateStraightRiver(RandDir());
        GenerateLake(30);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                spriteBatch.Draw(Tiles[map[i][j].imageId], new Vector2(i * TileSize, j * TileSize), Color.White);
    }

    public void GenerateForest(double prob)
    {
        for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                if (Rand(prob)) map[i][j].imageId = RandSymbol(2) + 2;
    }

    public void GenerateLake(int length = 1)
    {
        var (rx, ry) = RandCell(Size, Size);
        map[rx][ry].imageId = 4;
        for (var i = 0; i < length; i++)
        {
            (rx, ry) = Neighbour(rx, ry);
            if (CheckBounds(rx, ry)) map[rx][ry].imageId = 4;
            else break;
        }
    }

    public void GenerateMountain(int length = 1)
    {
        var (rx, ry) = RandCell(Size, Size);
        map[rx][ry].imageId = 5;
        for (var i = 0; i < length; i++)
        {
            (rx, ry) = Neighbour(rx, ry);
            if (CheckBounds(rx, ry)) map[rx][ry].imageId = 5;
            else break;
        }
    }

    public void GenerateStraightRiver(Direction dir)
    {
        var (rx, ry) = RandCellBorderDir(Size, Size, dir);
        map[rx][ry].imageId = 4;
        while (true)
        {
            (rx, ry) = RiverNeighbour(rx, ry, (int)dir);
            if (CheckBounds(rx, ry)) map[rx][ry].imageId = 4;
            else break;
        }
    }

    public void StartFire()
    {
        var (x, y) = RandCell(Size, Size);
        if (2 <= map[x][y].imageId && map[x][y].imageId <= 3) map[x][y].imageId = 6;
    }

    public void GenerateTree()
    {
        var (x, y) = RandCell(Size, Size);
        if (map[x][y].imageId == 0) map[x][y].imageId = RandSymbol(2) + 2;
    }

    public bool CheckBounds(int x, int y) => 0 <= x && x < Size && 0 <= y && y < Size;

    public bool Rand(double prob) => r.NextDouble() < prob / 100;

    public int RandSymbol(int n) => r.Next(n);

    public (int, int) RandCell(int x, int y) => (r.Next(x), r.Next(y));

    private readonly (int, int)[] moves = new[] { (0, 1), (0, -1), (1, 0), (-1, 0) };

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public (int, int) Neighbour(int x, int y)
    {
        var move = moves[r.Next(4)];
        return (x + move.Item1, y + move.Item2);
    }

    public (int, int) RandCellBorderDir(int h, int w, Direction dir)
    {
        if (dir == Direction.Up) return (r.Next(h), w - 1);
        if (dir == Direction.Down) return (r.Next(h), 0);
        if (dir == Direction.Left) return (h - 1, r.Next(w));
        return (0, r.Next(w));
    }

    public Direction RandDir() => (Direction)r.Next(4);

    public (int, int) RiverNeighbour(int x, int y, int ban)
    {
        var mBanned = new List<(int, int)>(moves);
        mBanned.RemoveAt(ban);
        var move = mBanned[r.Next(3)];
        return (x + move.Item1, y + move.Item2);
    }
}
