using System;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;


public class CameraController
{
    public Vector3 Position = new Vector3(100, 100, 100);
    public Vector3 Target = Vector3.Zero;

    private HashSet<Keys> pressedKeys = new HashSet<Keys>();
    private float speed = 5f;

    private float yaw = -90f;
    private float pitch = 0f;
    private bool firstMouse = true;
    private float lastX, lastY;
    private float sensitivity = 0.2f;

    public void KeyDown(Keys key) => pressedKeys.Add(key);
    public void KeyUp(Keys key) => pressedKeys.Remove(key);

    public void MouseMove(float x, float y)
    {
        if ((Control.MouseButtons & MouseButtons.Left) == 0)
        {
            firstMouse = true;
            return;
        }
        if (firstMouse)
        {
            lastX = x;
            lastY = y;
            firstMouse = false;
            return;
        }

        float offsetX = (x - lastX) * sensitivity;
        float offsetY = (lastY - y) * sensitivity;

        lastX = x;
        lastY = y;

        yaw += offsetX;
        pitch += offsetY;

        UpdateTarget();
    }

    private void UpdateTarget()
    {
        var direction = new Vector3(
            (float)(Math.Cos(MathHelper.DegreesToRadians(yaw)) * Math.Cos(MathHelper.DegreesToRadians(pitch))),
            (float)(Math.Sin(MathHelper.DegreesToRadians(pitch))),
            (float)(Math.Sin(MathHelper.DegreesToRadians(yaw)) * Math.Cos(MathHelper.DegreesToRadians(pitch)))
        );
        Target = Position + direction;
    }

    public void Update()
    {
        var forward = Target - Position;
        var right = Vector3.Cross(forward, Vector3.UnitY);
        var move = Vector3.Zero;

        if (pressedKeys.Contains(Keys.W)) move += forward;
        if (pressedKeys.Contains(Keys.S)) move -= forward;
        if (pressedKeys.Contains(Keys.A)) move -= right;
        if (pressedKeys.Contains(Keys.D)) move += right;
        if (pressedKeys.Contains(Keys.Q)) move += Vector3.UnitY;
        if (pressedKeys.Contains(Keys.E)) move -= Vector3.UnitY;

        if (move.LengthSquared > 0)
        {
            Position += move * speed;
            Target += move * speed;
        }
    }
}
