using Microsoft.Xna.Framework.Input;

namespace GameProject;

public class ShopController
{
    public static void Update(PlayerModel player, ShopModel shop)
    {
        if (player.IsDead() || !player.ShowOptions) return;

        var state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.Q) && !shop.IsPressedUp)
        {
            if (shop.SelectorPos == 0) shop.SelectorPos = shop.Upgrades.Length - 1;
            else shop.SelectorPos--;
            shop.IsPressedUp = true;
        }
        if (state.IsKeyDown(Keys.E) && !shop.IsPressedDown)
        {
            if (shop.SelectorPos == shop.Upgrades.Length - 1) shop.SelectorPos = 0;
            else shop.SelectorPos++;
            shop.IsPressedDown = true;
        }
        if (state.IsKeyDown(Keys.Enter) && !shop.IsPressed)
        {
            var option = shop.Upgrades[shop.SelectorPos];
            if (player.CanPay(option[1]) && option[0] < shop.MaxUpgrade)
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
        if (state.IsKeyUp(Keys.Q) && shop.IsPressedUp)
            shop.IsPressedUp = false;
        if (state.IsKeyUp(Keys.E) && shop.IsPressedDown)
            shop.IsPressedDown = false;
        if (state.IsKeyUp(Keys.Enter) && shop.IsPressed)
            shop.IsPressed = false;
    }
}
