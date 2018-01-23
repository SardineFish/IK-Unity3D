using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(IKGD))]
    public class IKGDEditor : IKEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var ik = target as IKGD;
            ik.Step = EditorGUILayout.FloatField("Step", ik.Step);
            ik.Delta = EditorGUILayout.FloatField("Delta", ik.Delta);
            ik.Iteration = EditorGUILayout.IntField("Iteration", ik.Iteration);
            if (GUILayout.Button("Start"))
            {
                iterator = IKGD.InverseKinematicsIterator(ik.Bones, ik.transform.position, ik.Step, ik.Delta, ik.Iteration).GetEnumerator();
                it = 0;
            }
        }

        float lastTime = 0;
        float dt = 0.1f;
        public IEnumerator<Quaternion[]> iterator;
        int it = 0;

        private void OnSceneGUI()
        {
            var ik = target as IKGD;
            if (Time.time - lastTime > dt)
            {
                lastTime = Time.time;
                if (iterator != null && iterator.MoveNext())
                {
                    Debug.Log(it++);
                    var rotations = iterator.Current;
                    for (var i = 0; i < rotations.Length; i++)
                    {
                        ik.Bones[i].transform.localRotation = rotations[i];
                    }
                }
            }
            Repaint();

        }
    }
}
