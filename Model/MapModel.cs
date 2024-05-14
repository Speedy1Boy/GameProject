﻿using Microsoft.Xna.Framework;
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
    public int WindPower { get; set; }
    public Direction WindDirection { get; set; }

    public MapModel(int width, int height, List<Texture2D> tiles, SpriteFont font)
    {
        Width = width;
        Height = height;
        Tiles = tiles;
        TileSize = 16;
        Font = font;

        Grid = new Tile[height][];
        for (var y = 0; y < height; y++)
        {
            Grid[y] = new Tile[width];
            for (var x = 0; x < width; x++)
                Grid[y][x].UpdateTile(x, y, 0, TileSize);
        }

        GenerateForest(85);
        for (var i = 0; i < 5; i++) GenerateLake(100);
        for (var i = 0; i < 7; i++) GenerateMountain(50);
        GenerateStraightRiver(RandDir());
        GenerateNearRiver(8);
        SpawnCoords = GenerateNearRiver(9);

        WindDirection = RandDir();
        WindPower = RandNumber(10);
    }

    public void GenerateForest(double prob)
    {
        for (var i = 0; i < Width; i++)
            for (var j = 0; j < Height; j++)
                if (Rand(prob))
                    Grid[j][i].UpdateTile(i, j, RandNumber(3) + 2, TileSize);
    }

    public void GenerateLake(int length = 1)
    {
        var (rX, rY) = RandCell(Width, Height);
        Grid[rY][rX].UpdateTile(rX, rY, 5, TileSize);
        for (var i = 0; i < length; i++)
        {
            (rX, rY) = Neighbor(rX, rY);
            if (CheckBounds(rX, rY, Width, Height))
                Grid[rY][rX].UpdateTile(rX, rY, 5, TileSize);
            else break;
        }
    }

    public void GenerateMountain(int length = 1)
    {
        var (rX, rY) = RandCell(Width, Height);
        Grid[rY][rX].UpdateTile(rX, rY, 6, TileSize);
        for (var i = 0; i < length; i++)
        {
            (rX, rY) = Neighbor(rX, rY);
            if (CheckBounds(rX, rY, Width, Height))
                Grid[rY][rX].UpdateTile(rX, rY, 6, TileSize);
            else break;
        }
    }

    public void GenerateStraightRiver(Direction dir)
    {
        var (rX, rY) = RandBorderCell(Width, Height, GetOppositDir(dir));
        Grid[rY][rX].UpdateTile(rX, rY, 5, TileSize);
        while (true)
        {
            (rX, rY) = RiverNeighbor(rX, rY, (int)dir);
            if (CheckBounds(rX, rY, Width, Height))
                Grid[rY][rX].UpdateTile(rX, rY, 5, TileSize);
            else break;
        }
    }

    public void StartFire()
    {
        var (x, y) = RandCell(Width, Height);
        var tileId = Grid[y][x].ImageId;
        if (2 <= tileId && tileId <= 4)
            Grid[y][x].UpdateTile(x, y, 7, TileSize);
    }

    public bool CanBurn(int x, int y) => Grid[y][x].ImageId >= 2 && Grid[y][x].ImageId <= 4;

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
                if (Grid[y][x].ImageId == 7)
                {
                    fire.Add((x, y));
                    Grid[y][x].UpdateTile(x, y, 7, TileSize);
                    Grid[y][x].FireCounter++;
                    if (Grid[y][x].FireCounter > 15)
                        Grid[y][x].UpdateTile(x, y, 1, TileSize);
                }
        foreach (var (x, y) in fire)
            for (var i = 0; i < 4; i++)
            {
                var nX = x + moves[i].x;
                var nY = y + moves[i].y;
                if (Rand(10 * Wind((Direction)i)) && CheckBounds(nX, nY, Width, Height) && CanBurn(nX, nY))
                {
                    Grid[nY][nX].UpdateTile(nX, nY, 7, TileSize);
                    Grid[nY][nX].FireCounter++;
                }
            }
    }

    public void GenerateTree()
    {
        var (x, y) = RandCell(Width, Height);
        if (Grid[y][x].ImageId == 0)
            Grid[y][x].UpdateTile(x, y, RandNumber(3) + 2, TileSize);
    }

    public void UpdateGrass()
    {
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (Grid[y][x].ImageId == 0)
                    for (var i = 0; i < 4; i++)
                    {
                        var nX = x + moves[i].x;
                        var nY = y + moves[i].y;
                        if (Rand(15) && CheckBounds(nX, nY, Width, Height) && Grid[nY][nX].ImageId == 1)
                            Grid[nY][nX].UpdateTile(nX, nY, 0, TileSize);
                    }
    }

    public int WindPowerClouds() => 20 + (9 - WindPower) * 3;

    public Vector2 GenerateNearRiver(int id)
    {
        var river = new List<(int, int)>();
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (Grid[y][x].ImageId == 5)
                    river.Add((x, y));
        var (rX, rY) = river[RandNumber(river.Count)];
        Grid[rY][rX].UpdateTile(rX, rY, id, TileSize);
        return new Vector2(rX * TileSize, rY * TileSize);
    }

    public Vector2 CalculateTextPos(int x, int y)
    {
        return new Vector2(x * TileSize, Height * TileSize + y * TileSize - 2);
    }
}