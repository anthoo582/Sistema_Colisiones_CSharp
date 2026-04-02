using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InfinitePlatformer;

public class Player
{
    const float speed = 400f, jump = -650f, grav = 1800f, maxFall = 800f, fric = 0.85f, acc = 0.3f, airAcc = 0.15f;
    Texture2D tx;
    public Vector2 pos, vel;
    public bool ground, wasJump;
    public int w = 32, h = 48;
    public Rectangle bounds => new((int)pos.X, (int)pos.Y, w, h);
    
    float coyoteTime = 0.1f, coyoteCounter;
    float jumpBuffer = 0.15f, jumpBufferCounter;
    bool jumpReleased = true;

    public Player(Vector2 start, Texture2D t)
    {
        pos = start;
        tx = t;
        vel = Vector2.Zero;
    }

    public void Update(float dt, KeyboardState k)
    {
        bool l = k.IsKeyDown(Keys.A) || k.IsKeyDown(Keys.Left);
        bool r = k.IsKeyDown(Keys.D) || k.IsKeyDown(Keys.Right);
        bool j = k.IsKeyDown(Keys.Space) || k.IsKeyDown(Keys.W) || k.IsKeyDown(Keys.Up);
        
        float tv = 0;
        if (l) tv = -speed;
        if (r) tv = speed;
        
        float curAcc = ground ? acc : airAcc;
        vel.X = Lerp(vel.X, tv, curAcc);
        if (System.Math.Abs(vel.X) < 10 && tv == 0) vel.X *= fric;
        
        if (j) jumpBufferCounter = jumpBuffer;
        if (jumpBufferCounter > 0) jumpBufferCounter -= dt;
        
        if (ground) coyoteCounter = coyoteTime;
        else if (coyoteCounter > 0) coyoteCounter -= dt;
        
        if (jumpBufferCounter > 0 && coyoteCounter > 0 && jumpReleased)
        {
            vel.Y = jump;
            coyoteCounter = 0;
            jumpBufferCounter = 0;
            jumpReleased = false;
        }
        
        if (!j) jumpReleased = true;
        
        if (!j && vel.Y < jump * 0.5f) vel.Y = jump * 0.5f;
        
        vel.Y += grav * dt;
        if (vel.Y > maxFall) vel.Y = maxFall;
        pos += vel * dt;
        if (pos.Y > 1000) Respawn();
    }

    public void CheckCollisions(System.Collections.Generic.List<Platform> plats)
    {
        ground = false;
        var b = bounds;
        foreach (var p in plats)
        {
            if (!b.Intersects(p.bounds)) continue;
            float ol = (b.X + b.Width) - p.bounds.X;
            float or = (p.bounds.X + p.bounds.Width) - b.X;
            float ot = (b.Y + b.Height) - p.bounds.Y;
            float ob = (p.bounds.Y + p.bounds.Height) - b.Y;
            bool fl = ol < or, ft = ot < ob;
            float mx = fl ? ol : or, my = ft ? ot : ob;
            if (mx < my)
            {
                pos.X = fl ? p.bounds.X - w : p.bounds.X + p.bounds.Width;
                vel.X = 0;
            }
            else
            {
                pos.Y = ft ? p.bounds.Y - h : p.bounds.Y + p.bounds.Height;
                vel.Y = 0;
                if (ft) ground = true;
            }
        }
    }

    void Respawn()
    {
        pos = new Vector2(100, 300);
        vel = Vector2.Zero;
    }

    float Lerp(float a, float b, float t) => a + (b - a) * t;

    public void Draw(SpriteBatch sb)
    {
        Color c = ground ? new Color(100, 200, 100) : new Color(100, 150, 255);
        sb.Draw(tx, bounds, c);
        sb.Draw(tx, new Rectangle((int)pos.X, (int)pos.Y, w, 2), Color.Black);
        sb.Draw(tx, new Rectangle((int)pos.X, (int)pos.Y + h - 2, w, 2), Color.Black);
        sb.Draw(tx, new Rectangle((int)pos.X, (int)pos.Y, 2, h), Color.Black);
        sb.Draw(tx, new Rectangle((int)pos.X + w - 2, (int)pos.Y, 2, h), Color.Black);
    }
}
