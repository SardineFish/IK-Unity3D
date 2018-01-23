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

        private void OnEnable()
        {
            UpdateSubBones(target as Bone);
        }

        public void OnSceneGUI()
        {
            var bone = target as Bone;
            var colorRed = Color.red;
            var colorBlue = Color.blue;
            var colorGreen = Color.green;
            colorRed.a = colorBlue.a = colorGreen.a = 0.2f;
            var radius = bone.Length * 0.3f;
            var matrix = bone.transform.localToWorldMatrix * Matrix4x4.Rotate(Quaternion.Inverse(bone.transform.localRotation)) * Matrix4x4.Rotate( Quaternion.FromToRotation(Vector3.right, bone.InitialVector));
            //Handles.DrawLine(bone.transform.position, bone.transform.position + (matrix * Vector3.right).ToVector3());

            // Draw angular x
            Handles.color = colorRed;
            Handles.DrawSolidArc(bone.transform.position, matrix * Vector3.right,matrix * (Quaternion.Euler(bone.AngularLimitX.low, 0,0)*new Vector3(0,1,0)) , bone.AngularLimitX.length, radius);
            colorRed.a =1;
            Handles.color = colorRed;
            Handles.DrawLine(bone.transform.position, bone.transform.position + (matrix * (Quaternion.Euler(bone.transform.localRotation.eulerAngles.x,0,0)* new Vector3(0, 1, 0))).ToVector3()* radius);
            
            // Draw angular y
            Handles.color = colorGreen;
            Handles.DrawSolidArc(bone.transform.position, matrix * Vector3.up, matrix * (Quaternion.Euler(0, bone.AngularLimitY.low, 0) * new Vector3(1, 0, 0)), bone.AngularLimitY.length, radius);
            colorGreen.a = 1;
            Handles.color = colorGreen;
            Handles.DrawLine(bone.transform.position, bone.transform.position + (matrix * (Quaternion.Euler(0, bone.transform.localRotation.eulerAngles.y, 0) * new Vector3(1, 0, 0))).ToVector3() * radius);

            // Draw angular z
            Handles.color = colorBlue;
            Handles.DrawSolidArc(bone.transform.position, matrix * Vector3.forward, matrix * (Quaternion.Euler(0, 0, bone.AngularLimitZ.low) * new Vector3(1, 0, 0)), bone.AngularLimitZ.length, radius);
            colorBlue.a = 1;
            Handles.color = colorBlue;
            Handles.DrawLine(bone.transform.position, bone.transform.position + (matrix * (Quaternion.Euler(0, 0, bone.transform.localRotation.eulerAngles.z    ) * new Vector3(1, 0, 0))).ToVector3() * radius);
            //Handles.DrawSolidArc(bone.transform.position, bone.transform.forward, bone.transform.right, bone.angularLimitX.low, 0.2f);

        }


        public override void OnInspectorGUI()
        {
            var bone = target as Bone;
            bone.Length = bone.InitialVector.magnitude;
            bone.Length = Mathf.Abs(EditorGUILayout.FloatField("Length", bone.Length));
            if (bone.Length > 0)
                bone.InitialVector *= bone.Length / bone.InitialVector.magnitude;
            bone.Width = EditorGUILayout.FloatField("Width", bone.Width);
            bone.MaxRotationSpeed = Mathf.Abs(EditorGUILayout.FloatField("Max Rotation Speed",bone.MaxRotationSpeed));
            bone.AngularLimit = EditorGUILayout.Toggle("Angular Limit", bone.AngularLimit);
            bone.AngularLimitX = EditorUtility.DrawAngularRangeField("Angular Limit x", bone.AngularLimitX);
            bone.AngularLimitY = EditorUtility.DrawAngularRangeField("Angular Limit y", bone.AngularLimitY);
            bone.AngularLimitZ = EditorUtility.DrawAngularRangeField("Angular Limit z", bone.AngularLimitZ);
            EditorGUILayout.Space();
            bone . InitialVector = EditorGUILayout.Vector3Field("Initial Vector", bone.InitialVector);
            EditorGUILayout.Space();
            if(GUILayout.Button("Init Children Bones"))
            {
                InitSubBones(bone);
            }
            EditorUtility.DrawFoldList("SubBones", true, bone.SubBones.Length, (i) =>
               {
                   EditorGUILayout.ObjectField(bone.SubBones[i], typeof(Bone), true);
               });
            SceneView.RepaintAll();
        }

        public static void UpdateSubBones(Bone bone)
        {
            for(var i = 0; i < bone.SubBones.Length; i++)
            {
                if (!bone.SubBones[i])
                {
                    bone.SubBones = bone.SubBones.Where(b => b).ToArray();
                    return;
                }
            }
        }

        public static Bone InitSubBones(Bone bone)
        {
            bone.SubBones = new Bone[0];
            for (var i = 0; i < bone.transform.childCount; i++)
            {
                var child = bone.transform.GetChild(i).gameObject;
                if (!child.GetComponent<Bone>())
                {
                    child.AddComponent<Bone>();
                }
                bone.AddSubBone(InitSubBones(child.GetComponent<Bone>()));
            }
            return bone;
        }
    }
}
