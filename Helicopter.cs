using Microsoft.Xna.Framework;

namespace GameProject;

public class Helicopter : IObject
{
    public int ImageId { get; set; }

    public Vector2 Pos { get; set; }
    public Vector2 Speed { get; set; }

    public void Update()
    {
        Pos += Speed;
    }
}
