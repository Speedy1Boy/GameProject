using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static GameProject.Utils;

namespace GameProject;

public class CloudMap
{
    private readonly int rainProb;
    private readonly int thunderProb;

    public int Width { get; }
    public int Height { get; }
    public Tile[][] Map { get; }
    public List<Texture2D> TextureList { get; }
    public int TileSize { get; }
    public MapModel MapModel { get; }

    public CloudMap(MapModel mapModel, List<Texture2D> textureList)
    {
        Width = mapModel.Width;
        Height = mapModel.Height;
        TextureList = textureList;
        TileSize = 16;
        MapModel = mapModel;

        rainProb = 20;
        thunderProb = 30;

        Map = new Tile[Height][];
        for (var j = 0; j < Height; j++)
        {
            Map[j] = new Tile[Width];
            for (var i = 0; i < Width; i++)
                Map[j][i].UpdateTile(i, j, 0, TileSize);
        }
        GenerateCloud(2.5);
    }

    public void GenerateCloud(double prob)
    {
        for (var i = 0; i < Width; i++)
            for (var j = 0; j < Height; j++)
                if (Rand(prob))
                {
                    Map[j][i].UpdateTile(i, j, 1, TileSize);
                    if (Rand(rainProb))
                        Map[j][i].UpdateTile(i, j, 2, TileSize);
                    if (Map[j][i].ImageId == 2 && Rand(thunderProb))
                        Map[j][i].UpdateTile(i, j, 3, TileSize);
                }
    }

    public void GenerateBorderCloud(int amount, Direction dir)
    {
        for (var i = 0; i < amount; i++)
        {
            var (x, y) = RandBorderCell(Width, Height, dir);
            Map[y][x].UpdateTile(x, y, 1, TileSize);
            if (Rand(rainProb))
                Map[y][x].UpdateTile(x, y, 2, TileSize);
            if (Map[y][x].ImageId == 2 && Rand(thunderProb))
                Map[y][x].UpdateTile(x, y, 3, TileSize);
        }
    }

    public void MoveCloud(Direction dir)
    {
        var clouds = new List<(int, int)>();
        for (var i = 0; i < Width; i++)
            for (var j = 0; j < Height; j++)
                if (Map[j][i].ImageId != 0)
                    clouds.Add((i, j));
        foreach (var (x, y) in clouds)
        {
            var nX = x + moves[(int)dir].x;
            var nY = y + moves[(int)dir].y;
            if (0 <= nX && nX < Width && 0 <= nY && nY < Height)
                Map[nY][nX] = Map[y][x];
            else GenerateBorderCloud(RandNumber(3), dir);
            Map[y][x].UpdateTile(x, y, 0, TileSize);
        }
    }

    public void ProcessCloud()
    {
        for (var i = 0; i < Width; i++)
            for (var j = 0; j < Height; j++)
                if (Map[j][i].ImageId == 2 && MapModel.Grid[j][i].ImageId == 7)
                {
                    MapModel.Grid[j][i].UpdateTile(i, j, 0, Width);
                    Map[j][i].UpdateTile(i, j, 1, Height);
                }
    }
}
