using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace InfinitePlatformer;

public class Camera
{
    public Matrix Transform;
    Viewport vp;
    Vector2 pos;
    const float smooth = 0.1f;

    public Camera(Viewport v) { vp = v; pos = Vector2.Zero; }

    public void Follow(Vector2 t)
    {
        Vector2 target = new(t.X - vp.Width / 2f, 0);
        if (target.X < 0) target.X = 0;
        pos = Vector2.Lerp(pos, target, smooth);
        Transform = Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0));
    }
}
