using System;
using System.Windows.Forms;
using System.Collections.Generic;


public static class KeyController
{
    public static HashSet<Keys> pressedKeys = new HashSet<Keys>();

    public static void KeyDown(Keys key) => pressedKeys.Add(key);
    public static void KeyUp(Keys key) => pressedKeys.Remove(key);
}

public class CameraController
{
    public Vector3 Position = new Vector3(1, 1.3f, -2);
    public Vector3 Forward = new Vector3(0, 0, 0);

    private float speed = 1f;
    private float rotationSpeed = 0.2f;

    private float cameraYaw = 0f;
    private float cameraPitch = 0f;


    float Dist;
    float vWidth;
    float vHeight;

    public CameraController(int width, int height)
    {
        Dist = 1;
        vWidth = 2.0f;
        vHeight = 2.0f * height / width;
    }

    public void Update()
    {
        var up = new Vector3(0, 1, 0);
        var right = Forward.Cross(up);

        var move = new Vector3(0, 0, 0);

        if (KeyController.pressedKeys.Contains(Keys.W)) move += Forward;
        if (KeyController.pressedKeys.Contains(Keys.S)) move -= Forward;
        if (KeyController.pressedKeys.Contains(Keys.A)) move -= right;
        if (KeyController.pressedKeys.Contains(Keys.D)) move += right;
        if (KeyController.pressedKeys.Contains(Keys.Q)) move += up;
        if (KeyController.pressedKeys.Contains(Keys.E)) move -= up;

        if (move.Length() != 0)
            Position += move.Normalize() * speed;

        if (KeyController.pressedKeys.Contains(Keys.D1)) cameraYaw -= rotationSpeed;
        if (KeyController.pressedKeys.Contains(Keys.D2)) cameraYaw += rotationSpeed;
        if (KeyController.pressedKeys.Contains(Keys.D3)) cameraPitch -= rotationSpeed;
        if (KeyController.pressedKeys.Contains(Keys.D4)) cameraPitch += rotationSpeed;

        cameraPitch = Math.Clamp(cameraPitch, -MathF.PI / 2 + 0.01f, MathF.PI / 2 - 0.01f);

        Forward = new Vector3(
            MathF.Cos(cameraPitch) * MathF.Sin(cameraYaw),
            MathF.Sin(cameraPitch),
            MathF.Cos(cameraPitch) * MathF.Cos(cameraYaw)
        ).Normalize();
    }

    public Vector3 GetRay(int x, int y, int width, int height)
    {

        float px = (x - width / 2.0f) * vWidth / width;
        float py = -(y - height / 2.0f) * vHeight / height;

        Vector3 rayLocalCamera = new Vector3(px, py, Dist).Normalize();

        var up = new Vector3(0, 1, 0);
        var right = Forward.Cross(up).Normalize();
        up = right.Cross(Forward).Normalize();

        Vector3 rayGlobal =
            (right * rayLocalCamera.X +
            up * rayLocalCamera.Y +
            Forward * rayLocalCamera.Z).Normalize();

        return rayGlobal;
    }
}
