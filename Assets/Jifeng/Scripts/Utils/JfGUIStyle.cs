using UnityEngine;
using System.Collections;

namespace Jifeng.Utils
{
    public class JfGUIStyle
    {
        private static GUIStyle _text_style = null;
        public static GUIStyle LABEL_TEXT_STYLE
        {
            get
            {
                if (_text_style == null)
                {
                    _text_style = new GUIStyle(GUI.skin.label);
                    _text_style.normal.textColor
                                    = new Color(0.84f, 0.79f, 0.6f);
                    _text_style.fontStyle = FontStyle.Bold;
                }
                return _text_style;
            }
        }
    }

}
