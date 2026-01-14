using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

partial class Game : GameWindow
{
    public Game()
        : base(800, 600, GraphicsMode.Default, "Lab 4")
    {
        VSync = VSyncMode.On;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        GL.Enable(EnableCap.DepthTest);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        if (Keyboard.GetState()[Key.Escape])
            Exit();
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        //Настройка вида экрана
        Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
        GL.MatrixMode(MatrixMode.Modelview);
        GL.LoadMatrix(ref modelview);

        Sierpinski.Render(5);

        // Mandelbrot.Render(800, 600, 100);
        // FractalTree3D.Render(7);

        SwapBuffers();
    }

    [STAThread]
    static void Main()
    {
        using (Game game = new Game())
        {
            game.Run(30.0);
        }
    }
}

