using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

partial class Game : GameWindow
{
    public Game()
        : base(800, 600, GraphicsMode.Default, "Lab 2")
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
        Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4,
            Width / (float)Height, 1.0f, 64.0f);
        GL.MatrixMode(MatrixMode.Projection);
        GL.LoadMatrix(ref projection);
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

        DrawExample();

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
