using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    class HitCurrentTarget
    {
        enum NotifyState
        {
            eWidth,
            eYello,
            eRed,
            eFail,
        }

        NotifyState currs;

        void EnterState(NotifyState ns)
        {
            if (ns != currs)
            {
                OnLeave(currs);
                InitState(ns);
            }
        }
        void    InitState(NotifyState ns)
        {
            currs = ns;
            starttime = Time.time;
            OnEnter(currs);
        }

        public HitTarget obj
        {
            get;
            private set;
        }
        float starttime;
        public HitCurrentTarget(HitTarget tar)
        {
            isFail = false;
            obj = tar;
            InitState(NotifyState.eWidth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true:delete current obj</returns>
        public bool Update()
        {
            float refValue = (Time.time - starttime) / 10;
            switch (currs)
            {
            case NotifyState.eWidth:
            {
                obj.SetHintColor(Color.white);
                if (refValue > 1)
                {
                    EnterState(NotifyState.eYello);
                }
            }
            break;
            case NotifyState.eYello:
            {
                float fv = (1 + refValue) / 2;
                Color clr = new Color(fv, fv, 0);
                obj.SetHintColor(clr);
                if (refValue >= 1)
                {
                    EnterState(NotifyState.eRed);
                }
            }
            break;
            case NotifyState.eRed:
            {
                float fv = (1 + refValue) / 2;
                Color clr = new Color(fv, 0, 0);
                obj.SetHintColor(clr);
                if (refValue >= 1)
                {
                    EnterState(NotifyState.eFail);
                }
            }
            break;
            case NotifyState.eFail:
            {
                isFail = true;
                return true;
            }
            }
            return false;
        }
        private void    OnEnter(NotifyState ns)
        {
            switch(ns)
            {
            case NotifyState.eWidth:
            obj.SetHint(true);
            break;
            }
        }
        private void OnLeave(NotifyState ns)
        {
            switch(ns)
            {
            case NotifyState.eFail:
            obj.SetHint(false);
            break;
            }
        }
        public void Clear()
        {
            obj.SetHint(false);
        }

        public bool isFail
        {
            get;
            private set;
        }
    }

    public class HitSceneManager : MonoBehaviour
    {

        public static HitSceneManager main;
        void Awake()
        {
            main = this;
        }

        public bool AmICurrent(HitTarget h)
        {
            if (current == null)
            {
                return false;
            }
            return (current.obj == h);
        }

        private HitTarget[] targets;
        HitCurrentTarget current = null;
        // Use this for initialization
        void Start()
        {
            targets = GameObject.FindObjectsOfType<HitTarget>();
            _isGameOver = false;
        }

        private bool _isGameOver;

        void SetGameOver(bool bWin)
        {
            _isGameOver = true;
            if (_isGameOver)
            {
                if (bWin)
                {
                    Jifeng.SoftVrEngine.CustomCanvas.main.ShowFindSuccess();
                }
                else
                {
                    Jifeng.SoftVrEngine.CustomCanvas.main.ShowFindFail();
                }
            }
        }
        bool isGameOver
        {
            get
            {
                return _isGameOver;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isGameOver)
            {
                return;
            }
            int idlingcount = 0;
            if (current == null)
            {
                for(int i = 0;i < targets.Length;++i)
                {
                    if(targets[i].IsIdling)
                    {
                        ++idlingcount;
                    }
                }
                int v = Random.Range(0, targets.Length);
                for (int i = 0; i < targets.Length; ++i)
                {
                    int t = (v + i) % targets.Length;
                    if (targets[t].IsIdling)
                    {
                        targets[t].SetWaiting();
                        current =
                            new HitCurrentTarget(targets[t]);
                        break;
                    }
                }
            }
            if (current != null)
            {
                if (idlingcount > 0 && targets.Length - idlingcount
                            >= SoftVrEngine.GameEnv.Instance.scoreTarget)
                {
                    SetGameOver(true);
                }
                else
                {
                    if (current.Update())
                    {
                        if (current.isFail)
                        {
                            SetGameOver(false);
                        }
                        current = null;
                    }
                    if (current != null && current.obj.IsPlaying)
                    {
                        current.Clear();
                        current = null;
                    }
                }
            }
            else
            {
                SetGameOver(true);
            }
        }
    }
}