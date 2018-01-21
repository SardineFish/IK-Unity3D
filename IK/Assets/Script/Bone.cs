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
    public Vector3 DirectionVector
    {
        get
        {
            return transform.rotation * InitialVector;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position, transform.position + DirectionVector * 10);
        if(SubBones.Length>0 && Edit)
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
