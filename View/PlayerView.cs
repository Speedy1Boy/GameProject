using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

public class PlayerView
{
    public static void Draw(SpriteBatch spriteBatch, PlayerModel player)
    {
        var texture = player.Direction == Direction.Left ? player.Textures[0] : player.Textures[1];
        spriteBatch.Draw(texture, player.Map.GetPlayerDestinationRectangle(player.Position), Color.White);
        var stats = $"HP: {player.Hp} / {player.MaxHP}, Tank: {player.Tank} / {player.MaxTank}, Money: {player.Money}";
        spriteBatch.DrawString(player.Map.Font, stats, player.Map.CalculateBottomTextPos(player.Map.StatLeftAlign(), 1),
            Color.White, 0, Vector2.Zero, player.Map.GetMultiplier(), SpriteEffects.None, 0);
    }
}
