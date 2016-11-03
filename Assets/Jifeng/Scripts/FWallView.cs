using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    // the manager of BrushPoints, check CreateDiceWorld.cs for detail
    //  there is a read-me at the top of the file
    public class FWallView :MonoBehaviour
    {
        [Tooltip("how many points will be used on this wall")]
        public int capacity;

        // cache the wall
        private FWallData _walldata = new FWallData();
        [HideInInspector]
        public FWallData walldata
        {
            get { return _walldata; }
        }
    }

}