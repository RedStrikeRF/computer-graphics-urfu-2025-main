using System;
using System.Drawing;


abstract class Shape3D
{
    public Color color;
    public float specularStrength;
    public int shininess;
    public float transparency;
    public float reflectivity;
    
    public abstract float Intersect(Vector3 origin, Vector3 direction);
    public abstract Vector3 NormalHitPoint(Vector3 hitPoint);
}