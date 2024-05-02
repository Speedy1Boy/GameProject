using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ReworkedGame;

internal class Player
{
    private readonly Texture2D texture;
    private Vector2 position;
    private Vector2 velocity;
    private float speed;
    private readonly int maxSpeed;
    private readonly Map map;

    public Player(Texture2D texture, Map map)
    {
        this.texture = texture;
        speed = 0;
        maxSpeed = 4;
        this.map = map;
    }

    public void Update()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            if (position.Y > 0)
            {
                velocity.Y -= 1;
                if (speed < maxSpeed) speed += 0.1f;
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            if (position.Y < (map.Size - 1) * map.TileSize)
            {
                velocity.Y += 1;
                if (speed < maxSpeed) speed += 0.1f;
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            if (position.X > 0)
            {
                velocity.X -= 1;
                if (speed < maxSpeed) speed += 0.1f;
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            if (position.X < (map.Size - 1) * map.TileSize)
            {
                velocity.X += 1;
                if (speed < maxSpeed) speed += 0.1f;
            }
        }
        if (Keyboard.GetState().IsKeyUp(Keys.W) &&
            Keyboard.GetState().IsKeyUp(Keys.S) &&
            Keyboard.GetState().IsKeyUp(Keys.A) &&
            Keyboard.GetState().IsKeyUp(Keys.D))
        {
            if (speed > 0) speed -= 0.1f;
            else if (speed >= float.MinValue) velocity = Vector2.Zero;
        }

        if (velocity != Vector2.Zero)
            velocity.Normalize();

        var oldPosition = position;
        position += velocity * speed;
        CheckCollisions(oldPosition);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, Color.White);
    }

    public void CheckCollisions(Vector2 oldPosition)
    {
        var playerCollider = new Rectangle((int)position.X, (int)position.Y, map.TileSize, map.TileSize);
        var newPos = position + velocity;
        var newRect = new Rectangle((int)newPos.X, (int)newPos.Y, map.TileSize, map.TileSize);
        for (var x = 0; x < map.Size; x++)
            for (var y = 0; y < map.Size; y++)
                if (map.Grid[x][y].ImageId == 5)
                {
                    var collider = map.Grid[x][y].Collider;
                    if (newPos.X != position.X)
                    {
                        newRect = new Rectangle((int)newPos.X, (int)position.Y, map.TileSize, map.TileSize);
                        if (newRect.Intersects(collider))
                        {
                            if (newPos.X > position.X)
                                newPos.X = collider.Left - playerCollider.Width;
                            else
                                newPos.X = collider.Right;
                            continue;
                        }
                    }
                    if (newPos.Y != position.Y)
                    {
                        newRect = new Rectangle((int)position.X, (int)newPos.Y, map.TileSize, map.TileSize);
                        if (newRect.Intersects(collider))
                        {
                            if (newPos.Y > position.Y)
                                newPos.Y = collider.Top - playerCollider.Height;
                            else
                                newPos.Y = collider.Bottom;
                        }
                    }

                }
        position = newPos;
    }
}
