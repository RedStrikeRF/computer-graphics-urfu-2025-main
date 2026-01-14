using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public static class Core
{
    public static Button CreateButton(string text, int x, int y, int w = 180, int h = 30, EventHandler click = null)
    {
        var btn = new Button();
        btn.Text = text;
        btn.SetBounds(x, y, w, h);

        if (click != null)
            btn.Click += click;

        return btn;
    }
}
