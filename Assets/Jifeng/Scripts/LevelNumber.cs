using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    // a sample script for show number (max 2 digits)
    public class LevelNumber : MonoBehaviour
    {
        [Tooltip("left digit")]
        public SpriteRenderer Left;
        [Tooltip("right digit")]
        public SpriteRenderer Right;
        [Tooltip("number 0-9,should be set by order,0-0,9-9,+-+,from 0 to 11")]
        public Sprite[] Digits;

        public static string  level_key = "current_level";

        public  static int GetCurrentLevel()
        {
            return PlayerPrefs.GetInt(level_key, 1);
        }

        void Start()
        {
            UpdateLevelNumber(GetCurrentLevel());
        }

        private void    UpdateLevelNumber(int v)
        {
            if (v > 99)
            {
                Left.sprite = Digits[9];
                Right.sprite = Digits[10];
            }
            else if (v > 9)
            {
                Left.sprite = Digits[v / 10];
                Right.sprite = Digits[v % 10];
            }
            else
            {
                Left.sprite = null;
                Right.sprite = Digits[v];
            }
        }

        public  void    NextLevel()
        {
            var v = PlayerPrefs.GetInt(level_key, 1);
            PlayerPrefs.SetInt(level_key, ++v);
            UpdateLevelNumber(v);
            SoftVrEngine.GameEnv.CurrentLevel = v;
        }
        public  void    LastLevel()
        {
            var v = PlayerPrefs.GetInt(level_key, 1);
            if(v > 1)
            {
                PlayerPrefs.SetInt(level_key, --v);
            }
            UpdateLevelNumber(v);
            SoftVrEngine.GameEnv.CurrentLevel = v;
        }
    }
}