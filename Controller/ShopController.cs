using Microsoft.Xna.Framework.Input;

namespace GameProject;

public class ShopController
{
    public static void Update(Player player, Shop shop)
    {
        if (player.IsDead() || !player.ShowOptions) return;

        if (Keyboard.GetState().IsKeyDown(Keys.Q) && !shop.IsPressedUp)
        {
            if (shop.SelectorPos == 0) shop.SelectorPos = shop.Upgrades.Length - 1;
            else shop.SelectorPos--;
            shop.IsPressedUp = true;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.E) && !shop.IsPressedDown)
        {
            if (shop.SelectorPos == shop.Upgrades.Length - 1) shop.SelectorPos = 0;
            else shop.SelectorPos++;
            shop.IsPressedDown = true;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !shop.IsPressed)
        {
            var option = shop.Upgrades[shop.SelectorPos];
            if (player.CanPay(option[1]))
            {
                option[0]++;
                if (shop.SelectorPos == 0)
                    player.MaxHP++;
                else
                    player.MaxTank++;
                player.Pay(option[1]);
                option[1] += 100;
            }
            shop.IsPressed = true;
        }
        if (Keyboard.GetState().IsKeyUp(Keys.Q) && shop.IsPressedUp)
            shop.IsPressedUp = false;
        if (Keyboard.GetState().IsKeyUp(Keys.E) && shop.IsPressedDown)
            shop.IsPressedDown = false;
        if (Keyboard.GetState().IsKeyUp(Keys.Enter) && shop.IsPressed)
            shop.IsPressed = false;
    }
}
