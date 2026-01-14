using System;
using System.Drawing;
using System.Windows.Forms;

public partial class RaycastingForm : Form
{
    private PictureBox pictureBox;
    private Render render;

    public RaycastingForm()
    {
        this.Text = "Lab 8. Raycasting 3D";
        this.Width = 1200;
        this.Height = 600;

        pictureBox = new PictureBox();
        pictureBox.Width = 800;
        pictureBox.Height = 600;
        this.Controls.Add(pictureBox);

        render = new Render(pictureBox.Width, pictureBox.Height);

        this.Load += (s, e) =>
            pictureBox.Image = render.RenderScene();

        this.KeyDown += GlControl_KeyDown;
        this.KeyUp += GlControl_KeyUp;

        AddCameraUI();
        UpdateCameraText();
    }

    private void GlControl_KeyDown(object sender, KeyEventArgs e)
    {
        KeyController.KeyDown(e.KeyCode);
        pictureBox.Image = render.RenderScene();
        UpdateCameraText();
    }

    private void GlControl_KeyUp(object sender, KeyEventArgs e)
    {
        KeyController.KeyUp(e.KeyCode);
        pictureBox.Image = render.RenderScene();
        UpdateCameraText();
    }

    [STAThread]
    public static void Main()
    {
        Application.Run(new RaycastingForm());
    }
}
