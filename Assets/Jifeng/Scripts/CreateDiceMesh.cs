using UnityEngine;
using System.Collections;

namespace Jifeng.SoftVrEngine
{
    // thanks:
    // <a href>http://answers.unity3d.com/questions/542787/change-texture-of-cube-sides.html</a>
    // take care we use different texture uv.
    public class CreateDiceMesh : MonoBehaviour
    {
        private static Vector2[] _uvs;
        private static Vector2[] UVS
        {
            get
            {
                if(_uvs == null)
                {
                    _uvs = new Vector2[24];
                    // 4-3,5-2,1-6
                    // front 4
                    _uvs[0] = Vector2.zero;
                    _uvs[1] = Vector2.right / 3;
                    _uvs[2] = Vector2.up / 2;
                    _uvs[3] = Vector2.right / 3 + Vector2.up / 2;
                    // top 1
                    _uvs[8] = Vector2.right / 3 * 2;
                    _uvs[9] = Vector2.right;
                    _uvs[4] = Vector2.right / 3 * 2 + Vector2.up / 2;
                    _uvs[5] = Vector2.right + Vector2.up / 2;
                    // back 3
                    _uvs[10] = Vector2.right / 3 * 2 + Vector2.up / 2;
                    _uvs[11] = Vector2.right + Vector2.up / 2;
                    _uvs[6] = Vector2.right / 3 * 2 + Vector2.up;
                    _uvs[7] = Vector2.right + Vector2.up;
                    // bottom 6
                    _uvs[12] = Vector2.up / 2;
                    _uvs[14] = Vector2.right / 3 + Vector2.up / 2;
                    _uvs[15] = Vector2.up;
                    _uvs[13] = Vector2.right / 3 + Vector2.up;
                    // left 5
                    _uvs[16] = Vector2.right / 3 + Vector2.up / 2;
                    _uvs[18] = Vector2.right / 3 * 2 + Vector2.up / 2;
                    _uvs[19] = Vector2.right / 3 + Vector2.up;
                    _uvs[17] = Vector2.right / 3 * 2 + Vector2.up;
                    // right 2
                    _uvs[20] = Vector2.right / 3;
                    _uvs[22] = Vector2.right / 3 * 2;
                    _uvs[23] = Vector2.right / 3 + Vector2.up / 2;
                    _uvs[21] = Vector2.right / 3 * 2 + Vector2.up / 2;
                }
                return _uvs;
            }
        }

        void Start()
        {
            var meshf = GetComponent<MeshFilter>();
            if(meshf == null)
            {
                Debug.LogError("do not remove the MeshFilter please");
            }

            var mesh = meshf.mesh;
            if(mesh == null || mesh.uv.Length != 24)
            {
                Debug.LogError("please use the build in cube");
            }

            mesh.uv = UVS;     
        }
    }
    
}
