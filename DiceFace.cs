using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DiceFace : MonoBehaviour
{
    private bool isUp;
    public Vector3 norm;
    public bool isNorNeg = false;
    public List<Transform> points = new List<Transform>();
    private float deg;
    public void FixedUpdate()
    {
        isUp = IsUp();
    }
    public bool IsUp()
    {
        Vector3 p1 = points[0].position;
        Vector3 p2 = points[1].position;
        Vector3 p3 = points[2].position;
        Vector3 v1 = p1 - p2;
        Vector3 v2 = p3 - p2;
        Vector3 nor = isNorNeg ? -Vector3.Cross(v1, v2) : Vector3.Cross(v1, v2);
        norm = nor.normalized;
        deg = Vector3.Angle(norm, Vector3.up);
        return deg < 45f;
    }

    public Vector3 normalize(Vector3 v)
    {
        float mod = v.magnitude;
        return new Vector3(v.x / mod, v.y / mod, v.z / mod);
    }
}
