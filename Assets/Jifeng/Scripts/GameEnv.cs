using UnityEngine;
using System.Collections;

namespace Jifeng.SoftVrEngine
{
    [System.Serializable]
    public  struct LevelProfileData
	{
        // the level id based 1
		public int levelid;
        public GameObject  profile;
	}
    // the game is made of several modules.
    //      we need pass parameter at interface layer 
    public class GameEnv : MonoBehaviour
    {
        [HideInInspector]
        public bool started;
        #region static Instance
        private static GameEnv _ins;
        public static GameEnv   Instance
        {
            get { return _ins; }
        }

        // the playing level.
        [HideInInspector]
        public static int CurrentLevel = 1;
        #endregion

        void    Awake()
        {
            started = false;
            _ins = this;
        }

        void Start()
        {
            started = true;           
        }

        [Tooltip(@"where to show the ui,we need refer an object
,you may like to set it by scription")]
        public GameObject UiRefer;
        [Tooltip("the level data,present by prefabs")]
        public LevelProfileData[] LevelDatas;

        // special for find levels.
        [HideInInspector]
        public string gameTargetName;
        [HideInInspector]   // todo refactor me
        public int scoreTarget;
    }
    
}
