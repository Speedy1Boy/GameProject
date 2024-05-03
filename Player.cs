using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject;

internal class Player
{
    private readonly Texture2D texture;
    private readonly Map map;
    private Vector2 position;
    private Vector2 velocity;
    public Vector2 Velocity
    {
        get { return velocity; }
        set
        {
            velocity = value;
            if (velocity.X > 4) velocity.X = 4;
            else if (velocity.X < -4) velocity.X = -4;
            else if (velocity.Y > 4) velocity.Y = 4;
            else if (velocity.Y < -4) velocity.Y = -4;
        }
    }

    public Player(Texture2D texture, Map map)
    {
        this.texture = texture;
        this.map = map;
    }

    public void Update()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            Velocity -= new Vector2(0, 0.1f);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            Velocity += new Vector2(0, 0.1f);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            Velocity -= new Vector2(0.1f, 0);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            Velocity += new Vector2(0.1f, 0);
        }

        if (Keyboard.GetState().IsKeyUp(Keys.W))
        {
            if (Velocity.Y < 0) Velocity += new Vector2(0, 0.1f);
        }
        if (Keyboard.GetState().IsKeyUp(Keys.S))
        {
            if (Velocity.Y > 0) Velocity -= new Vector2(0, 0.1f);
            if (Velocity.Y < 0 && Velocity.Y > -0.1f) Velocity = new Vector2(Velocity.X, 0);
        }
        if (Keyboard.GetState().IsKeyUp(Keys.A))
        {
            if (Velocity.X < 0) Velocity += new Vector2(0.1f, 0);
        }
        if (Keyboard.GetState().IsKeyUp(Keys.D))
        {
            if (Velocity.X > 0) Velocity -= new Vector2(0.1f, 0);
            if (Velocity.X < 0 && Velocity.X > -0.1f) Velocity = new Vector2(0, Velocity.Y);
        }

        position += Velocity;
        CheckCollisions();
        CheckCoordinatesInMap();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, Color.White);
    }

    public void CheckCoordinatesInMap()
    {
        var maxCord = (map.Size - 1) * map.TileSize;
        if (position.X < 0) position.X = 0;
        if (position.X > maxCord) position.X = maxCord;
        if (position.Y < 0) position.Y = 0;
        if (position.Y > maxCord) position.Y = maxCord;
    }

    public void CheckCollisions()
    {
        var playerCollider = new Rectangle((int)position.X, (int)position.Y, map.TileSize, map.TileSize);
        for (var x = 0; x < map.Size; x++)
            for (var y = 0; y < map.Size; y++)
            {
                var objectCollider = map.Grid[x][y].Collider;
                if (map.Grid[x][y].ImageId == 6)
                {
                    if (playerCollider.Intersects(objectCollider))
                    {
                        position -= velocity;
                        Velocity = Vector2.Zero;
                    }
                }
                if (map.Grid[x][y].ImageId == 7)
                {
                    if (playerCollider.Intersects(objectCollider))
                    {
                        map.Grid[x][y].UpdateTile(x, y, 0, map.TileSize);
                    }
                }
            }
    }
}
