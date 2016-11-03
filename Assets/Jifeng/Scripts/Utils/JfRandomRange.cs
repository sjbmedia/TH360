using UnityEngine;
using System.Collections;

namespace Jifeng.Utils
{
    public class JfRandomRange
    {
        private int[] candinate;
        int cursor;
        public JfRandomRange(int min,int max)
        {
            Init(min, max);
        }
        // [min,max],both include
        public  void    Init(int min,int max)
        {
            cursor = 0;
            candinate = new int[max - min + 1];
            for(int i = min; i <= max; ++i)
            {
                candinate[i - min] = i;
            }
            for(int i = 0;i < candinate.Length - 1; ++i)
            {
                int v = Random.Range(i + 1, candinate.Length - 1);
                var t = candinate[i];
                candinate[i] = candinate[v];
                candinate[v] = t;
            }
        }
        public  int Range()
        {
            return candinate[cursor++];
        }
        // does the next one exist
        public  bool    hasNext()
        {
            return cursor < candinate.Length;
        }
        // does the next 2 exist
        public bool hasNext2()
        {
            return cursor < candinate.Length - 1;
        }
        // does the next 3 exist
        public bool hasNext3()
        {
            return cursor < candinate.Length - 2;
        }
        // if value in the [0,cap) range
        public  bool    inRange(int v,int cap)
        {
            for(int i = 0;i < cap; ++i)
            {
                if(i >= candinate.Length)
                {
                    break;
                }
                if(candinate[i] == v)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
