using OpenTK;
using OpenTK.Graphics.OpenGL;

class Sierpinski
{
    public static void DrawSierpinski(Vector2 a, Vector2 b, Vector2 c, int depth)
    {
        if (depth == 0)
        {
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(0.9f, 0.0f, 0.0f);
            GL.Vertex2(a);
            GL.Color3(0.0f, 0.0f, 0.9f);
            GL.Vertex2(b);
            GL.Color3(0.0f, 0.9f, 0.0f);
            GL.Vertex2(c);
            GL.End();
        }
        else
        {
            Vector2 ab = (a + b) / 2;
            Vector2 bc = (b + c) / 2;
            Vector2 ca = (c + a) / 2;

            DrawSierpinski(a, ab, ca, depth - 1);
            DrawSierpinski(ab, b, bc, depth - 1);
            DrawSierpinski(ca, bc, c, depth - 1);
        }
    }

    public static void Render(int recursionDepth)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);

        Vector2 vertexA = new Vector2(-0.8f, -0.8f);
        Vector2 vertexB = new Vector2(0.8f, -0.8f);
        Vector2 vertexC = new Vector2(0.0f, 0.8f);

        DrawSierpinski(vertexA, vertexB, vertexC, recursionDepth);
    }
}
