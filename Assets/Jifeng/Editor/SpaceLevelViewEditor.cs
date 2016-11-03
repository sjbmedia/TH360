using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Jifeng.DemoSoftVr
{
    [CustomEditor(typeof(SpaceLevelView))]
    [ExecuteInEditMode]
    public class SpaceLevelViewEditor : Editor
    {
        private SpaceLevelView self;

        public override void OnInspectorGUI()
        {
            self = target as SpaceLevelView;
            DrawDefaultInspector();
            var op = GUILayout.ExpandWidth(true);
            GUILayout.Box("", op);
            GUILayout.Label(
                    "if want remove a point,just remove it form children"
                            , Utils.JfGUIStyle.LABEL_TEXT_STYLE, op);
            GUILayout.Box("", op);

            DrawButtonGrid();

            GUILayout.Box("reference gizmos - begin");
            self.refradius = EditorGUILayout.IntField("ref object radius"
                                                , self.refradius, op);
            self.enableReferenceX = EditorGUILayout.Toggle("show refer x"
                                                , self.enableReferenceX, op);
            self.enableReferenceY = EditorGUILayout.Toggle("show refer y"
                                                , self.enableReferenceY, op);
            self.enableReferenceZ = EditorGUILayout.Toggle("show refer z"
                                                ,self.enableReferenceZ, op);
            GUILayout.Box("reference gizmos - end");

            self = null;    // I don't want to give you dirty data.
        }

        private GUILayoutOption[] _buttonOpt;
        private GUILayoutOption[] buttonOpt
        {
            get
            {
                if(_buttonOpt == null)
                {
                    _buttonOpt = new GUILayoutOption[]
                    {
                        GUILayout.Width(300),
                        GUILayout.Height(80),
                    };
                }
                return _buttonOpt;
            }
        }

        private void    DrawButtonGrid()
        {
           
            if(GUILayout.Button("NewPoint",buttonOpt))
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.AddComponent<SpacePointView>();
                go.transform.parent = ((SpaceLevelView)target).transform;
                int xoff = Random.Range(1, 4);
                float yoff = Random.Range(-5.0f, 5.0f);
                Vector3 vp = new Vector3(5, xoff, yoff);
                switch (xoff)
                {
                    case 1:
                        break;
                    case 2:
                        vp.x *= -1;
                        break;
                    case 3:
                        {
                            var v = vp.x;
                            vp.x = vp.z;
                            vp.z = v;
                        }
                        break;
                    case 4:
                        {
                            var v = vp.x;
                            vp.x = vp.z;
                            vp.z = v;
                            vp.x *= -1;
                        }
                        break;
                }
                go.transform.localPosition = vp;
                go.name = self.transform.childCount.ToString();
                Undo.RegisterCreatedObjectUndo(go, "New Point");
            }
        }
    }
    
}
