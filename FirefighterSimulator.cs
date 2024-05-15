using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace GameProject;

public class FirefighterSimulator : Game
{
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private SpriteFont minecraftFont;
    private readonly List<Texture2D> mapTiles = new();
    private readonly List<Texture2D> cloudTiles = new();
    private MapModel map;
    private PlayerModel helicopter;
    private CloudsModel clouds;
    private ShopModel shop;

    public FirefighterSimulator()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        graphics.PreferredBackBufferWidth = 800;
        graphics.PreferredBackBufferHeight = 800;
        graphics.ApplyChanges();
        Window.Title = "Firefighter simulator";
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        minecraftFont = Content.Load<SpriteFont>("Fonts/Minecraft");

        var directory = new DirectoryInfo($"{Content.RootDirectory}/Map");
        var files = directory.GetFiles();
        foreach (var file in files)
        {
            var name = Path.GetFileNameWithoutExtension(file.Name);
            mapTiles.Add(Content.Load<Texture2D>($"Map/{name}"));
        }

        var dirInfo = new DirectoryInfo($"{Content.RootDirectory}/CloudMap");
        var filesInfo = dirInfo.GetFiles();
        foreach (var fileInfo in filesInfo)
        {
            var fileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            cloudTiles.Add(Content.Load<Texture2D>($"CloudMap/{fileName}"));
        }

        map = new MapModel(50, 45, mapTiles, minecraftFont);
        clouds = new CloudsModel(map, cloudTiles);
        helicopter = new PlayerModel(Content.Load<Texture2D>("Player/Helicopter"), clouds);
        shop = new ShopModel(map, helicopter);
    }

    protected override void Update(GameTime gameTime)
    {
        var ticks = gameTime.TotalGameTime.Ticks;

        MapController.Update(ticks, map);
        PlayerController.Update(ticks, helicopter);
        CloudsController.Update(ticks, clouds);
        ShopController.Update(helicopter, shop);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        spriteBatch.Begin();
        MapView.Draw(spriteBatch, map);
        PlayerView.Draw(spriteBatch, helicopter);
        CloudsView.Draw(spriteBatch, clouds);
        ShopView.Draw(spriteBatch, shop);
        spriteBatch.End();

        base.Draw(gameTime);
    }
}
