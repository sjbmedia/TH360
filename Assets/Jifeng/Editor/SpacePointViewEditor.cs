using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Jifeng.DemoSoftVr
{
    [CustomEditor(typeof(SpacePointView))]
    [ExecuteInEditMode]
    public class SpacePointViewEditor : Editor
    {
        private SpacePointView self;
        private const int WSPACE = 100;
        private GUILayoutOption[] _btnOpt;
        private GUILayoutOption[] btnOpt
        {
            get
            {
                if(_btnOpt == null)
                {
                    _btnOpt = new GUILayoutOption[]
                    {
                        GUILayout.Width(WSPACE),
                        GUILayout.Height(40),
                    };
                }
                return _btnOpt;
            }
        }

        public override void OnInspectorGUI()
        {
            self = target as SpacePointView;
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            if (GUILayout.Button("AlignGrid", btnOpt))
            {
                var p = self.transform.localPosition;
                if(Mathf.Approximately(p.x - (int)p.x,0)
                    && Mathf.Approximately(p.y - (int)p.y,0)
                    && Mathf.Approximately(p.z - (int)p.z,0))
                {
                    // already aligned,bye.
                }
                else
                {
                    Utils.JfUndo.RecordObject(self,self.transform,"AlignGrid");
                    p.x = Mathf.Ceil(p.x - 0.5f);
                    p.y = Mathf.Ceil(p.y - 0.5f);
                    p.z = Mathf.Ceil(p.z - 0.5f);
                    self.transform.localPosition = p;
                    Undo.FlushUndoRecordObjects();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Note,if shift goes wrong direction"
                                , Utils.JfGUIStyle.LABEL_TEXT_STYLE);
            GUILayout.Label("\tUNDO(ctrl+z) is first encouraged"
                                , Utils.JfGUIStyle.LABEL_TEXT_STYLE);
            GUILayout.Label("\t\topposite button may double wrong"
                                , Utils.JfGUIStyle.LABEL_TEXT_STYLE);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space(WSPACE);
            if(GUILayout.Button("ShiftUp",btnOpt))
            {
                Utils.JfUndo.RecordObject(self, self.transform, "ShiftUp");
                var p = self.transform.localPosition;
                p.y += 1;
                self.transform.localPosition = p;
                Undo.FlushUndoRecordObjects();
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("ShiftLeft",btnOpt))
            {
                MoveLeftRight(1, "ShiftLeft");
            }
            GUILayout.Space(WSPACE);
            if(GUILayout.Button("ShiftRight",btnOpt))
            {
                MoveLeftRight(-1, "ShiftRight");
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(WSPACE);
            if(GUILayout.Button("ShiftDown",btnOpt))
            {
                Utils.JfUndo.RecordObject(self, self.transform, "ShiftDown");
                var p = self.transform.localPosition;
                p.y += 1;
                self.transform.localPosition = p;
                Undo.FlushUndoRecordObjects();
            }
            GUILayout.EndHorizontal();
            self = null;
        }

        private void    MoveLeftRight(int v,string key)
        {
            var p = self.transform.localPosition;
            // we choose min(x,z) direction for move plane
            if (Mathf.Abs(p.x - p.z) < 0.5f)
            {
                // to near,sorry I cann't decide which is move plane
            }
            else
            {
                Utils.JfUndo.RecordObject(self, self.transform, key);
                // ok, x if move plane
                if (Mathf.Abs(p.x) < Mathf.Abs(p.z))
                {
                    // if z > 0 ,left is left,else left is right. lol.
                    if (p.z > 0)
                    {
                        p.x += v;
                    }
                    else
                    {
                        p.x -= v;
                    }
                }
                else // z is move plane
                {
                    if (p.x > 0)
                    {
                        p.z += v;
                    }
                    else
                    {
                        p.z -= v;
                    }
                }
                //ok,as you wish.(it may...)
                self.transform.localPosition = p;
                Undo.FlushUndoRecordObjects();
            }
        }
    }
    
}
