namespace GameProject;

public class CloudsController
{
    public static void Update(long ticks, CloudsModel clouds)
    {
        if (ticks % clouds.MapModel.WindPowerCloudsUpdate() == 0)
            clouds.MoveCloud(clouds.MapModel.WindDirection);
        if (ticks % 25 == 0) clouds.ProcessCloud();
    }
}
