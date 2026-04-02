using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace InfinitePlatformer;

public class Platform
{
    public Rectangle bounds;
    public Color color;
    public Platform(Rectangle b, Color c) { bounds = b; color = c; }
    public void Draw(SpriteBatch sb, Texture2D t)
    {
        sb.Draw(t, bounds, color);
        sb.Draw(t, new Rectangle(bounds.X, bounds.Y, bounds.Width, 3), Color.Black);
        sb.Draw(t, new Rectangle(bounds.X, bounds.Y + bounds.Height - 3, bounds.Width, 3), Color.Black);
        sb.Draw(t, new Rectangle(bounds.X, bounds.Y, 3, bounds.Height), Color.Black);
        sb.Draw(t, new Rectangle(bounds.X + bounds.Width - 3, bounds.Y, 3, bounds.Height), Color.Black);
    }
}

public class TerrainGenerator
{
    const int pw = 100, ph = 32, genDist = 2000, cleanDist = 2500;
    List<Platform> plats = new();
    Texture2D tx;
    System.Random rnd = new();
    float lastX, groundY = 500;

    public TerrainGenerator(Texture2D t)
    {
        tx = t;
        lastX = -500;
        GenInitial();
    }

    void GenInitial()
    {
        GenPlat(lastX, (int)groundY, 800, ph);
        lastX += 800;
        while (lastX < 1500) GenNext();
    }

    void GenNext()
    {
        float gap = 60 + rnd.Next(80);
        float hv = rnd.Next(-120, 120);
        float ny = groundY + hv;
        ny = System.Math.Clamp(ny, 200, 600);
        float diff = (lastX - 400) / 5000f;
        if (diff > 0) gap += gap * diff * 0.5f;
        int w = pw + rnd.Next(50, 150);
        groundY = ny;
        GenPlat(lastX + gap, (int)ny, w, ph);
        lastX += gap + w;
        if (rnd.NextDouble() < 0.3)
        {
            float mx = lastX - w / 2f, my = ny - 100 - rnd.Next(50);
            if (my > 150) GenPlat(mx - 40, (int)my, 80, 20);
        }
    }

    void GenPlat(float x, int y, int w, int h)
    {
        int ci = (int)(x / 500) % 5;
        Color c = ci switch { 0 => new Color(180, 120, 80), 1 => new Color(140, 160, 100), 2 => new Color(160, 140, 180), 3 => new Color(180, 160, 100), 4 => new Color(120, 160, 180), _ => new Color(150, 150, 150) };
        plats.Add(new Platform(new Rectangle((int)x, y, w, h), c));
    }

    public void Update(Vector2 p)
    {
        while (lastX < p.X + genDist) GenNext();
        for (int i = plats.Count - 1; i >= 0; i--)
            if (plats[i].bounds.X + plats[i].bounds.Width < p.X - cleanDist)
                plats.RemoveAt(i);
    }

    public List<Platform> GetPlatforms() => plats;

    public void Draw(SpriteBatch sb)
    {
        foreach (var p in plats) p.Draw(sb, tx);
    }
}
