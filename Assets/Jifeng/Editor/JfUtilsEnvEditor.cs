using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Jifeng.SoftVrEngine
{
   
    [CustomEditor(typeof(Jifeng.Utils.JfUtilsEnv))]
    [ExecuteInEditMode]
    public class JfUtilsEnvEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var self = target as Jifeng.Utils.JfUtilsEnv;
            GUILayout.Box("",GUILayout.ExpandWidth(true));
            GUILayout.Label("NOTE:drag has no use,set by scripting"
                                , Utils.JfGUIStyle.LABEL_TEXT_STYLE);
            EditorGUILayout.ObjectField("EventCamera"
                                , self.EventCamrea,typeof(Camera),true,null);
            GUILayout.Box("", GUILayout.ExpandWidth(true));
        }
    
    }
    
}
