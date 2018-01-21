using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class IK : MonoBehaviour
{
    public Bone StartBone;
    public Bone EndBone;
    public Bone[] Bones = new Bone[0];
}