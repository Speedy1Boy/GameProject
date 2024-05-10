using Microsoft.Xna.Framework;

namespace GameProject;

struct Tile
{
    public int ImageId { get; private set; }
    public Rectangle Collider { get; private set; }
    public int FireCounter { get; set; }

    public void UpdateTile(int x, int y, int id, int tileSize)
    {
        ImageId = id;
        Collider = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
    }
}
