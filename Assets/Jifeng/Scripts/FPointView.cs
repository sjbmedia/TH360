using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    public class FPointView : MonoBehaviour
    {
        [HideInInspector]
        [Tooltip("color the position holder only,has no use in game logic")]
        public ColorMaterialGallery.EditTagType ColorTag;
        [Tooltip("the target will rotate for increase game fun")]
        public PointDistort ViewTrans;
        [Tooltip("the target will not show whole body")]
        public PointEasyOrHard ViewParts;
    }
}