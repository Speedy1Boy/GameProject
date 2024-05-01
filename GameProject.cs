using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ReworkedGame;

public class GameProject : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private List<Texture2D> tiles = new();
    private Map map;
    private Player player;
    public GameProject()
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
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        tiles.Add(Content.Load<Texture2D>("Grass"));
        tiles.Add(Content.Load<Texture2D>("DeadGrass"));
        tiles.Add(Content.Load<Texture2D>("Tree"));
        tiles.Add(Content.Load<Texture2D>("Wood"));
        tiles.Add(Content.Load<Texture2D>("Water"));
        tiles.Add(Content.Load<Texture2D>("Cliff"));
        tiles.Add(Content.Load<Texture2D>("Fire"));
        map = new Map(50, tiles);

        player = new Player(Content.Load<Texture2D>("Player"), map);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
        }
        player.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Green);
        _spriteBatch.Begin();
        map.Draw(_spriteBatch);
        player.Draw(_spriteBatch);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
