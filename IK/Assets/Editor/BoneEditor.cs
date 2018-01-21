using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Bone))]
    public class BoneEditor : UnityEditor.Editor
    {
        public static Bone CreateBone(GameObject parent)
        {
            var obj = new GameObject("Bone", typeof(Bone));
            if (parent.GetComponent<Bone>())
            {
                var bone = parent.GetComponent<Bone>();
                obj.transform.position = bone.transform.position + (bone.transform.rotation * bone.InitialVector);
                bone.AddSubBone(obj.GetComponent<Bone>());
            }
            obj.transform.parent = parent.transform;
            return obj.GetComponent<Bone>();
        }
    }
}
