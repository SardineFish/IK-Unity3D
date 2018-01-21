using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FK : MonoBehaviour {
    public Bone StartBone;
    public Bone EndBone;
    public Bone[] Bones = new Bone[0];
    public Quaternion[] Rotations = new Quaternion[0];

	// Use this for initialization
	void Start () {
		
	}
	
    [ExecuteInEditMode]
	// Update is called once per frame
	void Update ()
    {
        if (Rotations.Length != Bones.Length)
        {
            Array.Resize(ref Rotations, Bones.Length);
        }
        for (var i = 0; i < this.Bones.Length; i++)
        {
            Rotations[i] = Bones[i].transform.localRotation;
        }
        this.transform.position = ForwardKinematics(Bones, Rotations);
	}

    public static Vector3 ForwardKinematics(Bone[] bones,Quaternion[] rotations)
    {
        if (bones.Length == 0)
            return Vector3.zero;
        var pos = bones[0].transform.position;
        for(var i = 0; i < bones.Length; i++)
        {
            bones[i].transform.localRotation = rotations[i];
            pos += bones[i].transform.rotation * bones[i].InitialVector;
            //Debug.DrawLine(bones[i].transform.position, bones[i].transform.position + bones[i].transform.rotation * bones[i].InitialVector*10, Color.black);
            
        }
        return pos;
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(StartBone.transform.position, EndBone.transform.position);

    }
    private void OnDrawGizmos()
    {
        float axisLength = 3;
        Color color;
        ColorUtility.TryParseHtmlString("#66CCFF", out color);
        Gizmos.color = color;
        Gizmos.DrawLine(transform.position + Vector3.left * axisLength, transform.position + Vector3.right * axisLength);
        Gizmos.DrawLine(transform.position + Vector3.forward * axisLength, transform.position + Vector3.back * axisLength);
        Gizmos.DrawLine(transform.position + Vector3.up * axisLength, transform.position + Vector3.down * axisLength);
    }
}
