using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject;

public class MapView
{
    public static void Draw(SpriteBatch spriteBatch, MapModel map)
    {
        for (var i = 0; i < map.Width; i++)
        {
            for (var j = 0; j < map.Height; j++)
                spriteBatch.Draw(map.Tiles[map.Grid[j][i].ImageId],
                    map.CalculateNewVectorPos(i, j),
                    Color.White);

            for (var k = 0; k < 5; k++)
                spriteBatch.Draw(map.Tiles[0],
                    map.CalculateNewVectorPos(i, map.Height + k),
                    Color.DarkSlateGray);
        }

        var stats = $"Wind direction is {map.WindDirection}, wind power is {map.WindPower + 1}";
        spriteBatch.DrawString(map.Font, stats, map.CalculateBottomTextPos(1, 3), Color.White);
    }
}
