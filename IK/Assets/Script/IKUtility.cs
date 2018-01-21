using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static  class IKUtility
{
    public static float FKDistance(Bone[] bones, Quaternion[] rotations, Vector3 target)
    {
        return (FK.ForwardKinematics(bones, rotations) - target).magnitude;
    }

    public static float FKDistanceDerivate(Bone[] bones,Quaternion[] rotations,Vector3 target, int idx,float delta)
    {
        int idxBone = idx / 4;
        int idxQuaternion = idx % 4;
        var y1 = FKDistance(bones, rotations, target);
        rotations[idxBone][idxQuaternion] += delta;
        var y2 = FKDistance(bones, rotations, target);
        return (y2 - y1)/delta;
    }

    public static Quaternion QuaternionBetweenVector(Vector3 u,Vector3 v)
    {
        u = u.normalized;
        v = v.normalized;
        var cosOfAng = Vector3.Dot(u, v);
        var halfCos = Mathf.Sqrt(0.5f * (1.0f + cosOfAng));
        var halfSin = Mathf.Sqrt(0.5f * (1.0f - cosOfAng));
        var axis = Vector3.Cross(u, v);
        var quaternion = new Quaternion(halfSin * axis.x, halfSin * axis.y, halfSin * axis.z, halfCos);
        return quaternion;
    }
}