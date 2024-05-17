using Microsoft.Xna.Framework;
using static GameProject.Utils;

namespace GameProject;

public class MapController
{
    public static void Update(long ticks, MapModel map)
    {
        if (ticks % 250 == 0) map.GenerateTree();
        if (ticks % 500 == 0) map.UpdateGrass();
        if (ticks % 250 == 0) map.StartFire();
        if (ticks % 200 == 0) map.UpdateFire();

        if (ticks % 2500 == 0)
        {
            map.WindDirection = RandDir();
            map.WindPower = RandNumber(10);
        }
        map.ChangeMapPos();
    }
}
