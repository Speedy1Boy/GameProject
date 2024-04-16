using System;

namespace GameProject;

public class GameplayPresenter
{
    private readonly IGameplayView _gameplayView;
    private readonly IGameplayModel _gameplayModel;

    public GameplayPresenter(IGameplayView gameplayView, IGameplayModel gameplayModel)
    {
        _gameplayView = gameplayView;
        _gameplayModel = gameplayModel;

        _gameplayView.CycleFinished += ViewModelUpdate;
        _gameplayView.PlayerMoved += ViewModelMovePlayer;
        _gameplayModel.Updated += ModelViewUpdate;

        _gameplayModel.Initalize();
    }

    private void ViewModelMovePlayer(object sender, ControlsEventArgs e)
    {
        _gameplayModel.MovePlayer(e.Direction);
    }

    private void ModelViewUpdate(object sender, GameplayEventArgs e)
    {
        _gameplayView.LoadGameCycleParameters(e.Objects);
    }

    private void ViewModelUpdate(object sender, EventArgs e)
    {
        _gameplayModel.Update();
    }

    public void LaunchGame() => _gameplayView.Run();
}
