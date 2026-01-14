using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;

public struct CylinderSegment
{
    public Vector3 Position;
    public float Radius;
    public float Height;
    public float RotateX;
    public float RotateY;
    public float RotateZ;
    public Color Color;
}

public class Tree {
    public static void GenerateTreeSegments(
        List<CylinderSegment> segments,
        Vector3 position,
        float radius,
        float height,
        int depth,
        Random rng,
        float rotateX = 0,
        float rotateY = 0,
        float rotateZ = 0
    )
    {
        if (depth <= 0) return;

        segments.Add(new CylinderSegment
        {
            Position = position,
            Radius = radius,
            Height = height,
            RotateX = rotateX,
            RotateY = rotateY,
            RotateZ = rotateZ,
            Color = depth == 1 ? Color.Green : Color.SaddleBrown
        });

        // Новая база – вершина текущего цилиндра (переход будет внутри GL.PushMatrix + Rotate)
        Vector3 newBase = new Vector3(0, 0, height);

        int branchCount = rng.Next(2, 4); // 2-3 ветви

        for (int i = 0; i < branchCount; i++)
        {
            float rx = rng.Next(-30, 30);
            float ry = rng.Next(-30, 30);
            float rz = rng.Next(-10, 10);

            float newHeight = height * (0.6f + (float)rng.NextDouble() * 0.2f);
            float newRadius = radius * 0.7f;

            GL.PushMatrix();
            GL.Translate(position);
            GL.Rotate(rotateX, 1, 0, 0);
            GL.Rotate(rotateY, 0, 1, 0);
            GL.Rotate(rotateZ, 0, 0, 1);

            GenerateTreeSegments(
                segments,
                newBase,
                newRadius,
                newHeight,
                depth - 1,
                rng,
                rx, ry, rz
            );

            GL.PopMatrix();
        }
    }

    public static void DrawTreeFromSegments(List<CylinderSegment> segments)
    {
        foreach (var seg in segments)
        {
            Figures.DrawCylinder(seg.Position, seg.Radius, seg.Height, 12, PrimitiveType.Quads, seg.Color, seg.RotateX, seg.RotateY);

        }
    }
}