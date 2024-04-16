using Microsoft.Xna.Framework;

namespace GameProject;

public interface IObject
{
    int ImageId { get; set; }

    Vector2 Pos { get; }

    void Update();
}
