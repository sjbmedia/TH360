using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Jifeng.DemoSoftVr
{
    // when generate target object,process the rotate data
    public enum PointDistort
    {
        eFix,
        eFaceCamera,
        eRandom,
    }
    // show more means you always can see the major part of the object
    // show less might means you can just guess the object(you might not see
    // the object),that makes the game hard (you pass level depends the luck)
    public enum PointEasyOrHard
    {
        eFix,
        eShowMore,
        eShowLess,
    }

    public  enum SchemaLoadFrom
	{
	    eAllRandom,
        eAllProfile,
        eSomeProfile,
	}
    public class FWallData
    {
        // use memory for speed.for quick compare.
        private int lastChildCount = 0;

        public bool IsVersionMatch(int currentCount)
        {
            var v = (lastChildCount == currentCount);
            lastChildCount = currentCount;
            return v;
        }

        private const int Width = 5;
        private const int Height = 5;
        public bool[,] points =
                                new bool[Width, Height];
    }
    public enum ModeOptionType
    {
        eEasy,
        eNeedLuck,
        eDependLuck,
    }

    public class FLevelView :MonoBehaviour
    {
        [Tooltip("simple/normal/hard")]
        public ModeOptionType GameMode;

        // if less than profile,random choose some in profile
        // if more than profile,use profile points.
        // if less than 3,use all profile points.
        [Tooltip("how many points our level need,0 = from profile")]
        public int Count;
    }
    
}
