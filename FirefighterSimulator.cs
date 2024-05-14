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
    private Map map;
    private Player helicopter;
    private CloudMap clouds;
    private Shop shop;
    private Texture2D cell;

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

        cloudTiles.Add(Content.Load<Texture2D>("CloudMap/Clear"));
        cloudTiles.Add(Content.Load<Texture2D>("CloudMap/Cloud"));
        cloudTiles.Add(Content.Load<Texture2D>("CloudMap/Rain"));
        cloudTiles.Add(Content.Load<Texture2D>("CloudMap/Thunder"));

        cell = Content.Load<Texture2D>("Cell");

        map = new Map(50, 45, mapTiles, minecraft);
        clouds = new CloudMap(map.Width, map.Height, cloudTiles);
        helicopter = new Player(Content.Load<Texture2D>("Player/Helicopter"), map);
        shop = new Shop(map, helicopter);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        map.Update(gameTime.TotalGameTime.Ticks, clouds);
        shop.Update();
        helicopter.Update(gameTime.TotalGameTime.Ticks);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        spriteBatch.Begin();

        for (var i = 0; i < map.Width; i++)
            for (var j = 0; j < 5; j++)
                spriteBatch.Draw(cell, new Vector2(i * map.TileSize, map.Height * map.TileSize + j * map.TileSize), Color.DarkSlateGray);
        map.Draw(spriteBatch);
        shop.Draw(spriteBatch);
        helicopter.Draw(spriteBatch);
        clouds.Draw(spriteBatch);

        spriteBatch.End();
        base.Draw(gameTime);
    }
}
