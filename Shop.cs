using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject;

internal class Shop
{
    private readonly Map map;
    private readonly Player helicopter;
    private int selectorPos;

    public int[][] Upgrades { get; }

    public Shop(Map map, Player helicopter)
    {
        this.map = map;
        this.helicopter = helicopter;
        Upgrades = new int[][]
        {
            new[] { 0, 100 },
            new[] { 0, 100 },
        };
    }

    public void Update(long ticks)
    {
        if (helicopter.IsDead() || !helicopter.ShowOptions) return;

        if (Keyboard.GetState().IsKeyDown(Keys.Q) && ticks % 7 == 0)
        {
            if (selectorPos == 0) selectorPos = Upgrades.Length - 1;
            else selectorPos--;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.E) && ticks % 7 == 0)
        {
            if (selectorPos == Upgrades.Length - 1) selectorPos = 0;
            else selectorPos++;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && ticks % 7 == 0)
        {
            if (helicopter.CanPay(Upgrades[selectorPos][1]))
            {
                Upgrades[selectorPos][0]++;
                if (selectorPos == 0)
                    helicopter.MaxHP++;
                else
                    helicopter.MaxTank++;
                helicopter.Pay(Upgrades[selectorPos][1]);
                Upgrades[selectorPos][1] += 100;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (helicopter.ShowOptions)
        {
            WriteUpgradeOption(spriteBatch, "HP:    ", 1, 0);
            WriteUpgradeOption(spriteBatch, "Tank: ", 3, 1);
        }
    }

    private void WriteUpgradeOption(SpriteBatch spriteBatch, string optionText, int pos, int optionNumber)
    {
        var text = $"{optionText} +{Upgrades[optionNumber][0]} -> +{Upgrades[optionNumber][0] + 1}   Cost: {Upgrades[optionNumber][1]}";
        var color = selectorPos == optionNumber ? Color.LightGreen : Color.White;
        spriteBatch.DrawString(map.Font, text, map.CalculateTextPos(30, pos), color);
    }
}
