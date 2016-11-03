using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Jifeng.SoftVrEngine
{
    [CustomEditor(typeof(GameEnv))]
    [ExecuteInEditMode]
    public class GameEnvEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var self = target as GameEnv;
            GUILayout.Box("", GUILayout.ExpandWidth(true));
            GUILayout.Label("NOTE:drag has no use,set by scripting"
                                , Utils.JfGUIStyle.LABEL_TEXT_STYLE);
            EditorGUILayout.ObjectField("UiRefer", self.UiRefer
                                            , typeof(GameObject), true, null);
            GUILayout.Box("", GUILayout.ExpandWidth(true));
            GameEnv gv = target as GameEnv;
            int v = 0;
            if(gv.LevelDatas != null)
            {
                v = gv.LevelDatas.Length;
            }
            int ev = EditorGUILayout.IntField("Level Count", v
                                        , GUILayout.ExpandWidth(true));
            // update new data
            if(ev != v)
            {
                Undo.RecordObject(gv, "Change Length");
                var old = gv.LevelDatas;
                gv.LevelDatas = new LevelProfileData[ev];
                if(old != null)
                {
                    for(int i = 0;i < old.Length; ++i)
                    {
                        if(i >= ev) // may decrease count
                        {
                            break;
                        }
                        gv.LevelDatas[i] = old[i];
                    }
                }
                Undo.FlushUndoRecordObjects();
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("LevelID", GUILayout.Width(120));
            EditorGUILayout.LabelField("Level Profile Data"
                                        , GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
            if(gv.LevelDatas != null)
            {
                for(int i = 0;i < gv.LevelDatas.Length; ++i)
                {
                    DrawElement(ref gv.LevelDatas[i]);
                }
            }
        }

        private void    DrawElement(ref LevelProfileData lpd)
        {
            EditorGUILayout.BeginHorizontal();
            int v = EditorGUILayout.IntField(lpd.levelid, GUILayout.Width(120));
            var o = EditorGUILayout.ObjectField(lpd.profile, typeof(GameObject)
                            ,false, GUILayout.ExpandWidth(true));
            if(v != lpd.levelid || o != lpd.profile)
            {
                Undo.RecordObjects(new Object[] { target }, "Level Profile");
                lpd.levelid = v;
                lpd.profile = o as GameObject;
                Undo.FlushUndoRecordObjects();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
    
}
