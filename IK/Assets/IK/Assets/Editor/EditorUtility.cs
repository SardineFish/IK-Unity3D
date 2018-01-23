using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Assets.Editor
{
    public static class EditorUtility
    {
        public static AngularRange DrawAngularRangeField(string lable, AngularRange value)
        {
            var rect = EditorGUILayout.BeginHorizontal(GUILayout.Height(22));
            rect.height = 20;
            EditorGUILayout.Space();
            var values = new float[] { value.low, value.high };
            EditorGUI.MultiFloatField(rect,new GUIContent(lable), new GUIContent[] { new GUIContent(" "), new GUIContent("~") }, values);
            EditorGUILayout.EndHorizontal();

            return new AngularRange(values[0], values[1]);
        }

        public static bool DrawFoldList(string lable, bool show, Func<bool> itemRenderingCallback)
        {
            show = EditorGUILayout.Foldout(show, lable);
            if (show)
            {
                GUIStyle style = new GUIStyle();
                style.margin.left = 40;
                EditorGUILayout.BeginVertical(style);
                while (itemRenderingCallback()) ;
                EditorGUILayout.EndVertical();
            }
            return show;
        }

        public static bool DrawFoldList(string lable, bool show,int count, Action<int> itemRenderingCallback)
        {
            show = EditorGUILayout.Foldout(show, lable);
            if (show)
            {
                GUIStyle style = new GUIStyle();
                style.margin.left = 40;
                EditorGUILayout.BeginVertical(style);
                for (var i = 0; i < count; i++)
                {
                    itemRenderingCallback(i);
                }
                EditorGUILayout.EndVertical();
            }
            return show;
        }
    }
}
