using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

public partial class MainForm : Form
{
    private GLControl glControl;
    private ComboBox comboBoxAxis;
    private NumericUpDown numericUpDown;
    private Button buttonRotate;
    private Button buttonReset;
    private float angleX = 0;
    private float angleY = 0;
    private float angleZ = 0;
    private bool loaded = false;

    public MainForm()
    {
        this.Text = "Lab 6";
        this.Width = 800;
        this.Height = 600;

        var groupBoxControls = new GroupBox { Text = "Управление", Left = 10, Top = 10, Width = 200, Height = 500 };
        var groupBoxGL = new GroupBox { Text = "Рисование", Left = 220, Top = 10, Width = 550, Height = 500 };

        comboBoxAxis = new ComboBox { Left = 10, Top = 30, Width = 100 };
        comboBoxAxis.Items.AddRange(new string[] { "X", "Y", "Z" });
        comboBoxAxis.SelectedIndex = 0;
        groupBoxControls.Controls.Add(comboBoxAxis);

        numericUpDown = new NumericUpDown { Left = 10, Top = 70, Width = 100, Value = 3 };
        groupBoxControls.Controls.Add(numericUpDown);

        buttonRotate = new Button { Left = 10, Top = 110, Width = 180, Height = 30, Text = "Rotate" };
        buttonRotate.Click += ButtonRotate_Click;
        groupBoxControls.Controls.Add(buttonRotate);

        buttonReset = new Button { Left = 10, Top = 150, Width = 180, Height = 30, Text = "Angle = 0" };
        buttonReset.Click += ButtonReset_Click;
        groupBoxControls.Controls.Add(buttonReset);

        glControl = new GLControl { Left = 10, Top = 20, Width = 520, Height = 460, BackColor = System.Drawing.Color.Black };
        glControl.Load += GlControl_Load;
        glControl.Paint += GlControl_Paint;
        glControl.Resize += GlControl_Resize;
        groupBoxGL.Controls.Add(glControl);

        this.Controls.Add(groupBoxControls);
        this.Controls.Add(groupBoxGL);
    }

    private void GlControl_Load(object sender, EventArgs e)
    {
        loaded = true;
        SetupViewport(glControl);
    }

    private void GlControl_Resize(object sender, EventArgs e)
    {
        if (!loaded) return;
        SetupViewport(glControl);
        glControl.Invalidate();
    }

    private void GlControl_Paint(object sender, PaintEventArgs e)
    {
        if (!loaded) return;

        GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
        GL.Enable(EnableCap.DepthTest);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        var modelview = Matrix4.LookAt(
            new Vector3(-300, 300, 200),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 1)
        );

        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref modelview);

        GL.Rotate(angleX, 1, 0, 0);
        GL.Rotate(angleY, 0, 1, 0);
        GL.Rotate(angleZ, 0, 0, 1);

        Figures.DrawAxes();

        // Figures.DrawTriangle();
        // Figures.DrawCube(new Vector3(1.5f, 0f, -6f), 70.0f, PrimitiveType.Quads, Color.White);
        // Figures.DrawCube(new Vector3(1.5f, 0f, -6f), 70.0f, PrimitiveType.LineLoop, Color.Black);

        // Figures.DrawTruncatedPyramid(new Vector3(0f, -2.0f, -6f), 80.0f, 40.0f, 101.0f, PrimitiveType.LineLoop, Color.White);
        // Figures.DrawCone(new Vector3(-3.0f, -2.0f, -6f), 50.5f, 100.5f, 20, PrimitiveType.LineLoop, Color.Red);

        // красивое
        Figures.DrawTrefoilSurface(0.004f, PrimitiveType.LineLoop);
        // Figures.DrawCylinder(new Vector3(-3.0f, -2.0f, -6f), 50.5f, 100.5f, 20, PrimitiveType.LineLoop, Color.Red, 0, 0);

        // Figures.DrawSphere();
        // Figures.DrawParametricSurface2(0.01f, 0.005f, PrimitiveType.Points);
        glControl.SwapBuffers();
    }

    private void ButtonRotate_Click(object sender, EventArgs e)
    {
        switch (comboBoxAxis.Text)
        {
            case "X": angleX = (angleX + (float)numericUpDown.Value) % 360; break;
            case "Y": angleY = (angleY + (float)numericUpDown.Value) % 360; break;
            case "Z": angleZ = (angleZ + (float)numericUpDown.Value) % 360; break;
        }
        glControl.Invalidate();
    }

    private void ButtonReset_Click(object sender, EventArgs e)
    {
        angleX = 0;
        angleY = 0;
        angleZ = 0;
        glControl.Invalidate();
    }

    private void SetupViewport(GLControl glControl)
    {
        float aspectRatio = (float)glControl.Width / glControl.Height;
        GL.Viewport(0, 0, glControl.Width, glControl.Height);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadIdentity();

        Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1f, 5000000000);
        GL.MultMatrix(ref perspective);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadIdentity();
    }
}
