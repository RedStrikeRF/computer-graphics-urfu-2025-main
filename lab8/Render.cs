using System;
using System.Drawing;
using System.Collections.Generic;

class Render
{
    public CameraController camera;
    public Color backgroundColor = Color.Blue; // Цвет фона
    private int width, height;
    private const float ambient = 0.2f; // Фоновое освещение
    private List<Shape3D> objects;
    private List<Vector3> lightDirs;

    public Render(int width, int height)
    {
        this.width = width;
        this.height = height;

        camera = new CameraController(width, height);

        objects = new List<Shape3D>();
        objects.Add(new Sphere(new Vector3(0, 0, 3), 1.0f, Color.Green, 0.7f, 64));
        objects.Add(new Sphere(new Vector3(2, 3, 5), 1.0f, Color.Red, 0.9f, 64));
        objects.Add(new Sphere(new Vector3(6, 1, 5), 2.0f, Color.Blue, 0.7f, 32, 0f, 0.8f));
        objects.Add(new Sphere(new Vector3(2, 3, 3), 1.0f, Color.Blue, 0.7f, 32, 0.5f));
        objects.Add(new Cube(new Vector3(2, 0, 1), 1.0f, Color.White, 0.99f, 128));
        objects.Add(new Cube(new Vector3(3, 0, 2), 1.0f, Color.Gray, 0.9f, 32));
        objects.Add(new Plane(new Vector3(0, 0, 0), new Vector3(0, 1, 0), Color.DarkCyan, 0.8f, 32));

        lightDirs = new List<Vector3>();
        lightDirs.Add(new Vector3(-1, -1, 1).Normalize());
        // lightDirs.Add(new Vector3(1, -1, 1).Normalize());
        // lightDirs.Add(new Vector3(1, -1, -1).Normalize());
        // lightDirs.Add(new Vector3(-1.1f, -1, 1).Normalize());
        // lightDirs.Add(new Vector3(-0.9f, -1, 1).Normalize());
        // lightDirs.Add(new Vector3(-1, -1.1f, 1).Normalize());
        // lightDirs.Add(new Vector3(-1, -0.9f, 1).Normalize());
        // lightDirs.Add(new Vector3(1, 1, -1).Normalize());
    }

    public Bitmap RenderScene()
    {
        var bmp = new Bitmap(width, height);

        camera.Update();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var rayGlobal = camera.GetRay(x, y, width, height);
                bmp.SetPixel(x, y, TraceRay(camera.Position, rayGlobal, 0));
            }
        }

        return bmp;
    }

    public Shape3D FindNearestIntersection(Vector3 rayOrigin, Vector3 rayDir, out float closestT)
    {
        closestT = float.MaxValue;
        Shape3D closestObject = null;

        foreach (var obj in objects)
        {
            float t = obj.Intersect(rayOrigin, rayDir);

            if (t < closestT && t > 0.001f)
            {
                closestT = t;
                closestObject = obj;
            }
        }

        return closestObject;
    }

    public bool InShadow(Vector3 point, Vector3 lightDirection, List<Shape3D> objects, Shape3D currentObj)
    {
        var shadowPoint = point + lightDirection * 0.001f;

        foreach (var obj in objects)
        {
            // if (obj == currentObj)
            //     continue;

            if (obj.Intersect(shadowPoint, lightDirection) > 0.001f)
                return true;
        }

        return false;
    }

    public static Color GammaCorrection(Color color, float intensity, float gamma = 0.45f)
    {
        float r = color.R / 255f;
        float g = color.G / 255f;
        float b = color.B / 255f;

        int red = CorrectChannel(r, intensity, gamma);
        int green = CorrectChannel(g, intensity, gamma);
        int blue = CorrectChannel(b, intensity, gamma);

        return Color.FromArgb(red, green, blue);
    }

    private static int CorrectChannel(float channel, float intensity, float gamma)
    {
        float corrected = (float)Math.Pow(Math.Clamp(channel * intensity, 0f, 1f), 1.0 / gamma);
        return (int)(corrected * 255);
    }

    private Color TraceRay(Vector3 origin, Vector3 rayGlobal, int depth)
    {
        if (depth > 2) return Color.Black;

        var obj = FindNearestIntersection(origin, rayGlobal, out float closestT);

        if (closestT == 0 || obj == null)
            return backgroundColor;

        Vector3 hitPoint = origin + rayGlobal * closestT;
        Vector3 normal = obj.NormalHitPoint(hitPoint);

        float lightIntensity = 0.0f;
        foreach (var lightDir in lightDirs)
        {
            if (InShadow(hitPoint, -lightDir, objects, obj))
            {
                if (lightIntensity == 0)
                    lightIntensity += ambient;
                continue;
            }

            // Освещенность
            float diffuse = Math.Max(0, normal.Dot(-lightDir));

            // Отражение
            Vector3 viewDir = (origin - hitPoint).Normalize();
            Vector3 reflectDir = (normal.Dot(-lightDir) * 2 * normal + lightDir).Normalize();

            float specular = (float)Math.Pow(Math.Max(0, viewDir.Dot(reflectDir)), obj.shininess);

            // Итоговое освещение
            lightIntensity += ambient + 0.6f * diffuse + obj.specularStrength * specular;
        }
        lightIntensity /= lightDirs.Count;

        lightIntensity = Math.Clamp(lightIntensity, 0, 1);

        // гамма коррекция
        var CorrectionColor = GammaCorrection(obj.color, lightIntensity, 0.9f);

        // прозрачность
        var transparency = obj.transparency;
        if (transparency > 0)
        {
            var transparencyColor = TraceRay(hitPoint, rayGlobal - normal * 0.001f, depth + 1);

            int r = (int)(CorrectionColor.R * (1 - transparency) + transparencyColor.R * transparency);
            int g = (int)(CorrectionColor.G * (1 - transparency) + transparencyColor.G * transparency);
            int b = (int)(CorrectionColor.B * (1 - transparency) + transparencyColor.B * transparency);

            CorrectionColor = Color.FromArgb(r, g, b);
        }

        // отражения
        var reflectivity = obj.reflectivity;
        if (reflectivity > 0)
        {
            Vector3 reflectDir = Reflect(rayGlobal, normal).Normalize();
            var reflectColor = TraceRay(hitPoint + normal * 0.001f, reflectDir, depth + 1);

            int r = (int)(CorrectionColor.R * (1 - reflectivity) + reflectColor.R * reflectivity);
            int g = (int)(CorrectionColor.G * (1 - reflectivity) + reflectColor.G * reflectivity);
            int b = (int)(CorrectionColor.B * (1 - reflectivity) + reflectColor.B * reflectivity);

            CorrectionColor = Color.FromArgb(r, g, b);
        }

        return CorrectionColor;

        // int r = (int)(obj.color.R * lightIntensity);
        // int g = (int)(obj.color.G * lightIntensity);
        // int b = (int)(obj.color.B * lightIntensity);

        // r = Math.Clamp(r, 0, 255);
        // g = Math.Clamp(g, 0, 255);
        // b = Math.Clamp(b, 0, 255);
        // return Color.FromArgb(r, g, b);
    }
    
    public static Vector3 Reflect(Vector3 incident, Vector3 normal)
    {
        return incident - 2 * incident.Dot(normal) * normal;
    }
}