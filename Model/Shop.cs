using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

public class Shop
{
    public MapModel Map { get; }
    public Player Helicopter { get; }
    public int[][] Upgrades { get; }
    public int SelectorPos { get; set; }
    public bool IsPressed { get; set; }
    public bool IsPressedUp { get; set; }
    public bool IsPressedDown { get; set; }

    public Shop(MapModel map, Player helicopter)
    {
        Map = map;
        Helicopter = helicopter;
        Upgrades = new int[][]
        {
            new[] { 0, 100 },
            new[] { 0, 100 },
        };
    }

    public void WriteUpgradeOption(SpriteBatch spriteBatch, string optionText, int pos, int optionNumber)
    {
        var text = $"{optionText} " +
            $"+{Upgrades[optionNumber][0]} -> +{Upgrades[optionNumber][0] + 1}   " +
            $"Cost: {Upgrades[optionNumber][1]}";
        var color = SelectorPos == optionNumber ? Color.LightGreen : Color.White;
        spriteBatch.DrawString(Map.Font, text, Map.CalculateTextPos(30, pos), color);
    }
}
