using UnityEngine;
using System.Collections;

namespace Jifeng.Utils
{

    // NOTE,check the shader please,this script is a shader helper.
    public class ColorTheSelectable : MonoBehaviour,JfSelectableShader
    {      

        public void SetDeepColor(Color clr)
        {
            GetComponent<Renderer>().material.SetColor("_DeepColor", clr);
        }

        public void SetBackColor(Color clr)
        {
            GetComponent<Renderer>().material.SetColor("_BackColor", clr);
        }

        public void SetSurfColor(Color clr)
        {
            GetComponent<Renderer>().material.SetColor("_SurfColor", clr);
        }

        public void SetTopColor(Color clr)
        {
            GetComponent<Renderer>().material.SetColor("_TopColor", clr);
        }

        public void SetInMaskTex(Texture tex)
        {
            GetComponent<Renderer>().material.SetTexture("_InnerTex", tex);
        }

        public void SetOutMaskTex(Texture tex)
        {
            GetComponent<Renderer>().material.SetTexture("_OuterTex", tex);
        }

        public void SetInMaskEnable(bool bv)
        {
            GetComponent<Renderer>().material.SetFloat("_InMaskEnable", bv ? 1 : 0);
        }

        public void SetOutMaskEnable(bool bv)
        {
            GetComponent<Renderer>().material.SetFloat("_OutMaskEnable", bv ? 1 : 0);
        }
    }
    
}
