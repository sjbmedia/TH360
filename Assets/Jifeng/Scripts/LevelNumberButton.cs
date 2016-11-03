using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    public class LevelNumberButton : MonoBehaviour
    {
        public enum OpType
        {
            Add,
            Dec,
        }
        [Tooltip("the button used for")]
        public OpType Op;
        private LevelNumber number;

        void Start()
        {
            StartCoroutine(DelayCacheNumberObject());
        }
        IEnumerator DelayCacheNumberObject()
        {
            yield return new WaitForSeconds(1.5f);
            number = GameObject.FindObjectOfType<LevelNumber>();
        }
       
        public  void    OnButtonConfirmed()
        {
            switch(Op)
            {
            case OpType.Add:
            if(number)
            {
                number.NextLevel();
            }
            break;
            case OpType.Dec:
            if(number)
            {
                number.LastLevel();
            }
            break;
            }
        }
    }
    
}
