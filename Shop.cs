using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject;

internal class Shop
{
    private readonly Map map;
    private readonly Player helicopter;
    private int selectorPos;
    private bool isPressedUp;
    private bool isPressedDown;

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

    public void Update()
    {
        if (helicopter.IsDead() || !helicopter.ShowOptions) return;

        if (Keyboard.GetState().IsKeyDown(Keys.Q) && !isPressedUp)
        {
            if (selectorPos == 0) selectorPos = Upgrades.Length - 1;
            else selectorPos--;
            isPressedUp = true;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.E) && !isPressedDown)
        {
            if (selectorPos == Upgrades.Length - 1) selectorPos = 0;
            else selectorPos++;
            isPressedDown = true;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            var option = Upgrades[selectorPos];
            if (helicopter.CanPay(option[1]))
            {
                option[0]++;
                if (selectorPos == 0)
                    helicopter.MaxHP++;
                else
                    helicopter.MaxTank++;
                helicopter.Pay(option[1]);
                option[1] += 100;
            }
        }
        if (Keyboard.GetState().IsKeyUp(Keys.Q) && isPressedUp)
            isPressedUp = false;
        if (Keyboard.GetState().IsKeyUp(Keys.E) && isPressedDown)
            isPressedDown = false;
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
