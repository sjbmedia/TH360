using UnityEngine;
using System.Collections;

namespace Jifeng.Utils
{
    // very simple for processing ray inputs.
    public class JfRaycaster3D : MonoBehaviour {

        public bool RaycasterEnabled;
        private JfSelectable _current;

        private JfSelectable current
        {
            get { return _current; }
            set
            {
                if(_current != value)
                {
                    if(_current != null)
                    {
                        _current.SetCapture(false);
                    }
                    _current = value;
                    if(_current != null)
                    {
                        _current.SetCapture(true);
                    }
                }
            }
        }

	    // Use this for initialization
	    void Start () {
	    }

        JfSelectable c;
        Vector3? hitc;
	    // Update is called once per frame
        void Update()
        {
            if(!JfUtilsEnv.Current)
            {
                return;
            }
            c = null;
            hitc = null;
            if (RaycasterEnabled)
            {
                var trans = JfUtilsEnv.Current.transform;
                var p = trans.TransformVector(new Vector3(0,0,1));
                Ray r = new Ray(trans.position, p);
                RaycastHit[] rh = Physics.RaycastAll(r);
                if (rh != null)
                {
                    foreach (RaycastHit h in rh)
                    {
                        c = h.collider.gameObject.GetComponent<JfSelectable>();
                        if (c != null)
                        {  
                            hitc = h.point;
                            break;
                        }
                    }
                }
            }
            current = c;
            if(current != null)
            {
                if (hitc != null)
                {
                    current.SetHitAt(current.gameObject.transform
                                        .InverseTransformPoint(hitc.Value));
                }
            }
        }
    }
}
