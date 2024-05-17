using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static GameProject.Utils;

namespace GameProject;

public class MapModel
{
    public int Width { get; }
    public int Height { get; }
    public List<Texture2D> Tiles { get; }
    public int TileSize { get; }
    public Tile[][] Grid { get; }
    public Vector2 SpawnCoords { get; }
    public SpriteFont Font { get; }
    public GameWindow Window { get; }
    public int WindPower { get; set; }
    public Direction WindDirection { get; set; }

    public MapModel(int width, int height, List<Texture2D> tiles, SpriteFont font, GameWindow window)
    {
        Width = width;
        Height = height;
        Tiles = tiles;
        TileSize = 16;
        Font = font;
        Window = window;

        Grid = new Tile[height][];
        for (var y = 0; y < height; y++)
        {
            Grid[y] = new Tile[width];
            for (var x = 0; x < width; x++)
                Grid[y][x].UpdateTile(x, y, 4, TileSize);
        }

        GenerateForest(85);
        for (var i = 0; i < 5; i++) GenerateLake(100);
        for (var i = 0; i < 7; i++) GenerateMountain(50);
        GenerateStraightRiver(RandDir());
        GenerateNearRiver(5);
        SpawnCoords = GenerateNearRiver(6);

        WindDirection = RandDir();
        WindPower = RandNumber(10);
    }

    public void GenerateForest(double prob)
    {
        for (var i = 0; i < Width; i++)
            for (var j = 0; j < Height; j++)
                if (Rand(prob))
                    Grid[j][i].UpdateTile(i, j, RandNumber(3) + 7, TileSize);
    }

    public void GenerateLake(int length = 1)
    {
        var (rX, rY) = RandCell(Width, Height);
        Grid[rY][rX].UpdateTile(rX, rY, 10, TileSize);
        for (var i = 0; i < length; i++)
        {
            (rX, rY) = Neighbor(rX, rY);
            if (CheckBounds(rX, rY, Width, Height))
                Grid[rY][rX].UpdateTile(rX, rY, 10, TileSize);
            else break;
        }
    }

    public void GenerateMountain(int length = 1)
    {
        var (rX, rY) = RandCell(Width, Height);
        Grid[rY][rX].UpdateTile(rX, rY, 1, TileSize);
        for (var i = 0; i < length; i++)
        {
            (rX, rY) = Neighbor(rX, rY);
            if (CheckBounds(rX, rY, Width, Height))
                Grid[rY][rX].UpdateTile(rX, rY, 1, TileSize);
            else break;
        }
    }

    public void GenerateStraightRiver(Direction dir)
    {
        var (rX, rY) = RandBorderCell(Width, Height, GetOppositDir(dir));
        Grid[rY][rX].UpdateTile(rX, rY, 10, TileSize);
        while (true)
        {
            (rX, rY) = RiverNeighbor(rX, rY, (int)dir);
            if (CheckBounds(rX, rY, Width, Height))
                Grid[rY][rX].UpdateTile(rX, rY, 10, TileSize);
            else break;
        }
    }

    public void StartFire()
    {
        var (x, y) = RandCell(Width, Height);
        var tileId = Grid[y][x].ImageId;
        if (tileId >= 7 && tileId <= 9)
            Grid[y][x].UpdateTile(x, y, 3, TileSize);
    }

    public bool CanBurn(int x, int y) => Grid[y][x].ImageId >= 7 && Grid[y][x].ImageId <= 9;

    public double Wind(Direction dir)
    {
        if (WindPower == 0)
            return 1;
        if (WindDirection == dir)
            return WindPower + 1;
        if (GetOppositDir(WindDirection) == dir)
            return 1 / (WindPower + 1);
        return MathHelper.Max((WindPower + 1) / 2, 1);
    }

    public void UpdateFire()
    {
        var fire = new List<(int, int)>();
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (Grid[y][x].ImageId == 3)
                {
                    fire.Add((x, y));
                    Grid[y][x].FireCounter++;
                    if (Grid[y][x].FireCounter > 15)
                    {
                        Grid[y][x].UpdateTile(x, y, 2, TileSize);
                        Grid[y][x].FireCounter = 0;
                    }
                }
        foreach (var (x, y) in fire)
            for (var i = 0; i < 4; i++)
            {
                var nX = x + moves[i].x;
                var nY = y + moves[i].y;
                if (Rand(10 * Wind((Direction)i)) && CheckBounds(nX, nY, Width, Height) && CanBurn(nX, nY))
                {
                    Grid[nY][nX].UpdateTile(nX, nY, 3, TileSize);
                    Grid[nY][nX].FireCounter++;
                }
            }
    }

    public void GenerateTree()
    {
        var (x, y) = RandCell(Width, Height);
        if (Grid[y][x].ImageId == 4)
            Grid[y][x].UpdateTile(x, y, RandNumber(3) + 7, TileSize);
    }

    public void UpdateGrass()
    {
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (Grid[y][x].ImageId == 4)
                    for (var i = 0; i < 4; i++)
                    {
                        var nX = x + moves[i].x;
                        var nY = y + moves[i].y;
                        if (Rand(15) && CheckBounds(nX, nY, Width, Height) && Grid[nY][nX].ImageId == 2)
                            Grid[nY][nX].UpdateTile(nX, nY, 4, TileSize);
                    }
    }

    public int WindPowerCloudsUpdate() => 20 + (9 - WindPower) * 3;

    public Vector2 GenerateNearRiver(int id)
    {
        var river = new List<(int, int)>();
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (Grid[y][x].ImageId == 10)
                    river.Add((x, y));
        var (rX, rY) = river[RandNumber(river.Count)];
        Grid[rY][rX].UpdateTile(rX, rY, id, TileSize);
        return new Vector2(rX * TileSize, rY * TileSize);
    }

    public Vector2 CalculateBottomTextPos(int x, int y)
    {
        var (nX, nY) = CalculateNewPos(x, Height + y);
        return new Vector2(nX, nY - 2);
    }

    public (int, int) CalculateNewPos(int x, int y)
    {
        return (GetCenteredX() + x * GetNewTileSize(), GetCenteredY() + y * GetNewTileSize());
    }

    public int GetCenteredX() => Window.ClientBounds.Width / 2 - (int)(Width / (float)2 * GetNewTileSize());

    public int GetCenteredY() => Window.ClientBounds.Height / 2 - (int)((Height + 5) / (float)2 * GetNewTileSize());

    public Rectangle GetDestinationRectangle(int x, int y)
    {
        var (nX, nY) = CalculateNewPos(x, y);
        var tileSize = GetNewTileSize();
        return new Rectangle(nX, nY, tileSize, tileSize);
    }

    public int GetNewTileSize()
    {
        var mX = Window.ClientBounds.Width / Width;
        var mY = Window.ClientBounds.Height / (Height + 5);
        return MathHelper.Min(mX, mY);
    }

    public Rectangle GetPlayerDestinationRectangle(Vector2 position)
    {
        var tileSize = GetNewTileSize();
        var m = GetMultiplier();
        return new Rectangle(GetCenteredX() + (int)(position.X * m), GetCenteredY() + (int)(position.Y * m), tileSize, tileSize);
    }

    public float GetMultiplier() => GetNewTileSize() / (float)TileSize;

    public bool IsSquare() => Width == Height + 5;

    public int StatLeftAlign() => IsSquare() ? 1 : Width / 5;

    public int StatRightAlign() => IsSquare() ? Width - 17 : Width * 3 / 5;
}
