using System;
using System.Collections.Generic;

namespace GameProject;

public interface IGameplayModel
{
    int PlayerId { get; set; }
    Dictionary<int, IObject> Objects { get; set; }
    event EventHandler<GameplayEventArgs> Updated;

    void Update();
    void MovePlayer(Direction dir);
    void Initalize();

    public enum Direction : byte
    {
        Left,
        Right,
        Up,
        Down
    }
}

public class GameplayEventArgs : EventArgs
{
    public Dictionary<int, IObject> Objects { get; set; }
}
