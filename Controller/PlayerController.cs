using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject;

public class PlayerController
{
    public static void Update(long ticks, Player player)
    {
        if (player.IsDead()) return;

        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            player.Velocity -= new Vector2(0, 0.1f);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            player.Velocity += new Vector2(0, 0.1f);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            player.Velocity -= new Vector2(0.1f, 0);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            player.Velocity += new Vector2(0.1f, 0);
        }

        if (Keyboard.GetState().IsKeyUp(Keys.W))
        {
            if (player.Velocity.Y < 0) player.Velocity += new Vector2(0, 0.1f);
            if (player.Velocity.Y > 0 && player.Velocity.Y < 0.1f) player.Velocity = new Vector2(player.Velocity.X, 0);
        }
        if (Keyboard.GetState().IsKeyUp(Keys.S))
        {
            if (player.Velocity.Y > 0) player.Velocity -= new Vector2(0, 0.1f);
            if (player.Velocity.Y < 0 && player.Velocity.Y > -0.1f) player.Velocity = new Vector2(player.Velocity.X, 0);
        }
        if (Keyboard.GetState().IsKeyUp(Keys.A))
        {
            if (player.Velocity.X < 0) player.Velocity += new Vector2(0.1f, 0);
            if (player.Velocity.X > 0 && player.Velocity.X < 0.1f) player.Velocity = new Vector2(0, player.Velocity.Y);
        }
        if (Keyboard.GetState().IsKeyUp(Keys.D))
        {
            if (player.Velocity.X > 0) player.Velocity -= new Vector2(0.1f, 0);
            if (player.Velocity.X < 0 && player.Velocity.X > -0.1f) player.Velocity = new Vector2(0, player.Velocity.Y);
        }

        if (ticks % 50 == 0) player.Money++;

        player.Position += player.Velocity;
        player.CheckCollisions(ticks);
        player.CheckCoordinatesInMap();
    }
}
