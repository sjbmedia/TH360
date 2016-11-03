using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Jifeng.DemoSoftVr
{
    public class DemoMenuItems
    {
        public  static  int TILE_WIDTH = 5;
        public static   int TILE_HEIGHT = 5;
        public static int TILE_FACE = 6;    // left/front/top/etc
        public static int  FaceName2Index(string fn)
        {
            switch(fn)
            {
                case "Left":
                    return 0;
                case "Right":
                    return 1;
                case "Front":
                    return 2;
                case "Back":
                    return 3;
                case "Floor":
                    return 4;
                case    "Ceiling":
                    return 5;
                default:
                    throw new System.ArgumentOutOfRangeException(
                                                    "wall name invalid");
            }
        }
        public  static string  FaceIndex2Name(int idx)
        {
            switch(idx)
            {
                case 0:
                    return "Left";
                case 1:
                    return "Right";
                case 2:
                    return "Front";
                case 3:
                    return "Back";
                case 4:
                    return "Floor";
                case 5:
                    return "Ceiling";
                default:
                    throw new System.ArgumentOutOfRangeException(
                                                    "wall index invalid");
            }
        }
        public static Vector3   PointPosition(int idx,int x,int y)
        {
            Vector3 p = new Vector3();
            switch(idx)
            {
                case    0:  // left
                    {
                        p.x = -5;
                        p.z = (x - TILE_WIDTH / 2) * 2;
                        p.y = (y - TILE_HEIGHT / 2) * 2;
                    }
                    break;
                case    1:  //right
                    {
                        p.x = 5;
                        p.z = -(x - TILE_WIDTH / 2) * 2;
                        p.y = (y - TILE_HEIGHT / 2) * 2;
                    }
                    break;
                case    2:  // front
                    {
                        p.z = -5;
                        p.x = (x - TILE_WIDTH / 2) * 2;
                        p.y = (y - TILE_HEIGHT / 2) * 2;
                    }
                    break;
                case    3:  // back
                    {
                        p.z = 5;
                        p.x = -(x - TILE_WIDTH / 2) * 2;
                        p.y = (y - TILE_HEIGHT / 2) * 2;
                    }
                    break;
                case    4:  // floor
                    {
                        p.y = -5;
                        p.x = (x - TILE_WIDTH / 2) * 2;
                        p.z = (y - TILE_HEIGHT / 2) * 2;
                    }
                    break;
                case    5:  // ceiling
                    {
                        p.y = 5;
                        p.x = -(x - TILE_WIDTH / 2) * 2;
                        p.z = -(y - TILE_HEIGHT / 2) * 2;
                    }
                    break;
            }
            return p;
        }

        public static Vector3 PositionAwayCenter(int idx)
        {
            Vector3 p = new Vector3();
            switch(idx)
            {
                case    0: // left
                    p.x = -1;
                    break;
                case    1: // right
                    p.x = 1;
                    break;
                case    2:  // front
                    p.z = -1;
                    break;
                case    3:  // back
                    p.z = 1;
                    break;
                case    4:  // floor
                    p.y = -1;
                    break;
                case    5:  // ceiling
                    p.y = 1;
                    break;
            }
            return p;
        }

        [MenuItem("Window/SoftVrEngine/New FindLevel",false,9101)]
        private static void NewFindLevel()
        {
            var levels = GameObject.FindObjectsOfType<FLevelView>();
            int nameidx = 1;
            if(levels != null)
            {
                nameidx = levels.Length + 1;
            }
            string namestart = "LevelPrefabs_" + nameidx;
            var root = new GameObject(namestart);
            var mats = root.AddComponent<ColorMaterialGallery>();
            mats.LoadColorMaterials();
            root.AddComponent<FLevelView>();
            CreateSideObject(0, new Vector3(0, 10, 0), root);
            CreateSideObject(1, new Vector3(0, 10, 5), root);
            CreateSideObject(2, new Vector3(0, 10, 10), root);
            CreateSideObject(3, new Vector3(0, 10, 15), root);
            CreateSideObject(4, new Vector3(0, 10, 20), root);
            CreateSideObject(5, new Vector3(0, 10, 25), root);
            Undo.RegisterCreatedObjectUndo(root, "NewFindLevel");
        }
        [MenuItem("Window/SoftVrEngine/New SpaceLevel",false,9102)]
        private static  void    NewSpaceLevel()
        {
            var levels = GameObject
                    .FindObjectsOfType<Jifeng.DemoSoftVr.SpaceLevelView>();
            int nameidx = 1;
            if(levels != null)
            {
                nameidx = levels.Length + 1;
            }
            string namestart = "SpaceLevel_" + nameidx;
            var root = new GameObject(namestart);
            root.AddComponent<Jifeng.DemoSoftVr.SpaceLevelView>();

            Undo.RegisterCreatedObjectUndo(root, "NewSpaceLevel");
        }

        static GameObject CreateSideObject(int n,Vector3 p,GameObject r)
        {
            var fn = FaceIndex2Name(n);
            var obj = new GameObject(fn);
            obj.transform.parent = r.transform;
            obj.transform.localPosition = p;
            obj.AddComponent<FWallView>();
            return obj;
        }

        // used by FindWallEditor
        public static string PointName(int x,int y)
        {
            return x + "x" + y;
        }
        // used by FindWallEditor
        public static GameObject CreatePointObject(int x,int y,GameObject p)
        {
            var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var mats = p.GetComponentInParent<ColorMaterialGallery>();
            var mt = mats.GetSchemaMaterial();
            var pv = obj.AddComponent<FPointView>();
            pv.ColorTag = mt;
            obj.GetComponent<Renderer>().material = mats.GetSchemaMaterial(mt);

            obj.name = PointName(x,y);
            if(p)
            {
                obj.transform.parent = p.transform;
                x = (x - TILE_WIDTH / 2) * 2;
                y = TILE_HEIGHT - y;
                y = (y - TILE_HEIGHT / 2) * 2;
                obj.transform.localPosition = new Vector3(x, y, 0);
            }
            return obj;
        }
    }
}