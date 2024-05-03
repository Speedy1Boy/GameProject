using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static GameProject.Utils;

namespace GameProject;

struct Tile
{
    public int ImageId;
    public Rectangle Collider;
    public int FireCounter;

    public void UpdateTile(int x, int y, int id, int tileSize)
    {
        ImageId = id;
        Collider = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
    }
}

internal class Map
{
    public int Size;
    public int TileSize;
    public List<Texture2D> Tiles;
    public Tile[][] Grid;
    private int tick;
    private int windPow;
    private Direction windDir;
    private Tile[][] cloudMap;

    public Map(int size, List<Texture2D> tiles)
    {
        Size = size;
        Tiles = tiles;
        TileSize = 16;

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
        windDir = RandDir();
        windPow = RandSymbol(10);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                spriteBatch.Draw(Tiles[Grid[i][j].ImageId], new Vector2(i * TileSize, j * TileSize), Color.White);
    }

    public void Update(Clouds clouds)
    {
        cloudMap = clouds.cloudMap;
        if (tick % 50 == 0) GenerateTree();
        if (tick % 100 == 0) UpdateGrass();
        if (tick % 50 == 0) StartFire();
        if (tick % 40 == 0) UpdateFire();
        if (tick % 500 == 0)
        {
            windDir = RandDir();
            windPow = RandSymbol(10);
        }
        if (tick % WindPowerClouds() == 0)
            clouds.MoveCloud(windDir);
        if (tick % 5 == 0) ProcessCloud();
        tick++;
    }

    public void GenerateForest(double prob)
    {
        for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                if (Rand(prob))
                    Grid[i][j].UpdateTile(i, j, RandSymbol(3) + 2, TileSize);
    }

    public void GenerateLake(int length = 1)
    {
        var (rx, ry) = RandCell(Size, Size);
        Grid[rx][ry].UpdateTile(rx, ry, 5, TileSize);
        for (var i = 0; i < length; i++)
        {
            (rx, ry) = Neighbor(rx, ry);
            if (CheckBounds(rx, ry, Size))
                Grid[rx][ry].UpdateTile(rx, ry, 5, TileSize);
            else break;
        }
    }

    public void GenerateMountain(int length = 1)
    {
        var (rx, ry) = RandCell(Size, Size);
        Grid[rx][ry].UpdateTile(rx, ry, 6, TileSize);
        for (var i = 0; i < length; i++)
        {
            (rx, ry) = Neighbor(rx, ry);
            if (CheckBounds(rx, ry, Size))
                Grid[rx][ry].UpdateTile(rx, ry, 6, TileSize);
            else break;
        }
    }

    public void GenerateStraightRiver(Direction dir)
    {
        var (rx, ry) = RandCellBorderDir(Size, Size, GetOppositDir(dir));
        Grid[rx][ry].UpdateTile(rx, ry, 5, TileSize);
        while (true)
        {
            (rx, ry) = RiverNeighbor(rx, ry, (int)dir);
            if (CheckBounds(rx, ry, Size))
                Grid[rx][ry].UpdateTile(rx, ry, 5, TileSize);
            else break;
        }
    }

    public void StartFire()
    {
        var (x, y) = RandCell(Size, Size);
        var tileId = Grid[x][y].ImageId;
        if (2 <= tileId && tileId <= 4)
            Grid[x][y].UpdateTile(x, y, 7, TileSize);
    }

    public bool CanBurn(int x, int y) => 2 <= Grid[x][y].ImageId && Grid[x][y].ImageId <= 4;

    public double Wind(Direction dir)
    {
        if (windPow == 0)
            return 1;
        if (windDir == dir)
            return windPow + 1;
        if (GetOppositDir(windDir) == dir)
            return 1 / (windPow + 1);
        return Math.Max((windPow + 1) / 2, 1);
    }

    public void UpdateFire()
    {
        var fire = new List<(int, int)>();
        for (var x = 0; x < Size; x++)
            for (var y = 0; y < Size; y++)
                if (Grid[x][y].ImageId == 7)
                {
                    fire.Add((x, y));
                    Grid[x][y].UpdateTile(x, y, 7, TileSize);
                    Grid[x][y].FireCounter++;
                    if (Grid[x][y].FireCounter >= 15)
                        Grid[x][y].UpdateTile(x, y, 1, TileSize);
                }
        foreach (var (x, y) in fire)
            for (var i = 0; i < 4; i++)
            {
                var nx = x + moves[i].x;
                var ny = y + moves[i].y;
                if (Rand(10 * Wind((Direction)i)) && CheckBounds(nx, ny, Size) && CanBurn(nx, ny))
                {
                    Grid[nx][ny].UpdateTile(nx, ny, 7, TileSize);
                    Grid[nx][ny].FireCounter++;
                }
            }
    }

    public void GenerateTree()
    {
        var (x, y) = RandCell(Size, Size);
        if (Grid[x][y].ImageId == 0)
            Grid[x][y].UpdateTile(x, y, RandSymbol(3) + 2, TileSize);
    }

    public void UpdateGrass()
    {
        for (var x = 0; x < Size; x++)
            for (var y = 0; y < Size; y++)
                if (Grid[x][y].ImageId == 0)
                    for (var i = 0; i < 4; i++)
                    {
                        var nx = x + moves[i].x;
                        var ny = y + moves[i].y;
                        if (Rand(15) && CheckBounds(nx, ny, Size) && Grid[nx][ny].ImageId == 1)
                            Grid[nx][ny].UpdateTile(nx, ny, 0, TileSize);
                    }
    }

    public int WindPowerClouds() => 2 + (9 - windPow) * 3;

    public void ProcessCloud()
    {
        for (var i = 0; i < Size; i++)
            for (var j = 0; j < Size; j++)
                if (cloudMap[i][j].ImageId == 2 && Grid[i][j].ImageId == 7)
                {
                    Grid[i][j].UpdateTile(i, j, 0, Size);
                    cloudMap[i][j].UpdateTile(i, j, 1, Size);
                }
    }
}
