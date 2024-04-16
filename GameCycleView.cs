using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameProject;

public class GameCycleView : Game, IGameplayView
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Dictionary<int, IObject> _objects = new();
    private readonly Dictionary<int, Texture2D> _textures = new();

    public event EventHandler CycleFinished = delegate { };
    public event EventHandler<ControlsEventArgs> PlayerMoved = delegate { };

    public GameCycleView()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _textures.Add(1, Content.Load<Texture2D>("Helicopter"));
    }

    public void LoadGameCycleParameters(Dictionary<int, IObject> objects)
    {
        _objects = objects;
    }

    protected override void Update(GameTime gameTime)
    {
        var keys = Keyboard.GetState().GetPressedKeys();
        if (keys.Length > 0)
        {
            var k = keys[0];
            switch (k)
            {
                case Keys.W:
                    PlayerMoved.Invoke(this, new ControlsEventArgs { Direction = IGameplayModel.Direction.Up });
                    break;
                case Keys.S:
                    PlayerMoved.Invoke(this, new ControlsEventArgs { Direction = IGameplayModel.Direction.Down });
                    break;
                case Keys.A:
                    PlayerMoved.Invoke(this, new ControlsEventArgs { Direction = IGameplayModel.Direction.Left });
                    break;
                case Keys.D:
                    PlayerMoved.Invoke(this, new ControlsEventArgs { Direction = IGameplayModel.Direction.Right });
                    break;
                case Keys.Escape:
                    Exit();
                    break;
            }
        }
        base.Update(gameTime);
        CycleFinished.Invoke(this, new EventArgs());
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        base.Draw(gameTime);
        _spriteBatch.Begin();
        foreach (var obj in _objects.Values)
            _spriteBatch.Draw(_textures[obj.ImageId], obj.Pos, Color.White);
        _spriteBatch.End();
    }
}
