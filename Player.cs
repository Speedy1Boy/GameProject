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
    private int tank;
    private int hp;
    private int money;

    public int MaxHP { get; set; }
    public int MaxTank { get; set; }
    public bool ShowOptions { get; private set; }

    public Vector2 Velocity
    {
        get => velocity;
        set
        {
            velocity = value;
            if (velocity.X > 4) velocity.X = 4;
            else if (velocity.X < -4) velocity.X = -4;
            else if (velocity.Y > 4) velocity.Y = 4;
            else if (velocity.Y < -4) velocity.Y = -4;
        }
    }

    public Player(Texture2D texture, Map map, int maxTank = 20, int maxHP = 20)
    {
        this.texture = texture;
        this.map = map;
        position = map.SpawnCoords;
        MaxTank = maxTank;
        tank = maxTank;
        MaxHP = maxHP;
        hp = maxHP;
        money = 2500;
    }

    public void Update(long ticks)
    {
        if (IsDead()) return;

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
            if (Velocity.Y > 0 && Velocity.Y < 0.1f) Velocity = new Vector2(Velocity.X, 0);
        }
        if (Keyboard.GetState().IsKeyUp(Keys.S))
        {
            if (Velocity.Y > 0) Velocity -= new Vector2(0, 0.1f);
            if (Velocity.Y < 0 && Velocity.Y > -0.1f) Velocity = new Vector2(Velocity.X, 0);
        }
        if (Keyboard.GetState().IsKeyUp(Keys.A))
        {
            if (Velocity.X < 0) Velocity += new Vector2(0.1f, 0);
            if (Velocity.X > 0 && Velocity.X < 0.1f) Velocity = new Vector2(0, Velocity.Y);
        }
        if (Keyboard.GetState().IsKeyUp(Keys.D))
        {
            if (Velocity.X > 0) Velocity -= new Vector2(0.1f, 0);
            if (Velocity.X < 0 && Velocity.X > -0.1f) Velocity = new Vector2(0, Velocity.Y);
        }

        if (ticks % 50 == 0) money++;

        position += Velocity;
        CheckCollisions(ticks);
        CheckCoordinatesInMap();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, Color.White);
        var stats = $"HP: {hp} / {MaxHP}, Tank: {tank} / {MaxTank}, Money: {money}";
        spriteBatch.DrawString(map.Font, stats, map.CalculateTextPos(1, 1), Color.White);
    }

    private void CheckCoordinatesInMap()
    {
        var mX = (map.Width - 1) * map.TileSize;
        var mY = (map.Height - 1) * map.TileSize;
        if (position.X < 0) position.X = 0;
        if (position.X > mX) position.X = mX;
        if (position.Y < 0) position.Y = 0;
        if (position.Y > mY) position.Y = mY;
    }

    private void CheckCollisions(long ticks)
    {
        var playerCollider = new Rectangle((int)position.X, (int)position.Y, map.TileSize, map.TileSize);
        for (var x = 0; x < map.Width; x++)
            for (var y = 0; y < map.Height; y++)
            {
                var objectCollider = map.Grid[y][x].Collider;
                if (!playerCollider.Intersects(objectCollider))
                {
                    if (map.Grid[y][x].ImageId == 9) ShowOptions = false;
                }
                if (playerCollider.Intersects(objectCollider))
                {
                    if (map.Grid[y][x].ImageId == 6)
                    {
                        position -= velocity;
                        Velocity = Vector2.Zero;
                    }
                    if (map.Grid[y][x].ImageId == 7 && !IsEmptyTank() && ticks % 5 == 0)
                    {
                        map.Grid[y][x].UpdateTile(x, y, 0, map.TileSize);
                        DropWater();
                        money += 20;
                    }
                    if (map.CloudMap[y][x].ImageId == 3 && ticks % 2 == 0) Damage();
                    if (map.Grid[y][x].ImageId == 8 && velocity == Vector2.Zero && ticks % 15 == 0) Heal();
                    if (map.Grid[y][x].ImageId == 9)
                    {
                        ShowOptions = true;
                        return;
                    }
                    if (map.Grid[y][x].ImageId == 5 && velocity == Vector2.Zero && ticks % 10 == 0)
                    {
                        AddWater();
                        return;
                    }
                }
            }
    }

    private void AddWater()
    {
        tank = MathHelper.Min(tank + 1, MaxTank);
    }

    private void DropWater()
    {
        if (!IsEmptyTank()) tank--;
    }

    private void Heal()
    {
        hp = MathHelper.Min(hp + 1, MaxHP);
    }

    private void Damage(int dmg = 1)
    {
        hp = MathHelper.Max(0, hp - dmg);
    }

    public void Pay(int cost)
    {
        if (CanPay(cost))
            money -= cost;
    }

    public bool IsEmptyTank() => tank == 0;

    public bool IsDead() => hp == 0;

    public bool CanPay(int cost) => money >= cost;
}
