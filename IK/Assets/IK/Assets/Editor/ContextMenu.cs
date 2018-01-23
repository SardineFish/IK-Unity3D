using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class ContextMenu
    {
        [MenuItem("GameObject/3D Object/Bone")]
        static void CreateBone(MenuCommand cmd)
        {
            var bone = BoneEditor.CreateBone(cmd.context as GameObject);
            Selection.SetActiveObjectWithContext(bone, bone.gameObject);
        }

        [MenuItem("GameObject/3D Object/FK")]
        static void CreateFK(MenuCommand cmd)
        {
            if((cmd.context as GameObject).GetComponent<Bone>())
            {
                var fk = FKEditor.CreateFK((cmd.context as GameObject).GetComponent<Bone>());
                Selection.SetActiveObjectWithContext(fk, fk.gameObject);
            }
        }

        [MenuItem("GameObject/3D Object/IK CCD")]
        static void CreateIKCCD(MenuCommand cmd)
        {
            var bone = (cmd.context as GameObject).GetComponent<Bone>();
            if (bone)
            {
                var ik = IKCCDEditor.CreateIKCCD(bone);
                Selection.SetActiveObjectWithContext(ik, ik.gameObject);
            }
        }
    }
}
