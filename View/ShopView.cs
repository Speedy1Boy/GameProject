using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

public class ShopView
{
    public static void Draw(SpriteBatch spriteBatch, Shop shop)
    {
        if (shop.Helicopter.ShowOptions)
        {
            shop.WriteUpgradeOption(spriteBatch, "HP:    ", 1, 0);
            shop.WriteUpgradeOption(spriteBatch, "Tank: ", 3, 1);
        }
    }
}
