using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameProject;

public class FirefighterSimulator : Game
{
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private SpriteFont minecraft;
    private readonly List<Texture2D> mapTiles = new();
    private readonly List<Texture2D> cloudTiles = new();
    private MapModel map;
    private Player helicopter;
    private CloudMap clouds;
    private Shop shop;

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

        minecraft = Content.Load<SpriteFont>("Fonts/Minecraft");

        mapTiles.Add(Content.Load<Texture2D>("Map/Grass"));
        mapTiles.Add(Content.Load<Texture2D>("Map/DeadGrass"));
        mapTiles.Add(Content.Load<Texture2D>("Map/Tree"));
        mapTiles.Add(Content.Load<Texture2D>("Map/Wood"));
        mapTiles.Add(Content.Load<Texture2D>("Map/Plant"));
        mapTiles.Add(Content.Load<Texture2D>("Map/Water"));
        mapTiles.Add(Content.Load<Texture2D>("Map/Cliff"));
        mapTiles.Add(Content.Load<Texture2D>("Map/Fire"));
        mapTiles.Add(Content.Load<Texture2D>("Map/Hospital"));
        mapTiles.Add(Content.Load<Texture2D>("Map/Shop"));
        mapTiles.Add(Content.Load<Texture2D>("Cell"));

        cloudTiles.Add(Content.Load<Texture2D>("CloudMap/Clear"));
        cloudTiles.Add(Content.Load<Texture2D>("CloudMap/Cloud"));
        cloudTiles.Add(Content.Load<Texture2D>("CloudMap/Rain"));
        cloudTiles.Add(Content.Load<Texture2D>("CloudMap/Thunder"));

        map = new MapModel(50, 45, mapTiles, minecraft);
        clouds = new CloudMap(map, cloudTiles);
        helicopter = new Player(Content.Load<Texture2D>("Player/Helicopter"), clouds);
        shop = new Shop(map, helicopter);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

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
