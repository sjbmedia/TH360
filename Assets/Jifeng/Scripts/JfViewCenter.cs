using UnityEngine;
using System.Collections;

namespace Jifeng.SoftVrEngine
{
    // draw focusing point ( with two color lines )
    public class JfViewCenter : MonoBehaviour
    {
        [Tooltip("show focusing center,null for hide")]
        public bool showCenterHint = true;
        [Tooltip("the color a,for show focusing point")]
        public Color colora = Color.red;
        [Tooltip("the color b,for show focusing point")]
        public Color colorb = Color.magenta;
        [Tooltip("the focusing point flag radius")]
        public int radius = 10;
        [Tooltip("the focusing cross thickness")]
        public int thickness = 1;
        #region Data_for_Drawing
        private Utils.JfDrawLine lineH;
        private Utils.JfDrawLine lineV;
        private Vector3 Refer = new Vector3(0, 0, 1);
        private Camera[] cameras;
        #endregion
        // Use this for initialization
        void Start()
        {
            lineH = new Utils.JfDrawLine();
            lineH.Startup(colora);
            lineV = new Utils.JfDrawLine();
            lineV.Startup(colorb);
            if (showCenterHint)
            {
                var go = GameObject.Find("SVECamera/Head");
                if(!go)
                {
                    Debug.LogError("NEED SVECamera,use prefabs to add one");
                }
                cameras = go.GetComponentsInChildren<Camera>();
            }
        }

        void OnGUI()
        {
            if (showCenterHint && cameras != null)
            {
                foreach(var c in cameras)
                {
                    var v1 = c.gameObject.transform.TransformPoint(Refer);
                    var v2 = c.WorldToScreenPoint(v1);
                    DrawCrossOn(v2);
                }
            }
        }

        void    DrawCrossOn(Vector2 c)
        {
            lineH.Draw(new Vector2(c.x - radius, c.y)
                                , new Vector2(c.x + radius, c.y), thickness);
            lineV.Draw(new Vector2(c.x, c.y - radius)
                                , new Vector2(c.x, c.y + radius), thickness);
        }
    }
    
}
