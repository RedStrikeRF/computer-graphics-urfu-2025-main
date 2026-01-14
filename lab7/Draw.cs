using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

public static class Figures
{
    public static int ListTextureId;
    public static void DrawList()
    {
        float z = 0f;
        GL.Disable(EnableCap.Blend);
        GL.Enable(EnableCap.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, ListTextureId);

        GL.Begin(PrimitiveType.Quads);

        GL.TexCoord2(0.0f, 0.0f);
        GL.Vertex3(-100.0f, -100.0f, z);

        GL.TexCoord2(1.0f, 0.0f);
        GL.Vertex3(100.0f, -100.0f, z);

        GL.TexCoord2(1.0f, 1.0f);
        GL.Vertex3(100.0f, 100.0f, z);

        GL.TexCoord2(0.0f, 1.0f);
        GL.Vertex3(-100.0f, 100.0f, z);

        GL.End();

        GL.Disable(EnableCap.Texture2D);
    }

    public static void DrawListGrid(int gridSize, float tileSize)
    {
        GL.Disable(EnableCap.Blend);
        GL.Enable(EnableCap.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, ListTextureId);

        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++)
            {
                float worldX = x * tileSize;
                float worldY = y * tileSize;

                GL.Begin(PrimitiveType.Quads);

                GL.TexCoord2(0.0f, 0.0f);
                GL.Vertex3(worldX, worldY, 0f);

                GL.TexCoord2(1.0f, 0.0f);
                GL.Vertex3(worldX + tileSize, worldY, 0f);

                GL.TexCoord2(1.0f, 1.0f);
                GL.Vertex3(worldX + tileSize, worldY + tileSize, 0f);

                GL.TexCoord2(0.0f, 1.0f);
                GL.Vertex3(worldX, worldY + tileSize, 0f);

                GL.End();
            }
        }

        GL.Disable(EnableCap.Texture2D);
    }


    public static void DrawTexturedTriangle(int textureId)
    {
        float z = 40f;

        GL.Enable(EnableCap.Texture2D);
        GL.BindTexture(TextureTarget.Texture2D, textureId);

        GL.Begin(PrimitiveType.Triangles);

        GL.TexCoord2(0.0f, 0.0f);
        GL.Vertex3(-100.0f, -100.0f, z);

        GL.TexCoord2(1.0f, 0.0f);
        GL.Vertex3(100.0f, -100.0f, z);

        GL.TexCoord2(0.5f, 1.0f);
        GL.Vertex3(0.0f, 100.0f, z);

        GL.End();

        GL.Disable(EnableCap.Texture2D);
    }

    public static void DrawCylinder(Vector3 center, float radius, float height, int segments, PrimitiveType drawType, Color color,
        float rotateX, float rotateY)
    {
        GL.PushMatrix();
        GL.Translate(center);
        GL.Color3(color);

        GL.Rotate(rotateX, 1, 0, 0);
        GL.Rotate(rotateY, 0, 1, 0);

        for (int i = 0; i < segments; i++)
        {
            float angle1 = 2f * (float)Math.PI * i / segments;
            float angle2 = 2f * (float)Math.PI * (i + 1) / segments;

            Vector3 p1 = new Vector3((float)Math.Cos(angle1) * radius, (float)Math.Sin(angle1) * radius, 0);
            Vector3 p2 = new Vector3((float)Math.Cos(angle2) * radius, (float)Math.Sin(angle2) * radius, 0);

            Vector3 p3 = new Vector3(p2.X, p2.Y, height);
            Vector3 p4 = new Vector3(p1.X, p1.Y, height);

            GL.Begin(drawType);

            GL.Vertex3(p1);
            GL.Vertex3(p2);
            GL.Vertex3(p3);
            GL.Vertex3(p4);

            GL.End();
        }

        GL.PopMatrix();
    }


    public static void DrawAxes()
    {
        GL.Begin(PrimitiveType.Lines);

        GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-300.0f, 0.0f, 0.0f); GL.Vertex3(300.0f, 0.0f, 0.0f); // X
        GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(0.0f, -300.0f, 0.0f); GL.Vertex3(0.0f, 300.0f, 0.0f); // Y
        GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 0.0f, -300f); GL.Vertex3(0.0f, 0.0f, 300.0f); // Z

        GL.End();
    }

    public static void DrawTriangle()
    {
        float z = 40f;
        GL.Begin(PrimitiveType.Triangles);

        GL.Color3(1.0f, 1.0f, 0.0f); GL.Vertex3(-100.0f, -100.0f, z);
        GL.Color3(1.0f, 0.0f, 0.0f); GL.Vertex3(100.0f, -100.0f, z);
        GL.Color3(0.2f, 0.9f, 1.0f); GL.Vertex3(0.0f, 100.0f, z);

        GL.End();
    }

    public static void DrawCube(Vector3 center, float size, PrimitiveType drawType, Color color)
    {
        GL.PushMatrix();
        GL.Translate(center);
        GL.Color4(color);
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        var vertices = new Vector3[]
        {
            new Vector3(-size, -size, -size),
            new Vector3( size, -size, -size),
            new Vector3( size,  size, -size),
            new Vector3(-size,  size, -size),
            new Vector3(-size, -size,  size),
            new Vector3( size, -size,  size),
            new Vector3( size,  size,  size),
            new Vector3(-size,  size,  size)
        };

        var faces = new int[][]
        {
            new int[] {0, 1, 2, 3},
            new int[] {4, 5, 6, 7},
            new int[] {0, 1, 5, 4},
            new int[] {2, 3, 7, 6},
            new int[] {1, 2, 6, 5},
            new int[] {0, 3, 7, 4}
        };

        foreach (var face in faces)
        {
            GL.Begin(drawType);
            GL.Vertex3(vertices[face[0]]);
            GL.Vertex3(vertices[face[1]]);
            GL.Vertex3(vertices[face[2]]);
            GL.Vertex3(vertices[face[3]]);
            GL.End();
        }

        GL.PopMatrix();
    }

    public static void DrawTruncatedPyramid(Vector3 center, float h, float r1, float r2, PrimitiveType drawType, Color color)
    {
        GL.PushMatrix();
        GL.Translate(center);
        GL.Color3(color);

        var vertices = new Vector3[]
        {
            new Vector3(-r2, -r2, 0),
            new Vector3(r2, -r2, 0),
            new Vector3(r2, r2, 0),
            new Vector3(-r2, r2, 0),
            new Vector3(-r1, -r1, h),
            new Vector3(r1, -r1, h),
            new Vector3(r1, r1, h),
            new Vector3(-r1, r1, h)
        };

        var faces = new int[][]
        {
            new int[] {0, 1, 2, 3},
            new int[] {4, 5, 6, 7},
            new int[] {0, 1, 5, 4},
            new int[] {2, 3, 7, 6},
            new int[] {1, 2, 6, 5},
            new int[] {0, 3, 7, 4}
        };

        foreach (var face in faces)
        {
            GL.Begin(drawType);
            GL.Vertex3(vertices[face[0]]);
            GL.Vertex3(vertices[face[1]]);
            GL.Vertex3(vertices[face[2]]);
            GL.Vertex3(vertices[face[3]]);
            GL.End();
        }

        GL.PopMatrix();
    }

    public static void DrawCone(Vector3 center, float radius, float height, int segments, PrimitiveType drawType, Color color)
    {
        GL.PushMatrix();
        GL.Translate(center);
        GL.Color3(color);

        var top = new Vector3(0, 0, height);
        for (int i = 0; i < segments; i++)
        {
            var a1 = 2 * Math.PI * i / segments;
            var a2 = 2 * Math.PI * (i + 1) / segments;
            var p1 = new Vector3((float)Math.Cos(a1) * radius, (float)Math.Sin(a1) * radius, 0);
            var p2 = new Vector3((float)Math.Cos(a2) * radius, (float)Math.Sin(a2) * radius, 0);

            GL.Begin(drawType);
            GL.Vertex3(top);
            GL.Vertex3(p1);
            GL.Vertex3(p2);
            GL.End();
        }

        GL.PopMatrix();
    }

    public static void DrawTrefoilSurface(float step = 0.01f, PrimitiveType drawType = PrimitiveType.Points)
    {
        GL.Begin(drawType);
        float size = 20f;

        for (float u = -2 * MathF.PI; u <= 2 * MathF.PI; u += step)
        {
            for (float v = -MathF.PI; v <= MathF.PI; v += step)
            {
                float cosU = MathF.Cos(u);
                float sinU = MathF.Sin(u);
                float cosV = MathF.Cos(v);
                float sinV = MathF.Sin(v);

                float x = cosU * cosV + 3 * cosU * (1.5f + MathF.Sin(1.5f * u) / 2);
                float y = sinU * cosV + 3 * sinU * (1.5f + MathF.Sin(1.5f * u) / 2);
                float z = sinV + 2 * MathF.Cos(1.5f * u);
                GL.Color3(0, 0, v);

                GL.Vertex3(x * size, y * size, z * size);
            }
        }

        GL.End();
    }

    public static void DrawParametricSurface2(float stepU = 0.1f, float stepV = 0.05f, PrimitiveType drawType = PrimitiveType.Points)
    {
        GL.Begin(drawType);
        GL.Color3(Color.LightBlue);
        float size = 70f;

        for (float u = 0f; u <= 4 * MathF.PI; u += stepU)
        {
            for (float v = 0.001f; v <= 2f; v += stepV)
            {
                float x = MathF.Cos(u) * MathF.Sin(v);
                float y = MathF.Sin(u) * MathF.Sin(v);
                float z = MathF.Cos(v) + MathF.Log(MathF.Tan(v / 2f)) + 0.2f * u - 4f;

                GL.Color3(0, v / 3 * 2, v / 3 * 2);
                GL.Vertex3(x * size, y * size, z * size + 3.3 * size);
            }
        }

        GL.End();
    }
}
