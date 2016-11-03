using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Jifeng.DemoSoftVr
{

    // process time depend effect of the selected status
    class FocusTargetShader
    {
        public FocusTargetShader(CustomTarget dice, System.DateTime now)
        {
            this.shaderintf = dice;
            this.startfrom = now;
            st = ShaderState.eIdle;
        }

        CustomTarget shaderintf;
        System.DateTime startfrom;
        ShaderState st;

        enum ShaderState
        {
            eIdle,
            eStart,
            eStep1,
            eStep2,
            eStep3,
            eEnd,
        }

        void EnterState(ShaderState s)
        {
            if (st == s)
            {
                return;
            }
            switch (s)
            {
                case ShaderState.eStart:
                    {
                        shaderintf.SetObjectSelected(true);
                        shaderintf.SetObjectColor(Color.black);
                    }
                    break;
                case ShaderState.eStep1:
                    {
                        shaderintf.SetObjectColor(Color.yellow);
                    }
                    break;
                case ShaderState.eStep2:
                    {
                        shaderintf.SetObjectColor(Color.cyan);
                    }
                    break;
                case ShaderState.eStep3:
                    {
                        shaderintf.SetObjectColor(Color.red);
                    }
                    break;
                case ShaderState.eEnd:
                    {
                        shaderintf.OnButtonClicked();
                        shaderintf.SetObjectSelected(false);
                    }
                    break;
            }
            st = s;
        }


        public void Reset()
        {
            shaderintf.SetObjectSelected(false);
            startfrom = System.DateTime.MinValue;
        }

        public void Update()
        {
            double delta = (System.DateTime.Now - startfrom).TotalMilliseconds;
            switch (st)
            {
                case ShaderState.eIdle:
                    {
                        EnterState(ShaderState.eStart);
                        startfrom = System.DateTime.Now;
                    }
                    break;
                case ShaderState.eStart:
                    {
                        if (delta > 500)
                        {
                            EnterState(ShaderState.eStep1);
                        }
                    }
                    break;
                case ShaderState.eStep1:
                    {
                        if (delta > 2000)
                        {
                            EnterState(ShaderState.eStep2);
                        }
                    }
                    break;
                case ShaderState.eStep2:
                    {
                        if (delta > 3500)
                        {
                            EnterState(ShaderState.eStep3);
                        }
                    }
                    break;
                case ShaderState.eStep3:
                    {
                        if (delta > 5000)
                        {
                            EnterState(ShaderState.eEnd);
                        }
                    }
                    break;
                case ShaderState.eEnd:
                    {
                        // you just stay in this state
                    }
                    break;
            }
        }
    }

    // the interface of OnButtonEnter/OnButtonLeave & the shader exchanger
    [RequireComponent(typeof(Jifeng.Utils.JfSelectable))]
    [RequireComponent(typeof(Utils.ColorTheSelectable))]
    public class CustomTarget : MonoBehaviour,Utils.JfSelectableTarget
    {
        [Tooltip("custom session,for query,like usertag")]
        public string customtag;
        [Tooltip("dice border style")]
        public Texture BorderTexture;
        [Tooltip("dice star texture")]
        public Texture DiceTexture;
        [Tooltip("dice star color")]
        public Color StarColor;
        [Tooltip("default deep color")]
        public Color DeepColor;
        [Tooltip("default back color")]
        public Color BackColor;
        FocusTargetShader target;
        Utils.ColorTheSelectable shader;
        // Use this for initialization
        void Start()
        {
            var ps = gameObject.GetComponent<Utils.JfSelectable>();
            ps.listener = this;
            shader = gameObject.GetComponent<Utils.ColorTheSelectable>();
            shader.SetInMaskTex(BorderTexture);
            shader.SetOutMaskTex(DiceTexture);
            shader.SetTopColor(StarColor);
            shader.SetSurfColor(StarColor);
            shader.SetBackColor(BackColor);
            shader.SetDeepColor(DeepColor);
        }

        public void OnButtonEnter()
        {
            target = new FocusTargetShader(this,System.DateTime.Now);
        }

        public void OnButtonLeave()
        {
            if (target != null)
            {
                target.Reset();
            }
            target = null;
        }

        public void OnButtonClicked()
        {
            if (customtag.ToLower() == "ok")
            {
                SoftVrEngine.CustomCanvas.main.ShowFindSuccess();
            }
            else
            {
                SoftVrEngine.CustomCanvas.main.ShowFindFail();
            }
        }
        public  void    SetObjectSelected(bool bv)
        {
            shader.SetInMaskEnable(bv);
        }

        public  void    SetObjectColor(Color clr)
        {
            shader.SetBackColor(clr);
        }


        public void OnButtonSlider(Vector3 p)
        {
            
        }

        public void OnButtonUpdate()
        {
            if (target != null)
            {
                target.Update();
            }
        }
    }    
}

