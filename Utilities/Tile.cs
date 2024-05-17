using Microsoft.Xna.Framework;

namespace GameProject;

public struct Tile
{
    public int ImageId { get; private set; }
    public Rectangle Collider { get; set; }
    public int FireCounter { get; set; }

    public void UpdateTile(int x, int y, int id, int tileSize)
    {
        ImageId = id;
        Collider = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
    }
}
