using UnityEngine;
using System.Collections;

namespace Jifeng.Utils
{
    public interface JfSelectableTarget
    {
        /// <summary>
        /// focus enters this selectable area
        /// </summary>
        void OnButtonEnter();
        /// <summary>
        /// focus leaves this selectable area
        /// </summary>
        void OnButtonLeave();
        /// <summary>
        /// slide action happens on this selectable area
        /// </summary>
        /// <param name="p">position based selectable local space</param>
        void OnButtonSlider(Vector3 p);
        /// <summary>
        /// this selectable update event
        /// </summary>
        void OnButtonUpdate();
    }

}
