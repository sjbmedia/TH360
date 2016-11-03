using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Jifeng.Utils
{
    public class JfDrawLine
    {
        public void Startup(Color clr)
        {
            colorTexture = new Texture2D(1,1);
            colorTexture.SetPixel(0,0,clr);
            colorTexture.wrapMode = TextureWrapMode.Repeat;
            colorTexture.Apply();
        }

        public  void    Cleanup()
        {
            colorTexture = null;
        }
        private Texture2D colorTexture;
        // do call it in OnGUI
        public void Draw(Vector2 s,Vector2 e)
        {
            Draw(s, e, 1);
        }
        // do call it in OnGUI
        public void Draw(Vector2 s,Vector2 e,int thickness)
        {
            if(colorTexture == null)
            {
                return;
            }
            var line = e - s;
            float k1;
            if(line.x == 0)
            {
                if(line.y > 0)
                {
                    k1 = 90;
                }
                else
                {
                    k1 = 270;
                }
            }
            else
            k1 = Mathf.Rad2Deg * Mathf.Atan(line.y / line.x);
            if (line.x < 0)
            {
                k1 += 180;
            }
            var thi = thickness;
            if(thi < 1)
            {
                thi = 1;
            }
            GUIUtility.RotateAroundPivot(k1, s);
            GUI.DrawTexture(new Rect(s.x, s.y - thi / 2, line.magnitude, thi)
                                    , colorTexture);
            GUIUtility.RotateAroundPivot(-k1, s);
        }
    }
}
