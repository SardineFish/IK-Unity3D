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
}