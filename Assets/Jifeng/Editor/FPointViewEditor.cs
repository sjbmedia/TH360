using UnityEngine;
using UnityEditor;

namespace Jifeng.DemoSoftVr
{
    [CustomEditor(typeof(FPointView))]
    [ExecuteInEditMode]
    public class FPointViewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            FPointView holder = (FPointView)target;
            DrawDefaultInspector();
            GUILayout.Box("",GUILayout.ExpandWidth(true));

            GUILayout.Label("Color the point if you like"
                                    ,Utils.JfGUIStyle.LABEL_TEXT_STYLE
                                    ,GUILayout.ExpandWidth(true));
            GUILayout.Label(
                "color the position holder only,has no use in game logic"
                , Utils.JfGUIStyle.LABEL_TEXT_STYLE
                , GUILayout.ExpandWidth(true));
            var ct =  
                    (ColorMaterialGallery.EditTagType)EditorGUILayout
                                .EnumPopup("ColorTag"
                                    ,holder.ColorTag
                                    ,GUILayout.ExpandWidth(true));
            if (holder.ColorTag != ct)
            {
                var rend = holder.GetComponent<Renderer>();
                Object[] gos = new Object[]{
                    holder,
                    rend,
                };
                Undo.RecordObjects(gos, "Level Inspector");
                holder.ColorTag = ct;
                var mg = holder.GetComponentInParent<ColorMaterialGallery>();
                rend.material = mg.GetSchemaMaterial(ct);
                Undo.FlushUndoRecordObjects();
            }

            GUILayout.Box("" ,GUILayout.ExpandWidth(true));
            serializedObject.ApplyModifiedProperties();
        }

        void    OnEnable()
        {
            ToolsSupport.ForceRotation();
        }
        void OnDisable()
        {
            ToolsSupport.ReleaseForce();
        }
    }
}
