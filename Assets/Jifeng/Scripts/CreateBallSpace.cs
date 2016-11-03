using UnityEngine;
using System.Collections;

namespace Jifeng.DemoSoftVr
{
    // a sample script for creating space level.
    public class CreateBallSpace : MonoBehaviour
    {
        [Tooltip("the point prefabs")]
        public GameObject PointPrefab;
        [Tooltip("the textures for coloring point")]
        public Texture[] PointTextures;

        void Awake()
        {
            if(!PointPrefab)
            {
                Debug.LogError("need selectable target prefab");
            }
            StartCoroutine(RealCreate());
        }
        IEnumerator RealCreate()
        {
            yield return new WaitForEndOfFrame();
            while(Jifeng.Utils.JfUtilsEnv.Instance == null
                || Jifeng.SoftVrEngine.GameEnv.Instance == null)
            {
                yield return new WaitForEndOfFrame();
            }
            LogicCreate();
            yield return new WaitForEndOfFrame();
            // add game logic control
            if(gameObject.GetComponent<HitSceneManager>() == null)
            {
                gameObject.AddComponent<HitSceneManager>();
            }
        }

        private void LogicCreate()
        {
            GameObject profile = null;
            foreach(var lfd in Jifeng.SoftVrEngine.GameEnv.Instance.LevelDatas)
            {
                if(lfd.levelid == Jifeng.SoftVrEngine.GameEnv.CurrentLevel)
                {
                    profile = lfd.profile;
                    break;
                }
            }
            if(!profile)
            {
                Debug.LogError("cann't find level profile data");
            }
            var go = Instantiate(profile) as GameObject;
            var space = go.GetComponent<SpaceLevelView>();
            if(space.LeveTarget > 0)
            {
                SoftVrEngine.GameEnv.Instance.scoreTarget = space.LeveTarget;
            }
            else
            {
                SoftVrEngine.GameEnv.Instance.scoreTarget 
                                            = space.transform.childCount;
            }

            CreatePointsFromProfile(space);

            GameObject.Destroy(go);
        }

        private void    CreatePointsFromProfile(SpaceLevelView slv)
        {
            foreach(Transform pt in slv.transform)
            {
                var go = Instantiate(PointPrefab) as GameObject;
                go.transform.parent = this.transform;
                go.transform.localPosition = pt.localPosition;
            }
        }
    }

}
