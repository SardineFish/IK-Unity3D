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

        public void OnSceneGUI()
        {
            var bone = target as Bone;
            var colorRed = Color.red;
            var colorBlue = Color.blue;
            var colorGreen = Color.green;
            colorRed.a = colorBlue.a = colorGreen.a = 0.2f;
            var matrix = bone.transform.localToWorldMatrix * Matrix4x4.Rotate(Quaternion.Inverse(bone.transform.localRotation)) * Matrix4x4.Rotate( Quaternion.FromToRotation(Vector3.right, bone.InitialVector));
            Handles.DrawLine(bone.transform.position, bone.transform.position + (matrix * Vector3.right).ToVector3());

            // Draw angular x
            Handles.color = colorRed;
            Handles.DrawSolidArc(bone.transform.position, matrix * Vector3.right,matrix * (Quaternion.Euler(bone.AngularLimitX.low, 0,0)*new Vector3(0,1,0)) , bone.AngularLimitX.length, 0.2f);
            colorRed.a =1;
            Handles.color = colorRed;
            Handles.DrawLine(bone.transform.position, bone.transform.position + (matrix * (Quaternion.Euler(bone.transform.localRotation.eulerAngles.x,0,0)* new Vector3(0, 1, 0))).ToVector3()*0.2f);
            
            // Draw angular y
            Handles.color = colorGreen;
            Handles.DrawSolidArc(bone.transform.position, matrix * Vector3.up, matrix * (Quaternion.Euler(0, bone.AngularLimitY.low, 0) * new Vector3(1, 0, 0)), bone.AngularLimitY.length, 0.2f);
            colorGreen.a = 1;
            Handles.color = colorGreen;
            Handles.DrawLine(bone.transform.position, bone.transform.position + (matrix * (Quaternion.Euler(0, bone.transform.localRotation.eulerAngles.y, 0) * new Vector3(1, 0, 0))).ToVector3() * 0.2f);

            // Draw angular z
            Handles.color = colorBlue;
            Handles.DrawSolidArc(bone.transform.position, matrix * Vector3.forward, matrix * (Quaternion.Euler(0, 0, bone.AngularLimitZ.low) * new Vector3(1, 0, 0)), bone.AngularLimitZ.length, 0.2f);
            colorBlue.a = 1;
            Handles.color = colorBlue;
            Handles.DrawLine(bone.transform.position, bone.transform.position + (matrix * (Quaternion.Euler(0, 0, bone.transform.localRotation.eulerAngles.z    ) * new Vector3(1, 0, 0))).ToVector3() * 0.2f);
            //Handles.DrawSolidArc(bone.transform.position, bone.transform.forward, bone.transform.right, bone.angularLimitX.low, 0.2f);

        }


        public override void OnInspectorGUI()
        {
            var bone = target as Bone;
            bone.AngularLimit = EditorGUILayout.Toggle("Angular Limit", bone.AngularLimit);
            bone.AngularLimitX = EditorUtility.DrawRangeField("Angular Limit x", bone.AngularLimitX);
            bone.AngularLimitY = EditorUtility.DrawRangeField("Angular Limit y", bone.AngularLimitY);
            bone.AngularLimitZ = EditorUtility.DrawRangeField("Angular Limit z", bone.AngularLimitZ);
            EditorGUILayout.Space();
            bone . InitialVector = EditorGUILayout.Vector3Field("Initial Vector", bone.InitialVector);
            EditorGUILayout.Space();
            EditorUtility.DrawFoldList("SubBones", true, bone.SubBones.Length, (i) =>
               {
                   EditorGUILayout.ObjectField(bone.SubBones[i], typeof(Bone), true);
               });

        }
    }
}
