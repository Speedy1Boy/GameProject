using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameProject;

public class PlayerController
{
    public static void Update(long ticks, PlayerModel player)
    {
        if (player.IsDead()) return;

        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.W))
        {
            player.Velocity -= new Vector2(0, 0.1f);
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            player.Velocity += new Vector2(0, 0.1f);
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
            player.Velocity -= new Vector2(0.1f, 0);
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
            player.Velocity += new Vector2(0.1f, 0);
        }

        if (keyboardState.IsKeyUp(Keys.W))
        {
            if (player.Velocity.Y < 0) player.Velocity += new Vector2(0, 0.1f);
            if (player.CheckPositiveVelocity(player.Velocity.Y)) player.Velocity = new Vector2(player.Velocity.X, 0);
        }
        if (keyboardState.IsKeyUp(Keys.S))
        {
            if (player.Velocity.Y > 0) player.Velocity -= new Vector2(0, 0.1f);
            if (player.CheckNegativeVelocity(player.Position.Y)) player.Velocity = new Vector2(player.Velocity.X, 0);
        }
        if (keyboardState.IsKeyUp(Keys.A))
        {
            if (player.Velocity.X < 0) player.Velocity += new Vector2(0.1f, 0);
            if (player.CheckPositiveVelocity(player.Velocity.X)) player.Velocity = new Vector2(0, player.Velocity.Y);
        }
        if (keyboardState.IsKeyUp(Keys.D))
        {
            if (player.Velocity.X > 0) player.Velocity -= new Vector2(0.1f, 0);
            if (player.CheckNegativeVelocity(player.Position.X)) player.Velocity = new Vector2(0, player.Velocity.Y);
        }

        if (ticks % 50 == 0) player.Money++;

        player.Position += player.Velocity;

        player.CheckCollisions(ticks);
        player.CheckCoordinatesInMap();
    }
}
