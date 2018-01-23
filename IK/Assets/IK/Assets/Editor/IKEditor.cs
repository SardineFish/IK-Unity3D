using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(IK))]
    public class IKEditor:UnityEditor.Editor
    {
        IK ik;
        bool showBones = true;
        private void OnSceneGUI()
        {
            var ik = target as IK;
            foreach(var bone in ik.Bones)
            {
                bone._showAsActive = true;
            }
        }

        public override void OnInspectorGUI()
        {
            ik = target as IK;
            var bone = EditorGUILayout.ObjectField("Start Bone", ik.StartBone, typeof(Bone), true) as Bone;
            if (bone != ik.StartBone)
            {
                ik.StartBone = bone;
                InitBones();
            }
            bone = EditorGUILayout.ObjectField("End Bone", ik.EndBone, typeof(Bone), true) as Bone;
            if (bone != ik.EndBone)
            {
                ik.EndBone = bone;
                InitBones();
            }
            EditorGUILayout.Space();
            showBones = EditorGUILayout.Foldout(showBones, "Bones");
            if (showBones)
            {
                GUIStyle style = new GUIStyle();
                style.margin.left = 20;
                EditorGUILayout.BeginVertical(style);
                for (var i = 0; i < ik.Bones.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(ik.Bones[i], typeof(Bone), true);
                    EditorGUILayout.Vector3Field("", ik.Bones[i].transform.localRotation.eulerAngles);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }
        }

        public void InitBones()
        {
            ik = target as IK;
            if (ik.StartBone && ik.EndBone)
            {
                ik.Bones = SearchBones(ik.EndBone.transform, ik.StartBone).ToArray();
            }
        }

        public List<Bone> SearchBones(Transform current, Bone target)
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
