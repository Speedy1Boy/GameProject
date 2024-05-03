using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using static GameProject.Utils;
using Color = Microsoft.Xna.Framework.Color;

namespace GameProject;

internal class Clouds
{
    private int size;
    private int tileSize;
    private List<Texture2D> textureList;
    private int rainCH;
    private int thunderCH;
    public Tile[][] cloudMap;

    public Clouds(int size, List<Texture2D> textureList)
    {
        this.size = size;
        this.textureList = textureList;
        tileSize = 16;

        rainCH = 20;
        thunderCH = 30;

        cloudMap = new Tile[size][];
        for (var i = 0; i < size; i++)
        {
            cloudMap[i] = new Tile[size];
            for (var j = 0; j < size; j++)
                cloudMap[i][j].ImageId = 0;
        }
        GenerateCloud(2.5);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
                spriteBatch.Draw(textureList[cloudMap[i][j].ImageId], new Vector2(i * tileSize, j * tileSize), Color.White);
    }

    public void GenerateCloud(double prob)
    {
        for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
                if (Rand(prob))
                {
                    cloudMap[i][j].UpdateTile(i, j, 1, tileSize);
                    if (Rand(rainCH))
                        cloudMap[i][j].UpdateTile(i, j, 2, tileSize);
                    if (cloudMap[i][j].ImageId == 2 && Rand(thunderCH))
                        cloudMap[i][j].UpdateTile(i, j, 3, tileSize);
                }
    }

    public void GenerateCloudDir(int amount, Direction dir)
    {
        for (var i = 0; i < amount; i++)
        {
            var (x, y) = RandCellBorderDir(size, size, dir);
            cloudMap[x][y].UpdateTile(x, y, 1, tileSize);
            if (Rand(rainCH))
                cloudMap[x][y].UpdateTile(x, y, 2, tileSize);
            if (cloudMap[x][y].ImageId == 2 && Rand(thunderCH))
                cloudMap[x][y].UpdateTile(x, y, 3, tileSize);
        }
    }

    public void MoveCloud(Direction dir)
    {
        var clouds = new List<(int, int)>();
        for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
                if (cloudMap[i][j].ImageId != 0)
                    clouds.Add((i, j));
        foreach (var (x, y) in clouds)
        {
            var move = moves[(int)dir];
            if (0 <= x + move.x && x + move.x < size && 0 <= y + move.y && y + move.y < size)
                cloudMap[x + move.x][y + move.y] = cloudMap[x][y];
            else GenerateCloudDir(RandSymbol(3), dir);
            cloudMap[x][y].UpdateTile(x, y, 0, tileSize);
        }
    }
}
