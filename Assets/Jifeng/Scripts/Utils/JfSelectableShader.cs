using UnityEngine;
using System.Collections;

namespace Jifeng.Utils
{
    /*
     * ReadMe,explain the mechanism.
     *  1.color the object with deep color.
     *  -.if inner mask enabled
     *  [2].    color the mask area with back color     
     *  -.if not enabled
     *      do nothing
     *  -.if out mask enabled
     *  [3].    color the mask area with surf color.
     *  -.if not enabled
     *  [4]    color the mask area with top color
     *  
     * Note,
     *  1. generally,do not dynamic change deep color.
     *      if you think you have to,use back color instead.
     *  2. do not dynamic change the surface color.
     *      if you think you have to,use top color instead.
     */
    public interface JfSelectableShader
    {
        /// <summary>
        /// the color near center of the object
        /// </summary>
        /// <param name="clr"></param>
        void SetDeepColor(Color clr);
        /// <summary>
        /// the color near deepColor,a little far from the object center
        /// </summary>
        /// <param name="clr"></param>
        void SetBackColor(Color clr);
        /// <summary>
        /// the color near backColor,more far from the object center.
        /// </summary>
        /// <param name="clr"></param>
        void SetSurfColor(Color clr);
        /// <summary>
        /// if at the same layer with surf color
        /// </summary>
        /// <param name="clr"></param>
        void SetTopColor(Color clr);

        void SetInMaskTex(Texture tex);
        void SetOutMaskTex(Texture tex);

        void SetInMaskEnable(bool bv);

        void SetOutMaskEnable(bool bv);
    }
    
}
