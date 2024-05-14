namespace GameProject;

public class CloudsController
{
    public static void Update(long ticks, CloudMap clouds)
    {
        if (ticks % clouds.MapModel.WindPowerClouds() == 0)
            clouds.MoveCloud(clouds.MapModel.WindDirection);
        if (ticks % 25 == 0) clouds.ProcessCloud();
    }
}
