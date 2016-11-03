using UnityEngine;
using UnityEditor;

namespace Jifeng.Utils
{
    public class JfUndo
    {
        public static void RecordObject(Object p1, Object p2, string name)
        {
            var p = new Object[] { p1, p2 };
            Undo.RecordObjects(p, name);
        }

        public  static  void    RecordObject(Object p1,Object p2,Object p3,
                                                string name)
        {
            var p = new Object[] { p1, p2, p3 };
            Undo.RecordObjects(p, name);
        }
    }
    
}
