using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameProject;

public class GameCycle : IGameplayModel
{
    public event EventHandler<GameplayEventArgs> Updated = delegate { };

    public int PlayerId { get; set; }

    public Dictionary<int, IObject> Objects { get; set; }

    public void MovePlayer(IGameplayModel.Direction dir)
    {
        var p = Objects[PlayerId] as Helicopter;
        switch (dir)
        {
            case IGameplayModel.Direction.Up:
                p.Speed += new Vector2(0, -1);
                break;
            case IGameplayModel.Direction.Down:
                p.Speed += new Vector2(0, 1);
                break;
            case IGameplayModel.Direction.Left:
                p.Speed += new Vector2(-1, 0);
                break;
            case IGameplayModel.Direction.Right:
                p.Speed += new Vector2(1, 0);
                break;
        }
    }

    public void Update()
    {
        foreach (var obj in Objects.Values)
            obj.Update();
        Updated.Invoke(this, new GameplayEventArgs { Objects = Objects });
    }

    public void Initalize()
    {
        Objects = new Dictionary<int, IObject>();
        var player = new Helicopter
        {
            Pos = new Vector2(0, 0),
            ImageId = 1,
            Speed = new Vector2(0, 0)
        };
        Objects.Add(1, player);
        PlayerId = 1;
    }
}
