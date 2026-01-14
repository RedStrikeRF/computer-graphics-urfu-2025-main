using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

partial class Game : GameWindow
{
    void DrawTriangleExample()
    {
        var n = 3;
        var z = 4.0f;

        var p = new Vector3[n];
        p[0] = new Vector3(-1.0f, -1.0f, z);
        p[1] = new Vector3(1.0f, -1.0f, z);
        p[2] = new Vector3(0.0f, 1.0f, z);

        var clr = new Vector3[n];
        clr[0] = new Vector3(1.0f, 1.0f, 0.0f);
        clr[1] = new Vector3(1.0f, 0.0f, 0.0f);
        clr[2] = new Vector3(0.2f, 0.9f, 1.0f);

        DrawFigure(p, clr, n, PrimitiveType.Triangles);
    }

    private void DrawSqureExample()
    {
        var n = 4;
        var z = 4.0f;

        var p = new Vector3[n];
        p[0] = new Vector3(-1.0f, -1.0f, z);
        p[1] = new Vector3(1.0f, -1.0f, z);
        p[2] = new Vector3(1.0f, 1.0f, z);
        p[3] = new Vector3(-1.0f, 1.0f, z);

        var clr = new Vector3[n];
        clr[0] = new Vector3(1.0f, 1.0f, 0.0f);
        clr[1] = new Vector3(1.0f, 0.0f, 0.0f);
        clr[2] = new Vector3(0.2f, 0.9f, 1.0f);
        clr[3] = new Vector3(0.0f, 1.0f, 0.0f);

        DrawFigure(p, clr, n, PrimitiveType.LineLoop); // PrimitiveType.Triangles, LineLoop, Polygon
        // DrawFigure(p, clr, n, PrimitiveType.Polygon);
    }
    
    private void DrawCircleExample()
    {
        var center = new Vector3(0.0f, 0.0f, 4.0f);
        var radius = 1.0f;
        var clr = new Vector3(1.0f, 0.5f, 0.2f);
        var sides = 50;

        DrawRegularPolygon(center, radius, clr, sides, PrimitiveType.Polygon);
    }

    private void DrawExample()
    {
        DrawSqureExample();
        DrawTriangleExample();
        DrawCircleExample();
    }
}
