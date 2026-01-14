using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class Line
{
    public Point StartPoint;
    public Point EndPoint;
    public float Thickness;
    public Color LineColor;
    public Color BackgroundColor;
    public DashStyle DashStyle;
    public LineCap StartCap;
    public LineCap EndCap;

    public Line(
        Point start,
        Point end,
        float thickness,
        Color lineColor,
        Color backgroundColor,
        DashStyle dashStyle = DashStyle.Solid,
        LineCap startCap = LineCap.Flat,
        LineCap endCap = LineCap.Flat)
    {
        StartPoint = start;
        EndPoint = end;
        Thickness = thickness;
        LineColor = lineColor;
        BackgroundColor = backgroundColor;
        DashStyle = dashStyle;
        StartCap = startCap;
        EndCap = endCap;
    }

    public void Draw(Graphics g)
    {
        Pen pen = new Pen(LineColor, Thickness);
        pen.DashStyle = DashStyle;
        pen.StartCap = StartCap;
        pen.EndCap = EndCap;
        g.DrawLine(pen, StartPoint, EndPoint);
    }

    public void Erase(Graphics g)
    {
        var pen = new Pen(BackgroundColor, Thickness);
        pen.DashStyle = DashStyle;
        pen.StartCap = StartCap;
        pen.EndCap = EndCap;
        g.DrawLine(pen, StartPoint, EndPoint);
    }
}
