using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(FK))]
    class FKEditor : UnityEditor.Editor
    {
        FK fk;
        public void OnSceneGUI()
        {
            fk = target as FK;
            if(fk.StartBone && fk.EndBone)
            {
                fk.Bones = SearchBones(fk.EndBone.transform, fk.StartBone).ToArray();
            }
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        public List<Bone> SearchBones(Transform current,Bone target)
        {
            if (current.GetComponent<Bone>() == target)
                return new List<Bone>(new Bone[] { current.GetComponent<Bone>() });
            if (!current.transform.parent)
                throw new Exception("Cannot reach the Start Bone.");
            var next = SearchBones(current.parent, target);
            if (current.GetComponent<Bone>())
                next.Add(current.GetComponent<Bone>());
            return next;

        }
    }
}
