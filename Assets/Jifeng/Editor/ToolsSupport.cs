using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System.Collections;

// https://community.unity.com/t5/Extensions-OnGUI/Hiding-default-transform-handles/td-p/664716
namespace Jifeng.DemoSoftVr
{
    public class ToolsSupport
    {
        private static Tool oldTool;
        public static void ForceRotation()
        {
            oldTool = Tools.current;
            Tools.current = Tool.Rotate;
            //Tools.hidden = true;
        }

        public static void ReleaseForce()
        {
            //Tools.hidden = false;
            Tools.current = oldTool;
        }
    }    
}
