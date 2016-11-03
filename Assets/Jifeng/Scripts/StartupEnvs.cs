using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    // a simple script for auto set env datas.
    public class StartupEnvs : MonoBehaviour
    {
        void    Start()
        {
            StartCoroutine(DelaySetEnvData());
        }

        IEnumerator DelaySetEnvData()
        {
            yield return new WaitForSeconds(1.0f);
            int antiflood = 10;
            while(Utils.JfUtilsEnv.Instance == null 
                || SoftVrEngine.GameEnv.Instance == null)
            {
                yield return new WaitForSeconds(0.3f);
                if(--antiflood < 0)
                {
                    break;
                }
            }
            if(Utils.JfUtilsEnv.Instance != null 
                && SoftVrEngine.GameEnv.Instance != null)
            {
                yield return new WaitForSeconds(1.0f);
                if(!Utils.JfUtilsEnv.Instance.EventCamrea)
                {
                    var ps = GameObject.FindObjectsOfType<Camera>();
                    Camera p = null;
                    foreach(var t in ps)
                    {
                        if(t.gameObject.transform.localPosition.x > 0)
                        {
                            p = t;
                            break;
                        }
                    }
                    Utils.JfUtilsEnv.Instance.EventCamrea = p;
                }
                if (!SoftVrEngine.GameEnv.Instance.UiRefer)
                {
                    // we do a big guess,the parent of main camera is vr head.
                    var root = Utils.JfUtilsEnv.Instance
                                            .EventCamrea.transform.parent;
                    var go = new GameObject();
                    go.name = "uireference";
                    go.transform.parent = root;
                    go.transform.localPosition = new Vector3(0, 0, 1);
                    SoftVrEngine.GameEnv.Instance.UiRefer = go;
                }
            }
            GameObject.Destroy(this);
        }
    }
    
}
