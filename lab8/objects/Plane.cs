using System;
using System.Drawing;

class Plane : Shape3D
{
    public Vector3 point;
    public Vector3 normal;

    public Plane(Vector3 point, Vector3 normal, Color color, float specularStrength = 0.5f, int shininess = 8, float transparency = 0, float reflectivity = 0)
    {
        this.point = point;
        this.normal = normal.Normalize();
        this.color = color;
        this.specularStrength = specularStrength;
        this.shininess = shininess;
        this.transparency = transparency;
        this.reflectivity = reflectivity;
    }

    public override float Intersect(Vector3 origin, Vector3 direction)
    {
        float denom = normal.Dot(direction);
        if (Math.Abs(denom) < 0.001f)
            return 0;

        float t = (point - origin).Dot(normal) / denom;
        if (t > 0.001f)
            return t;

        return 0;
    }

    public override Vector3 NormalHitPoint(Vector3 hitPoint)
    {
        return normal;
    }
}
