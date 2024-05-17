using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

public class ShopModel
{
    public MapModel Map { get; }
    public PlayerModel Helicopter { get; }
    public int[][] Upgrades { get; }
    public int MaxUpgrade { get; }
    public int SelectorPos { get; set; }
    public bool IsPressed { get; set; }
    public bool IsPressedUp { get; set; }
    public bool IsPressedDown { get; set; }

    public ShopModel(MapModel map, PlayerModel helicopter)
    {
        Map = map;
        Helicopter = helicopter;
        Upgrades = new int[][]
        {
            new[] { 0, 100 },
            new[] { 0, 100 },
        };
        MaxUpgrade = 99;
    }

    public void WriteUpgradeOption(SpriteBatch spriteBatch, string optionText, int pos, int optionNumber)
    {
        var upgrade = MathHelper.Min(Upgrades[optionNumber][0] + 1, MaxUpgrade);
        var text = $"{optionText} +{Upgrades[optionNumber][0]} -> +{upgrade}   Cost: {Upgrades[optionNumber][1]}";
        var color = SelectorPos == optionNumber ? Color.LightGreen : Color.White;
        spriteBatch.DrawString(Map.Font, text, Map.CalculateBottomTextPos(Map.Width - 17, pos),
            color, 0, Vector2.Zero, Map.GetMultiplier(), SpriteEffects.None, 0);
    }
}
