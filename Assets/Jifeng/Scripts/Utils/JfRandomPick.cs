using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Jifeng.Utils
{
    public class JfRandomPick
    {
        private List<int> candinate;
        int cursor;
        public  void    BeginRange()
        {
            candinate = new List<int>();
        }
        public void EndRange()
        {
            cursor = 0;
            for(int i = 0;i < candinate.Count - 1; ++i)
            {
                int v = Random.Range(i + 1, candinate.Count - 1);
                var t = candinate[v];
                candinate[i] = candinate[v];
                candinate[v] = t;
            }
        }
        public  void    AddCandinate(int v)
        {
            candinate.Add(v);
        }

        public  int Range()
        {
            return candinate[cursor++];
        }
        public  bool    hasNext()
        {
            return cursor < candinate.Count;
        }
    }
}
