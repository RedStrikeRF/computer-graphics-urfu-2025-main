using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class Polygon
{
    public PointF[] Points;
    public float Thickness;
    public Color BorderColor;
    public Color FillColor;
    public Color BackgroundColor;

    public Polygon(Point center, int sides, int radius, float thickness, Color borderColor, Color fillColor, Color backgroundColor)
    {
        Thickness = thickness;
        BorderColor = borderColor;
        FillColor = fillColor;
        BackgroundColor = backgroundColor;
        Points = new PointF[sides];

        var angleStep = 2 * Math.PI / sides;
        for (int i = 0; i < sides; i++)
        {
            var x = center.X + radius * (float)Math.Cos(i * angleStep);
            var y = center.Y + radius * (float)Math.Sin(i * angleStep);
            Points[i] = new PointF(x, y);
        }
    }

    public Polygon(PointF[] points, float thickness, Color borderColor, Color fillColor, Color backgroundColor)
    {
        Points = points;
        Thickness = thickness;
        BorderColor = borderColor;
        FillColor = fillColor;
        BackgroundColor = backgroundColor;
    }

    public void Draw(Graphics g)
    {
        var pen = new Pen(BorderColor, Thickness);
        var brush = new SolidBrush(FillColor);

        g.FillPolygon(brush, Points);
        g.DrawPolygon(pen, Points);
    }

    public void Erase(Graphics g)
    {
        var pen = new Pen(BackgroundColor, Thickness);
        var brush = new SolidBrush(BackgroundColor);
        
        g.FillPolygon(brush, Points);
        g.DrawPolygon(pen, Points);
    }
}
