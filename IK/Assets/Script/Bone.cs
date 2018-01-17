using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Bone : MonoBehaviour
{
    public Bone[] SubBones = new Bone[0];
    public float Width = 0.1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var bones = new List<Bone>();
        for (var i = 0; i < transform.childCount; i++)
        {
            var bone = transform.GetChild(i).GetComponent<Bone>();
            if (bone)
                bones.Add(bone);
        }
        SubBones = bones.ToArray();
    }
    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        foreach (var bone in SubBones)
        {
            var length = (bone.transform.position - transform.position).magnitude;
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
                V[i] *= length;
            }
            mesh.vertices = V;
            mesh.triangles = new int[] {0, 1, 5, 0, 2, 5, 0, 3, 5, 0, 4, 5};
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            Color color;
            ColorUtility.TryParseHtmlString("#66CCFF", out color);
            Gizmos.color = color;
            Gizmos.DrawWireMesh(mesh, transform.position,
                Quaternion.FromToRotation(Vector3.right, bone.transform.position - transform.position));
        }
    }
}
