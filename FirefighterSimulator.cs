using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;

namespace GameProject;

public class FirefighterSimulator : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private List<Texture2D> mapTiles = new();
    private List<Texture2D> cloudTiles = new();
    private Map map;
    private Player player;
    private Clouds clouds;
    public FirefighterSimulator()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 800;
        _graphics.ApplyChanges();
        Window.Title = "Firefighter simulator";
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        mapTiles.Add(Content.Load<Texture2D>("Grass"));
        mapTiles.Add(Content.Load<Texture2D>("DeadGrass"));
        mapTiles.Add(Content.Load<Texture2D>("Tree"));
        mapTiles.Add(Content.Load<Texture2D>("Wood"));
        mapTiles.Add(Content.Load<Texture2D>("Plant"));
        mapTiles.Add(Content.Load<Texture2D>("Water"));
        mapTiles.Add(Content.Load<Texture2D>("Cliff"));
        mapTiles.Add(Content.Load<Texture2D>("Fire"));

        cloudTiles.Add(Content.Load<Texture2D>("Clear"));
        cloudTiles.Add(Content.Load<Texture2D>("Cloud"));
        cloudTiles.Add(Content.Load<Texture2D>("Rain"));
        cloudTiles.Add(Content.Load<Texture2D>("Thunder"));

        map = new Map(50, mapTiles);
        clouds = new Clouds(map.Size, cloudTiles);
        player = new Player(Content.Load<Texture2D>("Player"), map);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        player.Update();
        map.Update(clouds);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Green);
        _spriteBatch.Begin();
        map.Draw(_spriteBatch);
        player.Draw(_spriteBatch);
        clouds.Draw(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
