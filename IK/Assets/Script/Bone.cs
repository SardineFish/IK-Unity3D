using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Bone : MonoBehaviour
{
    public Vector3 InitialVector = new Vector3(1, 0, 0);
    public float Length = 1;
    public float Width = 0.1f;
    public bool Edit = true;
    public Bone[] SubBones = new Bone[0];
    public bool _showAsActive = false;

    public bool AngularLimit = true;
    public Range AngularLimitX = new Range(-180, 180);
    public Range AngularLimitY = new Range(-180, 180);
    public Range AngularLimitZ = new Range(-180, 180);
    
    public Vector3 DirectionVector
    {
        get
        {
            return transform.rotation * InitialVector;
        }
    }

    public Matrix4x4 BoneToWorldMatrix
    {
        get
        {
            return transform.localToWorldMatrix * Matrix4x4.Rotate(Quaternion.FromToRotation(Vector3.right, InitialVector));
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    [ExecuteInEditMode]
    // Update is called once per frame
    void Update()
    {

        ApplyAngularLimit();
        //Debug.DrawLine(transform.position, transform.position + DirectionVector * 10);
        if (SubBones != null && SubBones.Length > 0 && Edit)
        {
            this.InitialVector = Quaternion.Inverse(transform.rotation) * (SubBones[0].transform.position - transform.position);
            this.Length = InitialVector.magnitude;
        }
    }

    public void AddSubBone(Bone bone)
    {
        Array.Resize(ref SubBones, SubBones.Length + 1);
        SubBones[SubBones.Length - 1] = bone;
    }

    public void ApplyAngularLimit()
    {
        if (AngularLimit)
        {
            var angle = transform.localRotation.eulerAngles;
            angle.x = AngularLimitX.Limit(MathUtility.MapAngle(angle.x));
            angle.y = AngularLimitY.Limit(MathUtility.MapAngle(angle.y));
            angle.z = AngularLimitZ.Limit(MathUtility.MapAngle(angle.z));
            transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.Euler(angle);

        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        var mesh = new Mesh();
        var V = new Vector3[]
        {
                new Vector3(0, 0, 0),
                new Vector3(Width, Width, 0),
                new Vector3(Width, -Width, 0),
                new Vector3(Width, 0, Width),
                new Vector3(Width, 0, -Width),
                new Vector3(1,0,0),
        };
        for (var i = 0; i < V.Length; i++)
        {
            V[i] *= Length;
        }
        mesh.vertices = V;
        mesh.triangles = new int[] { 0, 1, 5, 0, 2, 5, 0, 3, 5, 0, 4, 5 };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        Color color;
        if (_showAsActive)
            ColorUtility.TryParseHtmlString("#F44336", out color);
        else
            ColorUtility.TryParseHtmlString("#66CCFF", out color);
        _showAsActive = false;
        Gizmos.color = color;
        Gizmos.DrawWireMesh(mesh, transform.position,
            Quaternion.FromToRotation(Vector3.right, transform.rotation * InitialVector));
    }


}
