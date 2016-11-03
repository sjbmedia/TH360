using UnityEngine;
using UnityEditor;

namespace Jifeng.DemoSoftVr
{
    // the editor do only one work that show point data.
    // data changes will first apply to the child object then update the grid.
    [CustomEditor(typeof(FWallView))]
    [ExecuteInEditMode]
    public class FWallViewEditor : Editor
    {
        enum EditMode
        {
            Point,
            Viewable,
            Rotation,
            Check,
        }

        EditMode currentMode = EditMode.Point;
        FWallView findwall;

        Color   GetFarColor(Color c)
        {
            Color dclr = Color.gray;
            if(Mathf.Abs(c.r - 0.5f) < 0.2f && Mathf.Abs(c.g - 0.5f) < 0.2f 
                    && Mathf.Abs(c.b - 0.5f) < 0.2f)
            {
                dclr.r = 1.0f;
                dclr.g = 0.7f;
                dclr.b = 0.7f;
                dclr.a = 0.7f;
            }
            return dclr;
        }

        public override void OnInspectorGUI()
        {
            findwall = target as FWallView;
            // draw use less text
            var op = GUILayout.Width(300);
            GUILayout.Box("" ,op);
            var cont = "Capacity = max(Capacity,Children Count)";
            var nl = "\nIf count < Capacity, Random rest(cap - cnt).";
            GUILayout.TextArea(cont + nl
                            , Utils.JfGUIStyle.LABEL_TEXT_STYLE, op);
            GUILayout.Space(10);
            // draw count field
            int capv = EditorGUILayout.IntField("Capacity", findwall.capacity
                                , Utils.JfGUIStyle.LABEL_TEXT_STYLE
                                ,GUILayout.ExpandWidth(true));
            if(capv != findwall.capacity)
            {
                Undo.RecordObject(findwall, "Level Inspector");
                findwall.capacity = capv;
                Undo.FlushUndoRecordObjects();
            }
            GUILayout.Box("", op);

            // draw point grid & op buttons.
            System.Func<float,GUILayoutOption> w = GUILayout.Width;
            var wauto = GUILayout.ExpandWidth(true);
            var tbtn = EditorStyles.toolbarButton;
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar ,wauto);
            var dclr = GUI.color;
            var hiclr = GetFarColor(dclr);
            GUI.color = currentMode == EditMode.Point ? hiclr : dclr;
            if(GUILayout.Button("points" ,tbtn,w(100)))
            {
                currentMode = EditMode.Point;
            }
            GUI.color = currentMode == EditMode.Viewable ? hiclr : dclr;
            if(GUILayout.Button("offset",tbtn,w(100)))
            {
                currentMode = EditMode.Viewable;
            }
            GUI.color = currentMode == EditMode.Rotation ? hiclr : dclr;
            if(GUILayout.Button("rotate",tbtn,w(100)))
            {
                currentMode = EditMode.Rotation;
            }
            GUI.color = currentMode == EditMode.Check ? hiclr : dclr;
            if(GUILayout.Button("check",tbtn,w(100)))
            {
                currentMode = EditMode.Check;
            }
            GUI.color = dclr;
            EditorGUILayout.EndHorizontal();
            DrawPoints();
            EditorGUILayout.Space();
            if(GUILayout.Button("Reset" ,tbtn ,w(200)))
            {
                ClearPoints();
            }

            // batch options - trans
            GUILayout.Box("", op);
            GUILayout.Label("set children trans option ALL to:");
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("allFixed"))
            {
                BatchApplyOption(ApplyFaceFixed);
            }
            if(GUILayout.Button("allFaceCamera"))
            {
                BatchApplyOption(ApplyFaceCamera);
            }
            if(GUILayout.Button("allRandom"))
            {
                BatchApplyOption(ApplyFaceRandom);
            }
            GUILayout.EndHorizontal();
            // batch option - rotation
            GUILayout.Box("", op);
            GUILayout.Label("set children's view option ALL to");
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("ShowMore"))
            {
                BatchApplyOption(ApplyShowMore);
            }
            if(GUILayout.Button("ShowLess"))
            {
                BatchApplyOption(ApplyShowLess);
            }
            if(GUILayout.Button("KeepDefault"))
            {
                BatchApplyOption(ApplyShowFixed);
            }
            GUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }

        #region draw  points status
        Rect guiCurrentRect;
        static int rectSize = 50;
        #endregion
        #region draw points style
        #endregion

        void ClearPoints()
        {
            if (findwall.transform.childCount > 0)
            {
                foreach(Transform t in findwall.transform)
                {
                    GameObject.DestroyImmediate(t.gameObject);
                }
                Undo.RecordObject(findwall, "reset points");
                Undo.FlushUndoRecordObjects();
            }
        }

        // the draw point function only show whether a point esists.
        void DrawPoints()
        {
            // check the level struct
            //      see FindDemo ReadMe for struct description.
            //      it is at CreateDiceWorld script file
            var info = "level data struct invalid,try create new";
            if(!findwall.transform.parent)
            {
                Debug.LogError(info);
            }
            var level = findwall.transform.parent.GetComponent<FLevelView>();
            if(level == null)
            {
                Debug.LogError(info);
            }
            // draw the data.
            GUILayout.Space(10);
            guiCurrentRect = GUILayoutUtility.GetRect(
                                rectSize * 5 ,
                                rectSize * 5);
            if(!findwall.walldata.IsVersionMatch(findwall.transform.childCount))
            {
                for (int x = 0; x < 5; ++x )
                {
                    for(int y = 0;y < 5; ++y)
                    {
                        // point object view
                        RetrievePointData(x, y);
                    }
                }
            }
            // draw the schema,not the buttons.
            DrawPointGrid();

            for (int x = 0; x < 5; ++x)
            {
                for (int y = 0; y < 5; ++y)
                {
                    DrawPoint(x, y);
                }
            }
        }
        // retrieve data from transform.
        void RetrievePointData(int x, int y)
        {
            string pointname = DemoMenuItems.PointName(x, y);

            if(findwall.transform.FindChild(pointname))
            {
                findwall.walldata.points[x, y] = true;
            }
            else
            {
                findwall.walldata.points[x, y] = false;
            }
        }
        // just the positions flag,not the clickable buttons
        void DrawPointGrid()
        {
            // draw border
            var recttotal = rectSize * 5;
            EditorGUI.DrawRect(
                new Rect(guiCurrentRect.xMin 
                        ,guiCurrentRect.yMin
                        ,1
                        ,recttotal)
                    ,Color.yellow);
            EditorGUI.DrawRect(
                new Rect(guiCurrentRect.xMin + recttotal-1
                        ,guiCurrentRect.yMin
                        ,1
                        ,recttotal)
                    ,Color.yellow);
            EditorGUI.DrawRect(
                new Rect(guiCurrentRect.xMin ,guiCurrentRect.yMin
                        ,recttotal ,1)
                        ,Color.yellow);
            EditorGUI.DrawRect(
                new Rect(guiCurrentRect.xMin 
                        ,guiCurrentRect.yMin + recttotal - 1
                        ,recttotal ,1)
                    ,Color.yellow);
            
            // draw inner lines
            for(int x = 1; x < 5; ++x)
            {
                var left = guiCurrentRect.xMin + 
                                rectSize * x;
                EditorGUI.DrawRect(
                    new Rect(left,guiCurrentRect.yMin,1,recttotal)
                    ,Color.yellow);
            }
            // draw inner lines
            for(int y = 1; y < 5; ++y)
            {
                var top = guiCurrentRect.yMin + 
                                rectSize * y;
                EditorGUI.DrawRect(
                    new Rect(guiCurrentRect.xMin ,top ,recttotal ,1)
                    ,Color.yellow
                    );
            }
        }
        // draw the point,besides,show/hide the rotation object
        void    DrawPoint(int x,int y)
        {
            var pname = DemoMenuItems.PointName(x, y);
            // inspector
            DrawPointInspector(x ,y ,pname);
        }

        void    DrawPointInspector(int x,int y,string pointname)
        {
            float left = rectSize * x;
            float top = rectSize * y;
            Color guicolor = GUI.color;            
            string info = "";

            if (findwall.walldata.points[x, y])
            {
                info = "V";
            }
            else
            {
                var c = GUI.color;
                c.a = 0;
                GUI.color = c;                
            }
            
            if(GUI.Button(new Rect(guiCurrentRect.xMin + left + 1
                                ,guiCurrentRect.yMin + top + 1
                                ,rectSize - 2,rectSize - 2)
                            ,info))
            {
                var pname = DemoMenuItems.PointName(x, y);
                var t = findwall.transform.FindChild(pname);
                if(t)
                {
                    Undo.DestroyObjectImmediate(t.gameObject);
                }
                else
                {
                    // process add new point
                    var g = DemoMenuItems.CreatePointObject(x, y
                                                    , findwall.gameObject);
                    Undo.RegisterCreatedObjectUndo(g, "new point");
                }
            }
            GUI.color = guicolor;
        }

        #region BatchApplyOption
        
        // avoid lambda as possible.
        private delegate bool ApplyOption(FPointView fp, bool isTestOnly);
        private bool ApplyShowMore(FPointView fp,bool isTestOnly)
        {
            return SetViewOption(fp, PointEasyOrHard.eShowMore, isTestOnly);
        }
        private bool ApplyShowLess(FPointView fp,bool isTestOnly)
        {
            return SetViewOption(fp, PointEasyOrHard.eShowLess, isTestOnly);
        }
        private bool    ApplyShowFixed(FPointView fp,bool isTestOnly)
        {
            return SetViewOption(fp, PointEasyOrHard.eFix, isTestOnly);
        }
        private bool ApplyFaceFixed(FPointView fp,bool isTestOnly)
        {
            return SetViewOption(fp, PointDistort.eFix, isTestOnly);
        }
        private bool ApplyFaceCamera(FPointView fp,bool tOnly)
        {
            return SetViewOption(fp, PointDistort.eFaceCamera, tOnly);
        }
        private bool ApplyFaceRandom(FPointView fp,bool tOnly)
        {
            return SetViewOption(fp, PointDistort.eRandom, tOnly);
        }
        void    BatchApplyOption(ApplyOption ao)
        {
            var childrens = findwall.GetComponentsInChildren<FPointView>();
            bool bneedChange = false;
            foreach (var v in childrens)
            {
                if (ao(v, true))
                {
                    bneedChange = true;
                    break;
                }
            }
            if (bneedChange)
            {
                Undo.RecordObjects(childrens, "ApplyChildren");
                foreach (var v in childrens)
                {
                    ao(v, false);
                }
                Undo.FlushUndoRecordObjects();
            }
        }
        bool    SetViewOption(FPointView fp,PointEasyOrHard bpp,bool bTest)
        {
            if(fp.ViewParts == bpp)
            {
                return false;
            }
            if(!bTest)
            {
                fp.ViewParts = bpp;
            }
            return true;
        }
        bool SetViewOption(FPointView fp, PointDistort bpr, bool bTest)
        {
            if(fp.ViewTrans == bpr)
            {
                return false;
            }
            if(!bTest)
            {
                fp.ViewTrans = bpr;
            }
            return true;
        }
        #endregion
    }
}
