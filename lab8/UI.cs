using System;
using System.Drawing;
using System.Windows.Forms;

public partial class RaycastingForm : Form
{
    TextBox tX, tY, tZ, tdirX, tdirY, tdirZ;

    private void AddCameraUI()
    {
        var labelX = new Label() { Text = "X:", Left = 820, Top = 20, Width = 20 };
        var labelY = new Label() { Text = "Y:", Left = 820, Top = 50, Width = 20 };
        var labelZ = new Label() { Text = "Z:", Left = 820, Top = 80, Width = 20 };

        tX = new TextBox() { Left = 850, Top = 20, Width = 100, ReadOnly = true };
        tY = new TextBox() { Left = 850, Top = 50, Width = 100, ReadOnly = true };
        tZ = new TextBox() { Left = 850, Top = 80, Width = 100, ReadOnly = true };

        tX.TabStop = false;
        tY.TabStop = false;
        tZ.TabStop = false;

        this.Controls.Add(labelX);
        this.Controls.Add(labelY);
        this.Controls.Add(labelZ);
        this.Controls.Add(tX);
        this.Controls.Add(tY);
        this.Controls.Add(tZ);

        tdirX = new TextBox() { Left = 950, Top = 20, Width = 100, ReadOnly = true };
        tdirY = new TextBox() { Left = 950, Top = 50, Width = 100, ReadOnly = true };
        tdirZ = new TextBox() { Left = 950, Top = 80, Width = 100, ReadOnly = true };

        tdirX.TabStop = false;
        tdirY.TabStop = false;
        tdirZ.TabStop = false;

        this.Controls.Add(tdirX);
        this.Controls.Add(tdirY);
        this.Controls.Add(tdirZ);
    }

    private void UpdateCameraText()
    {
        var pos = render.camera.Position;
        tX.Text = pos.X.ToString("F2");
        tY.Text = pos.Y.ToString("F2");
        tZ.Text = pos.Z.ToString("F2");


        var dir = render.camera.Forward;
        tdirX.Text = dir.X.ToString("F2");
        tdirY.Text = dir.Y.ToString("F2");
        tdirZ.Text = dir.Z.ToString("F2");
    }
}
