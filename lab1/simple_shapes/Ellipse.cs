using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class Ellipse
{
    public Point Center;
    public int RadiusX;
    public int RadiusY;
    public Color BorderColor;
    public Color FillColor;
    public Color BackgroundColor;
    public string Text;
    public Font TextFont;
    public Color TextColor;

    public Ellipse(Point center, int radiusX, int radiusY, Color borderColor, Color fillColor, Color backgroundColor, string text, Color textColor)
    {
        Center = center;
        RadiusX = radiusX;
        RadiusY = radiusY;
        BorderColor = borderColor;
        FillColor = fillColor;
        BackgroundColor = backgroundColor;
        Text = text;
        TextFont = new Font("Arial", 10);
        TextColor = textColor;
    }

    public void Draw(Graphics g)
    {
        var rect = new Rectangle(Center.X - RadiusX, Center.Y - RadiusY, RadiusX * 2, RadiusY * 2);
        var pen = new Pen(BorderColor);
        var brush = new SolidBrush(FillColor);
        var textBrush = new SolidBrush(TextColor);
        
        g.FillEllipse(brush, rect);
        g.DrawEllipse(pen, rect);
        g.DrawString(Text, TextFont, textBrush, Center);
    }

    public void Erase(Graphics g)
    {
        var rect = new Rectangle(Center.X - RadiusX, Center.Y - RadiusY, RadiusX * 2, RadiusY * 2);
        var pen = new Pen(BackgroundColor);
        var brush = new SolidBrush(BackgroundColor);

        g.FillEllipse(brush, rect);
        g.DrawEllipse(pen, rect);
    }
}
