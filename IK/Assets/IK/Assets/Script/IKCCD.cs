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
    public float[] Weights = new float[0];

    [ExecuteInEditMode]
    private void Update()
    {
        /*var r = Quaternion.Euler(45, 0, 0);
        Quaternion.*/
        if (Bones.Length != Weights.Length)
        {
            var lengthOld = Weights.Length;
            Array.Resize(ref Weights, Bones.Length);
            for (var i = lengthOld; i < Bones.Length; i++)
                Weights[i] = 1;
        }

        var rotations = InverseKinematics(Bones, Weights, transform.position, Iteration);
        for (var i = 0; i < this.Bones.Length; i++)
        {
            if (i == 0)
                Bones[i].transform.rotation = rotations[i];
            else
                Bones[i].transform.localRotation = rotations[i];
            //Bones[i].ApplyAngularLimit();
        }

    }
    public static Quaternion[] InverseKinematics(Bone[] bones,float[] weights,Vector3 target,int iteration)
    {

        // Make matrixs to transfrom P(i)->P(i+1)
        var matrixs = new Matrix4x4[bones.Length];
        for (var i = 0; i < bones.Length; i++)
        {
            if (i == 0)
            {
                matrixs[0] = bones[0].transform.localToWorldMatrix;
            }
            else
                matrixs[i] = Matrix4x4.Inverse( bones[i - 1].transform.localToWorldMatrix) * bones[i].transform.localToWorldMatrix;
        }

        for (int iterate = 0; iterate < iteration; iterate++)
        {
            for (var i = bones.Length - 1; i >= 0; i--)
            {
                var localEndpoint = bones[bones.Length - 1].InitialVector;

                // Map the end point to current space
                var m = Matrix4x4.identity;
                if (i < bones.Length - 1)
                {
                    for (var j = i + 1; j < bones.Length; j++)
                    {
                        m = m * matrixs[j];
                    }
                }
                localEndpoint = m.MultiplyPoint(localEndpoint);

                // Map the target to current space
                m = Matrix4x4.identity;
                for (var j = 0; j <= i; j++)
                {
                    m = m * matrixs[j];
                }
                var localTarget = Matrix4x4.Inverse(m).MultiplyPoint(target);

                // Calculate the delta rotation
                var rotate = Quaternion.FromToRotation(localEndpoint, localTarget);

                // Apply the angular limit to total rotation and get the final delta rotation
                var finalRotation = bones[i].ApplyAngularLimit(matrixs[i].rotation * rotate);

                // Apply the rotation to the matrix
                // Remove the current rotation of the matrix and apply the final rotation.
                matrixs[i] = matrixs[i] * Matrix4x4.Inverse(Matrix4x4.Rotate(matrixs[i].rotation)) * Matrix4x4.Rotate(Quaternion.Lerp(matrixs[i].rotation, finalRotation, weights[i]));

            }
        }
        var rotations = new Quaternion[bones.Length];
        for (var i = 0; i < bones.Length; i++)
            rotations[i] = matrixs[i].rotation;
        return rotations;
    }

    public static IEnumerator<object[]> InverseKinematicsIterate(Bone[] bones, Vector3 target, int iteration)
    {
        var rotations = new Quaternion[bones.Length];
        var matrixs = new Matrix4x4[bones.Length];
        for (var i = 0; i < bones.Length; i++)
        {
            rotations[i] = bones[i].transform.localRotation;
            if (i == 0)
            {
                matrixs[0] = bones[0].transform.localToWorldMatrix;
            }
            else
                matrixs[i] = Matrix4x4.Inverse(bones[i - 1].transform.localToWorldMatrix) * bones[i].transform.localToWorldMatrix;
        }

        for (int iterate = 0; iterate < iteration; iterate++)
        {
            for (var i = bones.Length - 1; i >= 0; i--)
            {
                var localEndpoint = bones[bones.Length - 1].InitialVector;

                // Map the end point to current space
                var m = Matrix4x4.identity;
                if (i < bones.Length - 1)
                {
                    for (var j = i + 1; j < bones.Length; j++)
                    {
                        m = m * matrixs[j];
                    }
                }
                localEndpoint = m.MultiplyPoint(localEndpoint);

                // Map the target to current space
                m = Matrix4x4.identity;
                for (var j = 0; j <= i; j++)
                {
                    m = m * matrixs[j];
                }
                var localTarget = Matrix4x4.Inverse(m).MultiplyPoint(target);

                // from current to world
                m = Matrix4x4.identity;
                for (var j = 0; j <= i; j++)
                {
                    m = m * matrixs[j];
                }
                yield return new object[]
                {
                    m.MultiplyPoint(Vector3.zero),
                    m.MultiplyVector(localEndpoint),
                    m.MultiplyVector(localTarget)
                };

                // Calculate the delta rotation
                var rotate = Quaternion.FromToRotation(localEndpoint, localTarget);

                // Apply the angular limit to total rotation and get the final delta rotation
                //rotate = bones[i].ApplyAngularLimit(matrixs[i].rotation * rotate) * Quaternion.Inverse(matrixs[i].rotation);

                // Apply the rotation to the matrix
                matrixs[i] *= Matrix4x4.Rotate(rotate);
                bones[i].transform.localRotation *= rotate;

                m = Matrix4x4.identity;
                for (var j = 0; j <= i; j++)
                {
                    m = m * matrixs[j];
                }

                yield return new object[]
                {
                    m.MultiplyPoint(Vector3.zero),
                    m.MultiplyVector(localEndpoint),
                    m.MultiplyVector(localTarget)
                };

            }
            /*
            var rotations = new Quaternion[bones.Length];
            for (var i = 0; i < bones.Length; i++)
                rotations[i] = bones[i].transform.localRotation;
            yield return rotations;*/
        }
        for (var i = 0; i < bones.Length; i++)
            bones[i].transform.localRotation = matrixs[i].rotation;
    }
}