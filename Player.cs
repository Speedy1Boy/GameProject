using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ReworkedGame;

internal class Player
{
    private Texture2D texture;
    private Vector2 position;
    private Vector2 velocity;
    private float speed;
    private Tile[][] map;
    private Rectangle collider;

    public Player(Texture2D texture, Map map)
    {
        this.texture = texture;
        speed = 2;
        collider = new Rectangle((int)position.X, (int)position.Y, 16, 16);
        this.map = map.map;
    }

    public void Update()
    {
        var oldPosition = position;
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            if (position.Y > 0) velocity.Y -= 1;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            if (position.Y < 784) velocity.Y += 1;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            if (position.X > 0) velocity.X -= 1;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            if (position.X < 784) velocity.X += 1;
        }
        if (velocity != Vector2.Zero)
            velocity.Normalize();

        position += velocity * speed;
        if (IsCollided()) position = oldPosition;
        velocity = Vector2.Zero;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, Color.White);
    }

    public void MoveCollider()
    {
        collider = new Rectangle((int)position.X, (int)position.Y, 16, 16);
    }

    public bool IsCollided()
    {
        foreach (var d in moves)
            if (map[(int)(position.X + d.Item1) / 16][(int)(position.Y + d.Item2) / 16].imageId == 5)
            return true;
        return false;
    }

    private readonly (int, int)[] moves = new[] { (0, 0), (15, 0), (0, 15), (15, 15) };
}
