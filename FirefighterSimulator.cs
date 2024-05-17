using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.IO;

namespace GameProject;

public class FirefighterSimulator : Game
{
    private readonly GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private MapModel map;
    private PlayerModel helicopter;
    private CloudsModel clouds;
    private ShopModel shop;
    private SpriteFont minecraftFont;
    private bool isPaused;
    private bool isPressed;

    public FirefighterSimulator()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        graphics.PreferredBackBufferWidth = 800;
        graphics.PreferredBackBufferHeight = 800;
        graphics.ApplyChanges();
        Window.Title = "Firefighter simulator";
        Window.ClientSizeChanged += Window_ClientSizeChanged;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        minecraftFont = Content.Load<SpriteFont>("Fonts/Minecraft");

        var mapTiles = new List<Texture2D>();
        var directory = new DirectoryInfo($"{Content.RootDirectory}/Map");
        var files = directory.GetFiles();
        foreach (var file in files)
        {
            var name = Path.GetFileNameWithoutExtension(file.Name);
            mapTiles.Add(Content.Load<Texture2D>($"Map/{name}"));
        }

        var cloudTiles = new List<Texture2D>
        {
            Content.Load<Texture2D>("CloudMap/Clear"),
            Content.Load<Texture2D>("CloudMap/Cloud"),
            Content.Load<Texture2D>("CloudMap/Rain"),
            Content.Load<Texture2D>("CloudMap/Thunder")
        };

        var helicopterSprites = new[]
        {
            Content.Load<Texture2D>("Player/HelicopterLeft"),
            Content.Load<Texture2D>("Player/HelicopterRight")
        };

        map = new MapModel(50, 45, mapTiles, minecraftFont, Window);
        clouds = new CloudsModel(map, cloudTiles);
        helicopter = new PlayerModel(helicopterSprites, clouds);
        shop = new ShopModel(map, helicopter);
    }

    protected override void Update(GameTime gameTime)
    {
        var kState = Keyboard.GetState();
        if (kState.IsKeyDown(Keys.Escape) && !isPressed && !helicopter.IsDead())
        {
            isPaused = !isPaused;
            isPressed = true;
        }
        if (kState.IsKeyUp(Keys.Escape) && isPressed) isPressed = false;

        if (!isPaused)
        {
            var ticks = gameTime.TotalGameTime.Ticks;
            MapController.Update(ticks, map);
            CloudsController.Update(ticks, clouds);
            PlayerController.Update(ticks, helicopter);
            ShopController.Update(helicopter, shop);
        }

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
        DrawInfo();
        spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawInfo()
    {
        if (isPaused)
            spriteBatch.DrawString(minecraftFont, "Pause",
                GetInfoText(-2), Color.Black);
        if (helicopter.IsDead())
            spriteBatch.DrawString(minecraftFont, "Game Over",
                GetInfoText(-6), Color.Red, 0,
                Vector2.Zero, 1.5f, SpriteEffects.None, 0);
    }

    private Vector2 GetInfoText(int d)
    {
        var (x, y) = map.CalculateNewPos(1, 1);
        return new Vector2(x, y + d);
    }

    private void Window_ClientSizeChanged(object sender, EventArgs e)
    {
        Window.ClientSizeChanged -= Window_ClientSizeChanged;
        graphics.PreferredBackBufferWidth = Window.ClientBounds.Width < 800 ? 800 : Window.ClientBounds.Width;
        graphics.PreferredBackBufferHeight = Window.ClientBounds.Height < 800 ? 800 : Window.ClientBounds.Height;

        graphics.ApplyChanges();
        Window.ClientSizeChanged += Window_ClientSizeChanged;
    }
}
