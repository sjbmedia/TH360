using UnityEngine;
using System.Collections;

namespace Jifeng.SoftVrEngine
{
    public class JfEye : MonoBehaviour
    {
        public enum EyeAt
        {
            Left,
            Center,
            Right,
        }

        [Tooltip("which one")]
        public EyeAt Which;
    }
    
}
