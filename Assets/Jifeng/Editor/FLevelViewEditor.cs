using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Jifeng.DemoSoftVr
{
    [CustomEditor(typeof(FLevelView))]
    [ExecuteInEditMode]
    public class FLevelViewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var op = GUILayout.ExpandWidth(true);
            GUILayout.Box("", op);
            var cont = "REMOVE a child means 0 point on the side.";
            GUILayout.Label(cont, Utils.JfGUIStyle.LABEL_TEXT_STYLE, op);
            GUILayout.Box("", op);
        }
    }
}
