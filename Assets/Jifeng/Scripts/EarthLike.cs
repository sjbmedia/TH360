using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    public class EarthLike : MonoBehaviour
    {
        [Tooltip("the earth texture")]
        public Texture MainTexture;

        void    Awake()
        {
            gameObject.GetComponent<Renderer>().material.mainTexture = MainTexture;
        }
        
    }
    
}
