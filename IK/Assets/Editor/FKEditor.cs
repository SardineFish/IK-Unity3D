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
        bool showBones = true;
        public void OnSceneGUI()
        {
            fk = target as FK;
            fk.StartBone._showAsActive = true;
            fk.EndBone._showAsActive = true;
            foreach (var b in fk.Bones)
            {
                b._showAsActive = true;
            }
            fk = target as FK;
        }
        private void OnEnable()
        {
            
        }
        
        public override void OnInspectorGUI()
        {
            fk = target as FK;
            var bone = EditorGUILayout.ObjectField("Start Bone", fk.StartBone, typeof(Bone), true) as Bone;
            if(bone != fk.StartBone)
            {
                fk.StartBone = bone;
                InitBones();
            }
            bone = EditorGUILayout.ObjectField("End Bone", fk.EndBone, typeof(Bone), true) as Bone;
            if(bone !=fk.EndBone)
            {
                fk.EndBone = bone;
                InitBones();
            }
            EditorGUILayout.Space();
            showBones = EditorGUILayout.Foldout(showBones, "Bones");
            if (showBones)
            {
                GUIStyle style = new GUIStyle();
                style.margin.left = 20;
                EditorGUILayout.BeginVertical(style);
                for(var i=0;i<fk.Bones.Length;i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(fk.Bones[i], typeof(Bone), true);
                    fk.Rotations[i].eulerAngles = EditorGUILayout.Vector3Field("", fk.Rotations[i].eulerAngles);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
        }

        public void InitBones()
        {
            if (fk.StartBone && fk.EndBone)
            {
                fk.Bones = SearchBones(fk.EndBone.transform, fk.StartBone).ToArray();
                fk.Rotations = new Quaternion[fk.Bones.Length];
                for(var i=0;i<fk.Bones.Length;i++)
                {
                    fk.Rotations[i] = fk.Bones[i].transform.localRotation;
                }
            }
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

        public static FK CreateFK(Bone endbone)
        {
            var obj = new GameObject("FK", typeof(FK));
            obj.GetComponent<FK>().EndBone = endbone;
            return obj.GetComponent<FK>();
        }
    }
}
