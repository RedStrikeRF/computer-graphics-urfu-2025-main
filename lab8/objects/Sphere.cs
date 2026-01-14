using System;
using System.Drawing;

class Sphere : Shape3D
{
    public Vector3 center;
    public float radius;

    public Sphere(Vector3 center, float radius, Color color, float specularStrength = 0.7f, int shininess = 16, float transparency = 0, float reflectivity = 0)
    {
        this.center = center;
        this.radius = radius;
        this.color = color;
        this.specularStrength = specularStrength;
        this.shininess = shininess;
        this.transparency = transparency;
        this.reflectivity = reflectivity;
    }

    public override float Intersect(Vector3 origin, Vector3 direction)
    {
        Vector3 oc = origin - center;
        float a = direction.Dot(direction);
        float b = 2.0f * oc.Dot(direction);
        float c = oc.Dot(oc) - radius * radius;
        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0)
            return 0;

        return (-b - (float)Math.Sqrt(discriminant)) / (2.0f * a);
    }

    public override Vector3 NormalHitPoint(Vector3 hitPoint)
    {
        return (hitPoint - center).Normalize();
    }
}