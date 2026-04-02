using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

// Echo por anthony lopez - https://github.com/anthoo582

namespace InfinitePlatformer;

public class Game1 : Game
{
    GraphicsDeviceManager g; SpriteBatch sb; Player p; TerrainGenerator t; Camera c;
    SpriteFont f; Texture2D tx;
    const string url = "https://github.com/anthoo582";
    Rectangle crb; MouseState pm;

    public Game1()
    {
        g = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        g.PreferredBackBufferWidth = 960;
        g.PreferredBackBufferHeight = 540;
    }

    protected override void Initialize()
    {
        sb = new SpriteBatch(GraphicsDevice);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        tx = new Texture2D(GraphicsDevice, 1, 1);
        tx.SetData(new[] { Color.White });
        try { f = Content.Load<SpriteFont>("Font"); } catch { f = null; }
        p = new Player(new Vector2(100, 300), tx);
        t = new TerrainGenerator(tx);
        c = new Camera(GraphicsDevice.Viewport);
    }

    protected override void Update(GameTime gt)
    {
        var ks = Keyboard.GetState();
        var ms = Mouse.GetState();
        if (ks.IsKeyDown(Keys.Escape)) Exit();
        if (ms.LeftButton == ButtonState.Pressed && pm.LeftButton == ButtonState.Released)
            if (crb.Contains(ms.X, ms.Y))
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { UseShellExecute = true, CreateNoWindow = true });
        pm = ms;
        float dt = (float)gt.ElapsedGameTime.TotalSeconds;
        p.Update(dt, ks);
        t.Update(p.pos);
        p.CheckCollisions(t.GetPlatforms());
        c.Follow(p.pos);
        base.Update(gt);
    }

    protected override void Draw(GameTime gt)
    {
        GraphicsDevice.Clear(new Color(30, 30, 40));
        sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, c.Transform);
        t.Draw(sb); p.Draw(sb);
        sb.End();
        sb.Begin();
        string info = $"Pos: {p.pos.X:F0},{p.pos.Y:F0} | Vel: {p.vel.X:F1},{p.vel.Y:F1} | Suelo: {p.ground}";
        if (f != null) sb.DrawString(f, info, new Vector2(10, 10), Color.White);
        crb = new Rectangle(g.PreferredBackBufferWidth - 200, 10, 200, 20);
        sb.End();
        base.Draw(gt);
    }
}
