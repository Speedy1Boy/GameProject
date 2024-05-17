using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

public class PlayerModel
{
    private Vector2 position;
    private Vector2 velocity;

    public Texture2D[] Textures { get; }
    public MapModel Map { get; }
    public CloudsModel Clouds { get; }
    public int MaxTank { get; set; }
    public int Tank { get; set; }
    public int MaxHP { get; set; }
    public int Hp { get; set; }
    public int Money { get; set; }
    public bool ShowOptions { get; private set; }
    public Direction Direction { get; set; }

    public Vector2 Position { get => position; set => position = value; }
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

    public PlayerModel(Texture2D[] textures, CloudsModel clouds, int maxTank = 20, int maxHP = 20)
    {
        Textures = textures;
        Clouds = clouds;
        Map = clouds.MapModel;
        position = Map.SpawnCoords;
        MaxTank = maxTank;
        Tank = maxTank;
        MaxHP = maxHP;
        Hp = maxHP;
        Money = 2500;
    }

    public void CheckCollisions(long ticks)
    {
        var windowPos = Map.CalculateHelicopterWindowPos(position);
        var playerCollider = new Rectangle((int)windowPos.X + 1, (int)windowPos.Y + 1, Map.TileSize - 1, Map.TileSize - 1);
        for (var x = 0; x < Map.Width; x++)
            for (var y = 0; y < Map.Height; y++)
            {
                var objectCollider = Map.Grid[y][x].Collider;
                if (!playerCollider.Intersects(objectCollider))
                {
                    if (Map.Grid[y][x].ImageId == 6) ShowOptions = false;
                }
                if (playerCollider.Intersects(objectCollider))
                {
                    if (Map.Grid[y][x].ImageId == 1)
                    {
                        position -= velocity;
                        Velocity = Vector2.Zero;
                    }
                    if (Map.Grid[y][x].ImageId == 3 && !IsEmptyTank() && ticks % 5 == 0)
                    {
                        Map.Grid[y][x].UpdateTile(x, y, 4, Map.TileSize);
                        Map.Grid[y][x].FireCounter = 0;
                        DropWater();
                        Money += 20;
                    }
                    if (Clouds.CloudMap[y][x].ImageId == 3 && ticks % 2 == 0) Damage();
                    if (Map.Grid[y][x].ImageId == 5 && velocity == Vector2.Zero && ticks % 15 == 0) Heal();
                    if (Map.Grid[y][x].ImageId == 6)
                    {
                        ShowOptions = true;
                        return;
                    }
                    if (Map.Grid[y][x].ImageId == 10 && velocity == Vector2.Zero && ticks % 10 == 0)
                    {
                        AddWater();
                        return;
                    }
                }
            }
    }

    public void CheckCoordinatesInMap()
    {
        var mX = (Map.Width - 1) * Map.TileSize;
        var mY = (Map.Height - 1) * Map.TileSize;
        if (Position.X < 0) position.X = 0;
        if (Position.X > mX) position.X = mX;
        if (Position.Y < 0) position.Y = 0;
        if (Position.Y > mY) position.Y = mY;
    }

    private void AddWater()
    {
        Tank = MathHelper.Min(Tank + 1, MaxTank);
    }

    private void DropWater()
    {
        if (!IsEmptyTank()) Tank--;
    }

    private void Heal()
    {
        Hp = MathHelper.Min(Hp + 1, MaxHP);
    }

    private void Damage(int dmg = 1)
    {
        Hp = MathHelper.Max(0, Hp - dmg);
    }

    public void Pay(int cost)
    {
        if (CanPay(cost))
            Money -= cost;
    }

    public bool IsEmptyTank() => Tank == 0;

    public bool IsDead() => Hp == 0;

    public bool CanPay(int cost) => Money >= cost;

    public static bool CheckPositiveVelocity(float n) => n > 0 && n < 0.1f;

    public static bool CheckNegativeVelocity(float n) => n < 0 && n > -0.1f;
}
