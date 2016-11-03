using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Jifeng.SoftVrEngine
{
    public class JfSoftVrEngineWindow : EditorWindow
    {
        [MenuItem("Window/SoftVrEngine/Switch SDK",false,8001)]
        static void AddWindow()
        {
            Rect wdr = new Rect(0, 0, 500, 500);
            EditorWindow.GetWindowWithRect(
                                typeof(JfSoftVrEngineWindow), wdr, true
                                , "SoftVrEngine SDK Swicher");
        }

        void    OnGUI()
        {
            GUILayout.Space(30);
            GUILayout.Label("to use SDK switcher, see readme please."
                                ,Utils.JfGUIStyle.LABEL_TEXT_STYLE);
        }
    }
    
}
