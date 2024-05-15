using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

public class CloudsView
{
    public static void Draw(SpriteBatch spriteBatch, CloudsModel clouds)
    {
        for (var i = 0; i < clouds.Width; i++)
            for (var j = 0; j < clouds.Height; j++)
                spriteBatch.Draw(clouds.TextureList[clouds.CloudMap[j][i].ImageId],
                    new Vector2(i * clouds.TileSize, j * clouds.TileSize),
                    Color.White);
    }
}
