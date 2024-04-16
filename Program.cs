using System;

namespace GameProject;

public static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        //using var game = new GameCycleView();
        //game.Run();
        var g = new GameplayPresenter(new GameCycleView(), new GameCycle());
        g.LaunchGame();
    }
}
