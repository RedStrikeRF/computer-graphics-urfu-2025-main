using System;

public struct Vector3
{
    public float X, Y, Z;

    public Vector3(float x, float y, float z) { X = x; Y = y; Z = z; }

    public static Vector3 operator +(Vector3 a, Vector3 b) =>
        new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3 operator -(Vector3 a, Vector3 b) =>
        new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vector3 operator *(Vector3 a, float s) =>
        new Vector3(a.X * s, a.Y * s, a.Z * s);

    public static Vector3 operator -(Vector3 v) =>
        new Vector3(-v.X, -v.Y, -v.Z);

    public static Vector3 operator *(float s, Vector3 a) =>
        new Vector3(a.X * s, a.Y * s, a.Z * s);



    public float Dot(Vector3 b) => X * b.X + Y * b.Y + Z * b.Z;

    public float Length() => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

    public Vector3 Normalize()
    {
        float len = Length();
        return new Vector3(X / len, Y / len, Z / len);
    }

    public Vector3 Cross(Vector3 other) =>
        new Vector3(
            Y * other.Z - Z * other.Y,
            Z * other.X - X * other.Z,
            X * other.Y - Y * other.X
        );
}

