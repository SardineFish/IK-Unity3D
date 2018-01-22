using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[ExecuteInEditMode]
public class IKCCD: IK
{
    public int Iteration = 100;

    [ExecuteInEditMode]
    private void Update()
    {
        InverseKinematics(Bones, transform.position, Iteration);
    }
    public static Quaternion[] InverseKinematics(Bone[] bones,Vector3 target,int iteration)
    {
        var rotations = new Quaternion[bones.Length];
        var positions = new Vector3[bones.Length];
        var matrixs = new Matrix4x4[bones.Length];
        for (var i = 0; i < bones.Length; i++)
        {
            rotations[i] = bones[i].transform.rotation;
            positions[i] = bones[i].transform.position;
            if (i == 0)
            {
                matrixs[0] = bones[0].transform.worldToLocalMatrix;
            }
            else
                matrixs[i] = bones[i].transform.worldToLocalMatrix * Matrix4x4.Inverse(matrixs[i - 1]);
        }
        for (int iterate = 0; iterate < iteration; iterate++)
        {
            for (var i = bones.Length - 1; i >= 0; i--)
            {
                var endpoint = bones[bones.Length - 1].transform.position + bones[bones.Length - 1].DirectionVector;
                //bones[i].transform.Rotate(Quaternion.FromToRotation((endpoint - bones[i].transform.position).normalized, (target - bones[i].transform.position).normalized).eulerAngles, Space.World);
                var inverse = Quaternion.Inverse(bones[i].transform.rotation);
                var rotate = Quaternion.FromToRotation(inverse * (endpoint - bones[i].transform.position), inverse * (target - bones[i].transform.position));
                bones[i].transform.localRotation *= rotate;
                bones[i].ApplyAngularLimit();
            }
            /*
            var rotations = new Quaternion[bones.Length];
            for (var i = 0; i < bones.Length; i++)
                rotations[i] = bones[i].transform.localRotation;
            yield return rotations;*/
        }
        return rotations;
    }

    public static IEnumerator<ArrayList> InverseKinematicsIterate(Bone[] bones, Vector3 target, int iteration)
    {
        var rotations = new Quaternion[bones.Length];
        var positions = new Vector3[bones.Length];
        var matrixs = new Matrix4x4[bones.Length];
        var endpoint = bones[bones.Length - 1].transform.position + bones[bones.Length - 1].DirectionVector;
        var endDir = bones[bones.Length - 1].DirectionVector;
        for (var i = 0; i < bones.Length; i++)
        {
            rotations[i] = bones[i].transform.rotation;
            positions[i] = bones[i].transform.position;
            matrixs[i] = bones[i].transform.localToWorldMatrix;
        }
        for (int iterate = 0; iterate < iteration; iterate++)
        {
            for (var i = bones.Length - 1; i >= 0; i--)
            {
                //var endpoint = bones[bones.Length - 1].transform.position + bones[bones.Length - 1].DirectionVector;
                yield return new ArrayList(new object[]
                {
                    bones[i].transform.position,
                    bones[i].transform.position + (endpoint - bones[i].transform.position)*2,
                    target,
                    Quaternion.FromToRotation((endpoint - bones[i].transform.position).normalized, (target - bones[i].transform.position))
                });
                bones[i].transform.Rotate(Quaternion.FromToRotation((endpoint - bones[i].transform.position).normalized, (target - bones[i].transform.position).normalized).eulerAngles, Space.World);
                endpoint = bones[bones.Length - 1].transform.position + bones[bones.Length - 1].DirectionVector;
                yield return new ArrayList(new object[]
                {
                    bones[i].transform.position,
                    bones[i].transform.position + (endpoint - bones[i].transform.position)*2,
                    target,
                    Quaternion.FromToRotation((endpoint - bones[i].transform.position).normalized, (target - bones[i].transform.position))
                });
            }
            /*
            var rotations = new Quaternion[bones.Length];
            for (var i = 0; i < bones.Length; i++)
                rotations[i] = bones[i].transform.localRotation;
            yield return rotations;*/
        }
    }
}