using UnityEngine;
using System.Collections;

namespace Jifeng.SoftVrEngine
{
    // ui prefabs holder,show ui prefabs helper.
    public class CustomCanvas : MonoBehaviour
    {

        [Tooltip("the success menu prefabs")]
        public GameObject successobj;
        [Tooltip("the fail menu prefabs")]
        public GameObject failobj;

        private GameObject topmenu;

        void Awake()
        {
            main = this;
        }

        void Start()
        {
            foreach (Transform item in transform)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        // seems I prefer singleton
        public static CustomCanvas main = null;

        private void ShowPrefabsMenu(GameObject menuprefabs)
        {

            GameObject go = GameEnv.Instance.UiRefer;
            if(!go)
            {
                Debug.LogError(
                    "need ui refer,default SVECamera-prefab has done,try use");
            }

            if (topmenu == null)
            {
                topmenu = GameObject.Instantiate(menuprefabs) as GameObject;
            }
            topmenu.SetActive(true);
            topmenu.transform.parent = transform;

            topmenu.transform.rotation = go.transform.rotation;
            topmenu.transform.position = go.transform.position;
        }

        public void ShowFindSuccess()
        {
            ShowPrefabsMenu(successobj);
        }

        public void ShowFindFail()
        {
            ShowPrefabsMenu(failobj);
        }
    }
}

