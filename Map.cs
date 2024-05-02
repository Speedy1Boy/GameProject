using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ReworkedGame;

struct Tile
{
    public int ImageId;
    public Rectangle Collider;
}

internal class Map
{
    public int Size;
    public int TileSize;
    public List<Texture2D> Tiles;
    public Tile[][] Grid;
    private readonly Random r;

    public Map(int size, List<Texture2D> tiles)
    {
        Size = size;
        Tiles = tiles;
        TileSize = 16;

        r = new Random();

        Grid = new Tile[size][];
        for (var x = 0; x < size; x++)
        {
            Grid[x] = new Tile[size];
            for (var y = 0; y < size; y++)
                Grid[x][y].ImageId = 0;
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
                spriteBatch.Draw(Tiles[Grid[i][j].ImageId], new Vector2(i * TileSize, j * TileSize), Color.White);
    }

    public void GenerateForest(double prob)
    {
        for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                if (Rand(prob)) UpdateTile(i, j, RandSymbol(2) + 2);
    }

    public void GenerateLake(int length = 1)
    {
        var (rx, ry) = RandCell(Size, Size);
        UpdateTile(rx, ry, 4);
        for (var i = 0; i < length; i++)
        {
            (rx, ry) = Neighbour(rx, ry);
            if (CheckBounds(rx, ry)) UpdateTile(rx, ry, 4);
            else break;
        }
    }

    public void GenerateMountain(int length = 1)
    {
        var (rx, ry) = RandCell(Size, Size);
        UpdateTile(rx, ry, 5);
        for (var i = 0; i < length; i++)
        {
            (rx, ry) = Neighbour(rx, ry);
            if (CheckBounds(rx, ry)) UpdateTile(rx, ry, 5);
            else break;
        }
    }

    public void GenerateStraightRiver(Direction dir)
    {
        var (rx, ry) = RandCellBorderDir(Size, Size, dir);
        UpdateTile(rx, ry, 4);
        while (true)
        {
            (rx, ry) = RiverNeighbour(rx, ry, (int)dir);
            if (CheckBounds(rx, ry)) UpdateTile(rx, ry, 4);
            else break;
        }
    }

    public void StartFire()
    {
        var (x, y) = RandCell(Size, Size);
        var tileId = Grid[x][y].ImageId;
        if (2 <= tileId && tileId <= 3)
            UpdateTile(x, y, 6);
    }

    public void GenerateTree()
    {
        var (x, y) = RandCell(Size, Size);
        if (Grid[x][y].ImageId == 0)
            UpdateTile(x, y, RandSymbol(2) + 2);
    }

    public void UpdateTile(int x, int y, int id)
    {
        Grid[x][y].ImageId = id;
        Grid[x][y].Collider = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
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
