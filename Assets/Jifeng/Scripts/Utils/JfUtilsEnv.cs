using UnityEngine;
using System.Collections;

namespace Jifeng.Utils
{

    // utils are standalone module,the inputs can be set by unique interface
    public class JfUtilsEnv : MonoBehaviour
    {
        #region static Instance
        private static JfUtilsEnv _ins;
        [HideInInspector]
        public bool started = false;
        public static JfUtilsEnv Instance
        {
            get { return _ins; }
        }

        public  static  Camera  Current
        {
            get
            {
                if (_ins == null) return null;
                return _ins.EventCamrea;
            }
        }
        #endregion

        void    Awake()
        {
            started = false;
            _ins = this;
        }

        void    Start()
        {
            started = true;
        }

        // don't query camera use this object,use Current instead.
        [Tooltip("check ray start from,you may like to set it by scripting")]
        public Camera EventCamrea;
    }
    
}
