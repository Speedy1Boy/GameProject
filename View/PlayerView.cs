using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

public class PlayerView
{
    public static void Draw(SpriteBatch spriteBatch, Player player)
    {
        spriteBatch.Draw(player.Texture, player.Position, Color.White);
        var stats = $"HP: {player.Hp} / {player.MaxHP}, Tank: {player.Tank} / {player.MaxTank}, Money: {player.Money}";
        spriteBatch.DrawString(player.Map.Font, stats, player.Map.CalculateTextPos(1, 1), Color.White);
    }
}
