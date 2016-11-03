using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    // a simple script for set mesh texture,self-removed after set texture.
    public class DiceWall : MonoBehaviour
    {
        [Tooltip("the wall Tex")]
        public Texture WallTex;

        // Use this for initialization
        void Start()
        {
            GetComponent<Renderer>().material.mainTexture = WallTex;
            GameObject.Destroy(this);
        }

    }

}
