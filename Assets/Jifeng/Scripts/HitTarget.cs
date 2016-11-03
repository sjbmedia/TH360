using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    class HitTargetState
    {
        // I am tired of dp,just pass self,go ahead.
        private HitTarget self;
        // cache ColorTheBall script, for speed.
        public  HitTargetState(HitTarget ht)
        {
            self = ht;
        }

        public enum State
        {
            eIdle,      // cann't focus,do not play any effect
            eWait,      // wait for mouse event
            eFocusing,  // pre play
            ePlaying,   // play
            eStop,      // stop
        }

        private float FOCUS_LEN = 1.5f;
        private float PLAY_LEN = 30.0f;

        State current;
        float stateStartTime;
        void SetState(State s)
        {
            if (current != s)
            {
                LeaveState(current);
                current = s;
                stateStartTime = Time.time;
                EnterState(current);
            }
        }
        void    EnterState(State s)
        {
            switch (current)
            {
            case State.eFocusing:
            {
            }
            break;
            case State.ePlaying:
            {
                self.ParticleControler.Play();
                self.SetHint(false);
            }
            break;
            case State.eStop:
            {
                self.ParticleControler.Stop();
                self.ParticleControler.Clear();
            }
            break;
            }

        }

        void LeaveState(State s)
        {
            switch (s)
            {
            case State.eFocusing:
            {
            }
            break;
            }
        }

        public void Update()
        {
            switch (current)
            {
            case State.eWait:
            break;
            case State.eFocusing:
            {
                var now = Time.time;
                if (now > stateStartTime + FOCUS_LEN)
                {
                    SetState(State.ePlaying);
                    break;
                }
                // sorry for place a break keyword inst
                var dif = now - stateStartTime;
                dif = dif / FOCUS_LEN * 3;
                Color outcolor = new Color(dif, dif - 1, dif - 2);
                if (outcolor.r > 1)
                {
                    outcolor.r = 1;
                }
                if (outcolor.r < 0)
                {
                    outcolor.r = 0;
                }
                if (outcolor.g > 1)
                {
                    outcolor.g = 1;
                }
                if (outcolor.g < 0)
                {
                    outcolor.g = 0;
                }
                if (outcolor.b > 1)
                {
                    outcolor.b = 1;
                }
                if (outcolor.b < 0)
                {
                    outcolor.b = 0;
                }
                self.SetCheckColor(outcolor);
            }
            break;
            case State.ePlaying:
            {
                if (Time.time > stateStartTime + PLAY_LEN)
                {
                    SetState(State.eStop);
                }
            }
            break;
            case State.eStop:
            {
                if (Time.time > stateStartTime + 0.1f)
                {
                    SetState(State.eIdle);
                }
            }
            break;
            }
        }

        public  void    OnMouseEnter()
        {
            if(current == State.eWait)
            {
                SetState(State.eFocusing);
            }
            self.SetCheck(true);
        }

        public  void    OnMouseLeave()
        {
            self.SetCheck(false);
            if(current == State.eFocusing)
            {
                SetState(State.eWait);
            }
        }

        public  void    OnNeedWait()
        {
            if(current == State.eIdle)
            {
                SetState(State.eWait);
            }
        }

        public  bool    IsIdling()
        {
            if(current == State.eIdle)
            {
                return true;
            }
            return false;
        }

        public  bool    IsPlaying()
        {
            return (current == State.ePlaying || current == State.eStop);
        }
    }

    [RequireComponent(typeof(Jifeng.Utils.JfSelectable))]
    [RequireComponent(typeof(Jifeng.Utils.ColorTheSelectable))]
    public class HitTarget : MonoBehaviour,Utils.JfSelectableTarget
    {
        [Tooltip("the texture used for selecting")]
        public Texture SelectingTex;
        [Tooltip("the texture used for skin")]
        public Texture BallTex;
        [Tooltip("top color")]
        public Color TopColor;
        [Tooltip("surface color")]
        public Color SurfaceColor;
        [Tooltip("default deep color")]
        public Color DeepColor;
        [Tooltip("default back color")]
        public Color BackColor;
        Utils.ColorTheSelectable shader;
        HitTargetState htstate;
        void Awake()
        {
            htstate = new HitTargetState(this);
        }
        // Use this for initialization
        void Start()
        {
            var ps = gameObject.GetComponent<Utils.JfSelectable>();
            ps.listener = this;
            shader = gameObject.GetComponent<Utils.ColorTheSelectable>();
            shader.SetInMaskTex(SelectingTex);
            shader.SetOutMaskTex(BallTex);
            shader.SetDeepColor(DeepColor);
            shader.SetBackColor(BackColor);
            shader.SetSurfColor(SurfaceColor);
            shader.SetTopColor(TopColor);
        }

        private ParticleSystem _particlesys;
        public ParticleSystem ParticleControler
        {
            get
            {
                if(_particlesys == null)
                {
                    _particlesys = GetComponentInChildren<ParticleSystem>();
                    if(!_particlesys)
                    {
                        Debug.LogError(
                            "no particle,are you using other prefabs?");
                    }
                }
                return _particlesys;
            }
        }

        public  void    SetHint(bool bv)
        {
            shader.SetOutMaskEnable(bv);
        }

        public  void    SetCheck(bool bv)
        {
            shader.SetInMaskEnable(bv);
        }

        public void SetHintColor(Color clr)
        {
            shader.SetTopColor(clr);
        }
        public void SetCheckColor(Color clr)
        {
            shader.SetBackColor(clr);
        }

        public void OnButtonEnter()
        {
            htstate.OnMouseEnter();
        }

        public void OnButtonLeave()
        {
            htstate.OnMouseLeave();
        }

        public void SetWaiting()
        {
            htstate.OnNeedWait();
        }

        public bool IsIdling
        {
            get
            {
                return htstate.IsIdling();
            }
        }

        public bool IsPlaying
        {
            get
            {
                return htstate.IsPlaying();
            }
        }


        public void OnButtonSlider(Vector3 p)
        {
           
        }

        public void OnButtonUpdate()
        {
            htstate.Update();
        }
    }
}