using UnityEngine;
using System.Collections;

namespace Jifeng.SoftVrEngine
{

    public class JfVrHead : MonoBehaviour
    {
        private static JfVrHead _head;

        // only one,muse one,in the scene,use "SVECamera" prefabs please.
        [HideInInspector]
        public  static  JfVrHead  Instance
        {
            get { return _head; }
        }

        // change values at runtime may have no effect.
        [Tooltip("whether to split the screen")]
        public bool VRModeEnabled = true;
        public bool trackRotation = true;
        public bool trackPosition = true;
        public Transform target;
        public bool updateEarly;
        private Transform mytrans;
        private JfSimulateInput inputSimulate = new JfSimulateInput();
        private Camera _mainCamera;
        void Awake()
        {
            _head = this;
            mytrans = transform;
        }
        void OnEnable()
        {
            var eyes = this.GetComponentsInChildren<JfEye>();
            UpdateEyes(eyes);
            JfEye.EyeAt maineye = JfEye.EyeAt.Center;
            if(VRModeEnabled)
            {
                maineye = JfEye.EyeAt.Left; // left/right both ok.
            }
            foreach(var e in eyes)
            {
                if(e.Which == maineye)
                {
                    _mainCamera = e.GetComponent<Camera>();
                }
            }
        }
        void Start()
        {
            if (Jifeng.Utils.JfUtilsEnv.Instance != null)
            {
                Jifeng.Utils.JfUtilsEnv.Instance.EventCamrea = _mainCamera;
            }
            var go = new GameObject();
            go.name = "uireference";
            go.transform.parent = this.transform;
            go.transform.localPosition = new Vector3(0, 0, 1);
            if(GameEnv.Instance != null)
            {
                GameEnv.Instance.UiRefer = go;
            }
        }
        private void UpdateEyes(JfEye[] eyes)
        {
            foreach(var e in eyes)
            {
                if(e.Which == JfEye.EyeAt.Center)
                {
                    e.gameObject.SetActive(!VRModeEnabled);
                }
                else
                {
                    e.gameObject.SetActive(VRModeEnabled);
                }
            }
        }

        public  Camera  DefCamre
        {
            get
            {
                return _mainCamera;
            }
        }

        private bool headUpdatedThisFrame;
        void Update()
        {
            headUpdatedThisFrame = false;
            if(updateEarly)
            {
                UpdateHead();
            }
        }

        void LateUpdate()
        {
            UpdateHead();
        }

        void    UpdateHead()
        {
            if(headUpdatedThisFrame)
            {
                return;
            }
            headUpdatedThisFrame = true;
            inputSimulate.UpdateState();
            if(trackRotation)
            {
                var r = inputSimulate.headPose.Orientation;
                if(target == null)
                {
                    transform.localRotation = r;
                }
                else
                {
                    mytrans.transform.rotation = target.rotation * r;
                }
            }
            if(trackPosition)
            {
                Vector3 p = inputSimulate.headPose.Position;
                if(target == null)
                {
                    transform.localPosition = p;
                }
                else
                {
                    transform.position = target.position + target.rotation * p;
                }
            }
        }
    }
    
}
