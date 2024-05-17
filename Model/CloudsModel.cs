using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static GameProject.Utils;

namespace GameProject;

public class CloudsModel
{
    private readonly int rainProb;
    private readonly int thunderProb;

    public int Width { get; }
    public int Height { get; }
    public Tile[][] CloudMap { get; }
    public List<Texture2D> TextureList { get; }
    public int TileSize { get; }
    public MapModel MapModel { get; }

    public CloudsModel(MapModel mapModel, List<Texture2D> textureList)
    {
        Width = mapModel.Width;
        Height = mapModel.Height;
        TextureList = textureList;
        TileSize = 16;
        MapModel = mapModel;

        rainProb = 20;
        thunderProb = 30;

        CloudMap = new Tile[Height][];
        for (var j = 0; j < Height; j++)
        {
            CloudMap[j] = new Tile[Width];
            for (var i = 0; i < Width; i++)
                CloudMap[j][i].UpdateTile(i, j, 0, TileSize);
        }
        GenerateCloud(2.5);
    }

    public void GenerateCloud(double prob)
    {
        for (var i = 0; i < Width; i++)
            for (var j = 0; j < Height; j++)
                if (Rand(prob))
                {
                    CloudMap[j][i].UpdateTile(i, j, 1, TileSize);
                    if (Rand(rainProb))
                        CloudMap[j][i].UpdateTile(i, j, 2, TileSize);
                    if (CloudMap[j][i].ImageId == 2 && Rand(thunderProb))
                        CloudMap[j][i].UpdateTile(i, j, 3, TileSize);
                }
    }

    public void GenerateBorderClouds(int amount, Direction dir)
    {
        for (var i = 0; i < amount; i++)
        {
            var (x, y) = RandBorderCell(Width, Height, dir);
            CloudMap[y][x].UpdateTile(x, y, 1, TileSize);
            if (Rand(rainProb))
                CloudMap[y][x].UpdateTile(x, y, 2, TileSize);
            if (CloudMap[y][x].ImageId == 2 && Rand(thunderProb))
                CloudMap[y][x].UpdateTile(x, y, 3, TileSize);
        }
    }

    public void MoveCloud(Direction dir)
    {
        var clouds = new List<(int, int)>();
        for (var i = 0; i < Width; i++)
            for (var j = 0; j < Height; j++)
                if (CloudMap[j][i].ImageId != 0)
                    clouds.Add((i, j));
        foreach (var (x, y) in clouds)
        {
            var nX = x + moves[(int)dir].x;
            var nY = y + moves[(int)dir].y;
            if (0 <= nX && nX < Width && 0 <= nY && nY < Height)
                CloudMap[nY][nX] = CloudMap[y][x];
            else GenerateBorderClouds(RandNumber(3), dir);
            CloudMap[y][x].UpdateTile(x, y, 0, TileSize);
        }
    }

    public void ProcessCloud()
    {
        for (var i = 0; i < Width; i++)
            for (var j = 0; j < Height; j++)
                if (CloudMap[j][i].ImageId == 2 && MapModel.Grid[j][i].ImageId == 3)
                {
                    MapModel.Grid[j][i].UpdateTile(i, j, 4, Width);
                    MapModel.Grid[j][i].FireCounter = 0;
                    CloudMap[j][i].UpdateTile(i, j, 1, Height);
                }
    }

    public void ChangeCloudMapPos()
    {
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
            {
                var (nX, nY) = MapModel.CalculateNewPos(x, y);
                CloudMap[y][x].Collider = new Rectangle(nX, nY, TileSize, TileSize);
            }
    }
}
