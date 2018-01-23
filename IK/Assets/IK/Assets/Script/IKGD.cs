using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IKGD : IK {
    public float Step = 5;
    public float Delta = 0.02f;
    public int Iteration = 100;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    [ExecuteInEditMode]
	void Update () {
        var rotations = InverseKinematics(Bones, transform.position, Step, Delta, Iteration);
        for(var i = 0; i < this.Bones.Length; i++)
        {
            Bones[i].transform.localRotation = rotations[i];
        }
	}

    public static Quaternion[] InverseKinematics(Bone[] bones,Vector3 position,float step, float delta,int iteration)
    {
        var rotations = new Quaternion[bones.Length];
        for (var i = 0; i < bones.Length; i++)
            rotations[i] = bones[i].transform.localRotation;
        var dFK = new float[bones.Length * 4];
        for(var iterate = 0; iterate < iteration; iterate++)
        {
            for(var i = 0; i < bones.Length * 4; i++)
            {
                dFK[i] = IKUtility.FKDistanceDerivate(bones, rotations, position, i, delta);
            }
            for (var i = 0; i < bones.Length * 4; i++)
            {
                int idxBone = i / 4;
                int idxQuaternion = i % 4;
                rotations[idxBone][idxQuaternion] -= step * dFK[i];
            }
        }
        return rotations;
    }

    public static IEnumerable<Quaternion[]> InverseKinematicsIterator(Bone[] bones, Vector3 position, float step, float delta, int iteration)
    {
        var rotations = new Quaternion[bones.Length];
        for (var i = 0; i < bones.Length; i++)
            rotations[i] = bones[i].transform.localRotation;
        var dFK = new float[bones.Length * 4];
        for (var iterate = 0; iterate < iteration; iterate++)
        {
            for (var i = 0; i < bones.Length * 4; i++)
            {
                dFK[i] = IKUtility.FKDistanceDerivate(bones, rotations, position, i, delta);
            }
            for (var i = 0; i < bones.Length * 4; i++)
            {
                int idxBone = i / 4;
                int idxQuaternion = i % 4;
                rotations[idxBone][idxQuaternion] -= step * dFK[i];
            }
            yield return rotations;
        }
    }
}
