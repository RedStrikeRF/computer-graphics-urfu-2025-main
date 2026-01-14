using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;

public partial class MainForm : Form
{
    private GLControl glControl;
    private ComboBox comboBoxAxis;
    private NumericUpDown numericUpDown;
    private Button buttonRotate;
    private Button buttonReset;
    private float angleX = -90;
    private float angleY = 0;
    private float angleZ = 0;
    private bool loaded = false;

    private static Random rng = new Random();

    private CameraController camera = new CameraController();

    public MainForm()
    {
        this.Text = "Lab 7";
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

        this.KeyPreview = true;

        glControl.MouseMove += GlControl_MouseMove;
        glControl.KeyDown += GlControl_KeyDown;
        glControl.KeyUp += GlControl_KeyUp;

        groupBoxGL.Controls.Add(glControl);

        this.Controls.Add(groupBoxControls);
        this.Controls.Add(groupBoxGL);
    }

    private void GlControl_KeyDown(object sender, KeyEventArgs e)
    {
        camera.KeyDown(e.KeyCode);
        camera.Update();
        glControl.Invalidate();
    }

    private void GlControl_KeyUp(object sender, KeyEventArgs e)
    {
        camera.KeyUp(e.KeyCode);
        camera.Update();
        glControl.Invalidate();
    }

    private void GlControl_MouseMove(object sender, MouseEventArgs e)
    {
        camera.MouseMove(e.X, e.Y);
        glControl.Invalidate();
    }

    private void GlControl_Load(object sender, EventArgs e)
    {
        loaded = true;
        Figures.ListTextureId = LoadTexture("grass.jpg");
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
            camera.Position,
            camera.Target,
            Vector3.UnitY
        );
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref modelview);

        GL.Rotate(angleX, 1, 0, 0);
        GL.Rotate(angleY, 0, 1, 0);
        GL.Rotate(angleZ, 0, 0, 1);

        Figures.DrawAxes();

        // Random rng = new Random();
        DrawTree(
            position: new Vector3(100, 0, 0),
            radius: 4f,
            height: 70f,
            segments: 7,
            depth: 9
        );
        // Figures.DrawList();
        Figures.DrawListGrid(10, 100);
        // DrawTree(
        //     position: new Vector3(0, 0, 0),
        //     radius: 2f,
        //     height: 70f,
        //     segments: 7,
        //     depth: 7
        // );


        // Tree.DrawTreeFromSegments(treeSegments);

        // Figures.DrawTexturedTriangle(textureId);
        // Figures.DrawCylinder(new Vector3(-3.0f, -2.0f, -6f), 50.5f, 100.5f, 20, PrimitiveType.Quads, Color.Red);

        // Figures.DrawTriangle();
        // Figures.DrawCube(new Vector3(1.5f, 0f, -6f), 70.0f, PrimitiveType.Quads, Color.White);
        // Figures.DrawCube(new Vector3(1.5f, 0f, -6f), 70.0f, PrimitiveType.LineLoop, Color.Black);

        // Figures.DrawTruncatedPyramid(new Vector3(0f, -2.0f, -6f), 80.0f, 40.0f, 101.0f, PrimitiveType.LineLoop, Color.White);
        // Figures.DrawCone(new Vector3(-3.0f, -2.0f, -6f), 50.5f, 100.5f, 20, PrimitiveType.LineLoop, Color.Red);

        // красивое
        // Figures.DrawTrefoilSurface(0.004f, PrimitiveType.LineLoop);

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
        angleX = -90;
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

    public static int LoadTexture(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл текстуры не найден: {filePath}");

        int textureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureId);

        using (Bitmap bitmap = new Bitmap(filePath))
        {
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb
            );

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                data.Width,
                data.Height,
                0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0
            );

            bitmap.UnlockBits(data);
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        return textureId;
    }

    public static void DrawTree(Vector3 position, float radius, float height, int segments, int depth, int iter = 0)
    {
        if (depth <= 1)
        {
            if (iter == 1)
                Figures.DrawCube(Vector3.Zero, 10, PrimitiveType.Quads, Color.FromArgb(60, 0, 200, 0));
            else
                Figures.DrawCube(Vector3.Zero, 2, PrimitiveType.Quads, Color.FromArgb(178, 0, 120, 0));
            return;
        }

        GL.PushMatrix();
        GL.Translate(position);

        // Figures.DrawCube(Vector3.Zero, 2, PrimitiveType.Quads, Color.DarkGreen);
            
        // if (depth == 6)
        //     Figures.DrawCube(Vector3.Zero, 30, PrimitiveType.Quads, Color.FromArgb(32, 0, 200, 0));

        Figures.DrawCylinder(
            center: Vector3.Zero,
            radius: radius,
            height: height,
            segments: segments,
            drawType: PrimitiveType.Quads,
            color: Color.SaddleBrown,
            rotateX: 0,
            rotateY: 0
        );

        Vector3 top = new Vector3(0, 0, height);

        int branches = 3;
        var anglesX = new int[] { 30, -25, 10 };
        var anglesY = new int[] { 20, -15, -30 };
        for (int i = 0; i < branches; i++)
        {
            float newHeight = height * (0.6f + 1 * 0.2f); 
            float newRadius = radius * 0.7f;

            GL.PushMatrix();
            GL.Translate(top);
            GL.Rotate(anglesX[i], 1, 0, 0);
            GL.Rotate(anglesY[i], 0, 1, 0);

            DrawTree(Vector3.Zero, newRadius, newHeight, segments, depth - 1, i);
            GL.PopMatrix();
        }

        GL.PopMatrix();
    }
}
