using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Linq;

partial class Game : GameWindow
{    
    private void DrawFigure(Vector3[] points, Vector3[] colors, int count, PrimitiveType primitive)
    {
        GL.Begin(primitive);
        for (int i = 0; i < count; i++)
        {
            GL.Color3(colors[i]);
            GL.Vertex3(points[i]);
        }
        GL.End();
    }

    private void DrawRegularPolygon(Vector3 center, float radius, Vector3[] color, int count, PrimitiveType mode)
    {
        GL.Begin(mode);
        for (int i = 0; i < count; i++)
        {
            var angle = i * 3.1415f * 2 / count;
            var x = center.X + radius * (float)Math.Cos(angle);
            var y = center.Y + radius * (float)Math.Sin(angle);

            GL.Color3(color[i]);
            GL.Vertex3(x, y, center.Z);
        }
        GL.End();
    }

    void DrawRegularPolygon(Vector3 center, float radius, Vector3 color, int count, PrimitiveType mode)
    {  
        var colors = Enumerable.Repeat(color, count).ToArray();
        DrawRegularPolygon(center, radius, colors, count, mode);
    }
}
