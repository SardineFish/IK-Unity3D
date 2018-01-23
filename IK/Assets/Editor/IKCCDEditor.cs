using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Assets.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(IKCCD))]
    public class IKCCDEditor:IKEditor
    {
        IEnumerator<object[]> iterator;
        public override void OnInspectorGUI()
        {
            var ik = target as IKCCD;
            ik.Iteration = EditorGUILayout.IntField("Iteration", ik.Iteration);
            EditorGUILayout.Space();
            base.OnInspectorGUI();
            if (GUILayout.Button("Start"))
            {
                iterator = IKCCD.InverseKinematicsIterate(ik.Bones, ik.transform.position, ik.Iteration);
                it = 0;
            }
            if (GUILayout.Button("Next"))
            {
                iterator.MoveNext();
            }
            SceneView.RepaintAll();
        }
        float lastTime = 0;
        float dt = 0.1f;
        int it = 0;

        private void OnSceneGUI()
        {
            var ik = target as IKCCD;
            if(ik.Bones!=null && ik.Bones.Length > 0)
            {
                Color colorPink;
                ColorUtility.TryParseHtmlString("#F48FB1", out colorPink);
                Debug.DrawLine(ik.Bones[ik.Bones.Length-1].transform.position + ik.Bones[ik.Bones.Length - 1].DirectionVector, ik.transform.position, colorPink);
            }
            if (iterator==null || iterator.Current == null)
                return;
            var origin = (Vector3)iterator.Current[0];
            Debug.DrawLine(origin, origin + (Vector3)iterator.Current[1]);
            Debug.DrawLine(origin, origin + (Vector3)iterator.Current[2]);
            /*var rotate = (Quaternion)iterator.Current[3];
            Debug.DrawLine(origin, (Vector3)iterator.Current[1]);
            Debug.DrawLine(origin, (Vector3)iterator.Current[2]);
            var dir1 = (Vector3)iterator.Current[1] - (Vector3)iterator.Current[0];
            var dir2 = (Vector3)iterator.Current[2] - (Vector3)iterator.Current[0];
            //var rotate = Quaternion.FromToRotation(dir1, dir2);//IKUtility.QuaternionBetweenVector(dir1, dir2);
            dir1 = rotate * dir1;
            Debug.DrawLine((Vector3)iterator.Current[0], (Vector3)iterator.Current[0] + dir1 * 2,Color.red);*/
            /*var ik = target as IKCCD;
            if (Time.time - lastTime > dt)
            {
                lastTime = Time.time;
                if (iterator != null && iterator.MoveNext())
                {
                    Debug.Log(it++);
                }
            }
            Repaint();*/
        }

        public static IKCCD CreateIKCCD(Bone endbone)
        {
            var obj = new GameObject("IK CCD",typeof(IKCCD));
            var ik = obj.GetComponent<IKCCD>();
            ik.EndBone = endbone;
            ik.transform.position = endbone.transform.position + endbone.DirectionVector;
            return ik;
        }
    }
}
