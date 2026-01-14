using System;
using System.Drawing;

class Cube : Shape3D
{
    public Vector3 center;
    public Vector3 min;
    public Vector3 max;

    public Cube(Vector3 center, float size, Color color, float specularStrength = 0.7f, int shininess = 16, float transparency = 0, float reflectivity = 0)
    {
        this.center = center;
        float half = size / 2;
        this.min = new Vector3(center.X - half, center.Y - half, center.Z - half);
        this.max = new Vector3(center.X + half, center.Y + half, center.Z + half);

        this.color = color;
        this.specularStrength = specularStrength;
        this.shininess = shininess;
        this.transparency = transparency;
        this.reflectivity = reflectivity;
    }

    public override float Intersect(Vector3 origin, Vector3 direction)
    {
        float tMin = (min.X - origin.X) / direction.X;
        float tMax = (max.X - origin.X) / direction.X;
        if (tMin > tMax) (tMin, tMax) = (tMax, tMin);

        float tyMin = (min.Y - origin.Y) / direction.Y;
        float tyMax = (max.Y - origin.Y) / direction.Y;
        if (tyMin > tyMax) (tyMin, tyMax) = (tyMax, tyMin);

        if ((tMin > tyMax) || (tyMin > tMax))
            return 0;

        if (tyMin > tMin) tMin = tyMin;
        if (tyMax < tMax) tMax = tyMax;

        float tzMin = (min.Z - origin.Z) / direction.Z;
        float tzMax = (max.Z - origin.Z) / direction.Z;
        if (tzMin > tzMax) (tzMin, tzMax) = (tzMax, tzMin);

        if ((tMin > tzMax) || (tzMin > tMax))
            return 0;

        if (tzMin > tMin) tMin = tzMin;
        if (tzMax < tMax) tMax = tzMax;

        return tMin > 0.001f ? tMin : tMax > 0.001f ? tMax : 0;
    }

    public override Vector3 NormalHitPoint(Vector3 hitPoint)
    {
        if (Math.Abs(hitPoint.X - min.X) < 0.001f) return new Vector3(-1, 0, 0);
        if (Math.Abs(hitPoint.X - max.X) < 0.001f) return new Vector3(1, 0, 0);
        if (Math.Abs(hitPoint.Y - min.Y) < 0.001f) return new Vector3(0, -1, 0);
        if (Math.Abs(hitPoint.Y - max.Y) < 0.001f) return new Vector3(0, 1, 0);
        if (Math.Abs(hitPoint.Z - min.Z) < 0.001f) return new Vector3(0, 0, -1);
        if (Math.Abs(hitPoint.Z - max.Z) < 0.001f) return new Vector3(0, 0, 1);

        return new Vector3(0, 0, 0);
    }
}
