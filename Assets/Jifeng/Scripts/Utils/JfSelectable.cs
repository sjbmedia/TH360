using UnityEngine;
using System.Collections;

namespace Jifeng.Utils
{
    // act likes an interface,user can always call same function
    public class JfSelectable : MonoBehaviour
    {
        // if it exists,I will call directly instead of SendMessage
        // NOTE:listener exists,event will be sent to intf only.
        [HideInInspector]
        [Tooltip("the listener can register self for speeding.")]
        public JfSelectableTarget listener;

        private bool isCapturing = false;

        void Update()
        {
            if(listener != null)
            {
                listener.OnButtonUpdate();
            }
        }
        public bool IsCapturing()
        {
            return isCapturing;
        }
        public void SetCapture(bool bv)
        {
            isCapturing = bv;
            if (bv)
            {
                if (listener == null)
                {
                    SendMessage("OnButtonEnter"
                                , SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    listener.OnButtonEnter();
                }
            }
            else
            {
                if (listener == null)
                {
                    SendMessage("OnButtonLeave"
                                , SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    listener.OnButtonLeave();
                }
            }
        }
        // vector3 p,in local space
        public void SetHitAt(Vector3 p)
        {
            if (listener == null)
            {
                SendMessage("OnButtonSlider"
                            , p, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                listener.OnButtonSlider(p);
            }
        }
    }
    
}
