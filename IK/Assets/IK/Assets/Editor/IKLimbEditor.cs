using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace Assets.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(IKLimb))]
    public class IKLimbEditor:IKCCDEditor
    {
    }
}
