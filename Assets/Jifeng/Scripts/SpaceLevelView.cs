using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    public class SpaceLevelView : MonoBehaviour
    {

        [Tooltip("how many live fire can pass level,0 means all(child count)")]
        public int LeveTarget;
        [Tooltip("simple/normal/hard")]
        public ModeOptionType GameMode;
        public bool enableReferenceX { get; set; }
        public bool enableReferenceY { get; set; }
        public bool enableReferenceZ { get; set; }
        private int _refradius = 5;
        public int refradius
        { get { return _refradius; } set { _refradius = value; } }
        
        void    OnDrawGizmos()
        {
            if (enableReferenceZ && refradius > 1)  
            {
                Matrix4x4 mat = Gizmos.matrix;
                Color defaultColor = Gizmos.color;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.color = new Color(0, 1, 0.5f, 0.3f);
                Vector3 vp = new Vector3(0, 0, refradius);
                Gizmos.DrawCube(vp
                    , new Vector3(refradius * 2, refradius * 2, 0.005f));
                Gizmos.color = new Color(0, 0.5f, 1, 0.3f);
                vp.z = -refradius;
                Gizmos.DrawCube(vp
                    , new Vector3(refradius * 2, refradius * 2, 0.005f));
                Gizmos.color = defaultColor;
                Gizmos.matrix = mat;
            }
            if(enableReferenceY && refradius > 1)
            {
                Matrix4x4 mat = Gizmos.matrix;
                Color defaultColor = Gizmos.color;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.color = new Color(0.5f, 1, 0f, 0.3f);
                Vector3 vp = new Vector3(0, refradius, 0);
                Gizmos.DrawCube(vp
                    , new Vector3(refradius * 2, 0.005f, refradius * 2));
                Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
                vp.y = -refradius;
                Gizmos.DrawCube(vp
                    , new Vector3(refradius * 2, 0.005f, refradius * 2));
                Gizmos.color = defaultColor;
                Gizmos.matrix = mat;
            }
            if (enableReferenceX && refradius > 1)
            {
                Matrix4x4 mat = Gizmos.matrix;
                Color defaultColor = Gizmos.color;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.color = new Color(0.5f, 0, 1.0f, 0.3f);
                Vector3 vp = new Vector3(refradius, 0, 0);
                Gizmos.DrawCube(vp
                    , new Vector3(0.005f, refradius * 2, refradius * 2));
                Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
                vp.x = -refradius;
                Gizmos.DrawCube(vp
                    , new Vector3(0.005f, refradius * 2, refradius * 2));
                Gizmos.color = defaultColor;
                Gizmos.matrix = mat;
            }
        }
    }
    
}
