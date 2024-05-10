using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using static GameProject.Utils;

namespace GameProject;

internal class CloudMap
{
    private readonly int width;
    private readonly int height;
    private readonly List<Texture2D> textureList;
    private readonly int tileSize;
    private readonly int rainProb;
    private readonly int thunderProb;

    public Tile[][] Map { get; }

    public CloudMap(int width, int height, List<Texture2D> textureList)
    {
        this.width = width;
        this.height = height;
        this.textureList = textureList;
        tileSize = 16;

        rainProb = 20;
        thunderProb = 30;

        Map = new Tile[height][];
        for (var j = 0; j < height; j++)
        {
            Map[j] = new Tile[width];
            for (var i = 0; i < width; i++)
                Map[j][i].UpdateTile(i, j, 0, tileSize);
        }
        GenerateCloud(2.5);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                spriteBatch.Draw(textureList[Map[j][i].ImageId], new Vector2(i * tileSize, j * tileSize), Color.White);
    }

    public void GenerateCloud(double prob)
    {
        for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (Rand(prob))
                {
                    Map[j][i].UpdateTile(i, j, 1, tileSize);
                    if (Rand(rainProb))
                        Map[j][i].UpdateTile(i, j, 2, tileSize);
                    if (Map[j][i].ImageId == 2 && Rand(thunderProb))
                        Map[j][i].UpdateTile(i, j, 3, tileSize);
                }
    }

    public void GenerateBorderCloud(int amount, Direction dir)
    {
        for (var i = 0; i < amount; i++)
        {
            var (x, y) = RandBorderCell(width, height, dir);
            Map[y][x].UpdateTile(x, y, 1, tileSize);
            if (Rand(rainProb))
                Map[y][x].UpdateTile(x, y, 2, tileSize);
            if (Map[y][x].ImageId == 2 && Rand(thunderProb))
                Map[y][x].UpdateTile(x, y, 3, tileSize);
        }
    }

    public void MoveCloud(Direction dir)
    {
        var clouds = new List<(int, int)>();
        for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                if (Map[j][i].ImageId != 0)
                    clouds.Add((i, j));
        foreach (var (x, y) in clouds)
        {
            var nX = x + moves[(int)dir].x;
            var nY = y + moves[(int)dir].y;
            if (0 <= nX && nX < width && 0 <= nY && nY < height)
                Map[nY][nX] = Map[y][x];
            else GenerateBorderCloud(RandNumber(3), dir);
            Map[y][x].UpdateTile(x, y, 0, tileSize);
        }
    }
}
