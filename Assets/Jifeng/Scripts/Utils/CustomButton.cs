using UnityEngine;
using System.Collections;

namespace Jifeng.Utils
{ 
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Jifeng.Utils.JfSelectable))]
    public class CustomButton : MonoBehaviour {

        private bool bHasConfirmedSend;
        private float lastPercent;

        public enum UnlockDirection
        {
            eToRight,
            eToTop,
        }

        public UnlockDirection TargetDirection;
        [Tooltip("the cursor's shape for hinting ")]
        public Sprite hintSprite;
	    void Start () {
            var sr = GetComponent<SpriteRenderer>();
            var mat = sr.GetComponent<Renderer>().material;
            if(hintSprite)
            {
                var baser = sr.sprite.rect;
                var herer = hintSprite.rect;
                var fw = herer.width / baser.width;
                var fh = herer.height / baser.height;
                mat.SetFloat("_CursorWid" ,fw);
                mat.SetFloat("_CursorHei" ,fh);
                mat.SetFloat("_CursorOff" ,Random.Range(0.0f ,1.0f));
                mat.SetTexture("_CursorTex" ,hintSprite.texture);

            }
            else
            {
                mat.SetFloat("_CursorWid" ,0);
                mat.SetFloat("_CursorHei" ,0);  
            }
	    }

        public void OnButtonEnter()
        {
            lastPercent = 0.3f;
            gameObject.GetComponent<SpriteRenderer>()
                                .material.SetFloat("_Percent" ,0);
            GetComponent<Renderer>().material.SetFloat("_Borderp" ,1);
            GetComponent<Renderer>().material.SetFloat("_Percent" ,lastPercent);

        }
        public void OnButtonLeave()
        {
            bHasConfirmedSend = false;
            lastPercent = 0;
            GetComponent<Renderer>().material.SetFloat("_Borderp" ,0);
            GetComponent<Renderer>().material.SetFloat("_Percent" ,lastPercent);
        }
        public void OnButtonSlider(Vector3 p)
        {
            SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            float perc = (p.x + 1) / 2.0f;

            if(perc < 0)
            {
                perc = 0;
            }
            else if(perc > 1)
            {
                perc = 1;
            }

            if(Mathf.Abs(lastPercent - perc) < 0.1f)
            {
                lastPercent = perc;
                sr.material.SetFloat("_Percent" ,perc);

                if(perc > 0.9f && !bHasConfirmedSend)
                {
                    bHasConfirmedSend = true;
                    SendMessage("OnButtonConfirmed"
                                    ,SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}