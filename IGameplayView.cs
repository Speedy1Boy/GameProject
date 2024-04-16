using System;
using System.Collections.Generic;

namespace GameProject;

public interface IGameplayView
{
    event EventHandler CycleFinished;
    event EventHandler<ControlsEventArgs> PlayerMoved;

    void LoadGameCycleParameters(Dictionary<int, IObject> objects);
    void Run();
}

public class ControlsEventArgs : EventArgs
{
    public IGameplayModel.Direction Direction { get; set; }
}
