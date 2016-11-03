using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jifeng.DemoSoftVr
{
    /*
     * ReadMe,simple explain of FindDemo(DiceWorld now is FindDemo's new name).
     * Find Demo
     * room is made of 4 walls ceiling floor
     *      FLeveView script is a manager at the level of room.
     *      FLevelData is the data present class
     * wall is made of many brush points.
     *      FWallView script is a manager at the level of wall.
     *      FPointView script describes the brush points
     *      
     * child -- rot
     * FPointView
     *      --  FWallView
     *          --  FLevelView
     */


    // used in load level profile session.
    //  after loaded,the intermediate data will be discarded.
    namespace Data
    {
        enum DataPointState
        {
            eNone,
            eProfiled,
            eGenerated, // generate at runtime,random,according local cap
            eRemoved,   // removed at runtime
        }
        class DataPoint
        {
            public PointDistort distort;
            public PointEasyOrHard easy;
            public DataPointState state;
        }
        class DataWall
        {
            public int Capacity;        // the capacity from profile
            public int Count;           // the count by counting point
            public DataPoint[,] schema = new
                    DataPoint[DemoMenuItems.TILE_WIDTH, DemoMenuItems.TILE_HEIGHT];
        }
        class DataLevel
        {
            public int Count;   // how many do we need
            // the data from profile
            public DataWall[] walls = new DataWall[DemoMenuItems.TILE_FACE];
            public List<IndexData> indexes = new List<IndexData>();
            public GameObject source;
        }
        class IndexData
        {
            public int walli;
            public int xi;
            public int yi;
        }
    }
    // a sample script for creating targets
    public class CreateDiceWorld : MonoBehaviour
    {
        // a simple & sample target object.if you want merge you own resources
        //      this script is the place.
        [Tooltip("the dice prefabs")]
        public GameObject DicePrefab;
        [Tooltip("textures for coloring dice")]
        public Texture[] DiceTextures;
        void    Awake()
        {
            currentData = null;
            if(!DicePrefab)
            {
                Debug.LogError("need selectable target prefab");
            }
            StartCoroutine(RealCreate());
        }

        private Data.DataLevel currentData;
        // you may need wait some thing.
        IEnumerator RealCreate()
        {
            yield return new WaitForEndOfFrame();
            while(Jifeng.Utils.JfUtilsEnv.Instance == null
                || SoftVrEngine.GameEnv.Instance == null)
            {
                yield return new WaitForSeconds(0.01f);
            }
            while(!Utils.JfUtilsEnv.Instance.started 
                || !SoftVrEngine.GameEnv.Instance.started)
            {
                yield return new WaitForSeconds(0.01f);
            }
            LogicCreate();
        }
        private void LogicCreate()
        {
            GameObject profile = null;
            foreach (var lfd in SoftVrEngine.GameEnv.Instance.LevelDatas)
            {
                if (lfd.levelid == SoftVrEngine.GameEnv.CurrentLevel)
                {
                    profile = lfd.profile;
                    break;
                }
            }

            if (!profile)
            {
                Debug.LogError("cann't find level profile data");
            }
            var go = Instantiate(profile) as GameObject;
            currentData = new Data.DataLevel();
            currentData.source = go;
            for (int i = 0; i < currentData.walls.Length; ++i)
            {
                currentData.walls[i] = new Data.DataWall();
            }
            LoadLevelFromProfile();
            RandomWallPoints();
            TrimWallPoints();
            CreateIndexData();
            InstanceLevelData();
            currentData.source = null;
            GameObject.Destroy(go);
        }
        // add points according local cap.do it after load.
        private void RandomWallPoints()
        {
            // first random if need
            foreach (var v in currentData.walls)
            {
                var TILE_COUNT =
                        DemoMenuItems.TILE_WIDTH * DemoMenuItems.TILE_HEIGHT;
                var cap = v.Capacity;
                if (cap > TILE_COUNT)
                {
                    cap = TILE_COUNT;
                }
                if (v.Count >= cap)
                {
                    continue;   // donot need generate
                }
                Jifeng.Utils.JfRandomPick rp = new Utils.JfRandomPick();
                rp.BeginRange();
                for (int x = 0; x < DemoMenuItems.TILE_WIDTH; ++x)
                {
                    for (int y = 0; y < DemoMenuItems.TILE_HEIGHT; ++y)
                    {
                        if (v.schema[x, y].state == Data.DataPointState.eNone)
                        {
                            rp.AddCandinate(x + y * DemoMenuItems.TILE_HEIGHT);
                        }
                    }
                }
                rp.EndRange();
                var dif = cap - v.Count;
                for (int i = 0; i < dif; ++i)
                {
                    var rc = rp.Range();
                    var x = rc % DemoMenuItems.TILE_HEIGHT;
                    var y = rc / DemoMenuItems.TILE_WIDTH;
                    v.schema[x, y].state = Data.DataPointState.eGenerated;
                    v.schema[x, y].easy = PointEasyOrHard.eFix;
                    v.schema[x, y].distort = PointDistort.eRandom;
                }
            }
        }
        // trim points according global cap.
        private void TrimWallPoints()
        {
            int count = 0;
            foreach(var wall in currentData.walls)
            {
                count += wall.Count;
            }
            if(count < currentData.Count)
            {
                return;
            }
            if(currentData.Count < 3)
            {
                return;
            }
            int dif = count - currentData.Count;
            var jr = new Jifeng.Utils.JfRandomRange(0, count - 1);
            int cursor = 0;
            foreach(var wall in currentData.walls)
            {
                foreach(var p in wall.schema)
                {
                    if(p.state == Data.DataPointState.eProfiled 
                        || p.state == Data.DataPointState.eGenerated)
                    {
                        if(jr.inRange(cursor,dif))
                        {
                            p.state = Data.DataPointState.eRemoved;
                        }
                        ++cursor;
                    }
                }
            }
        }
        private void CreateIndexData()
        {
            currentData.indexes.Clear();
            for (int w = 0; w < currentData.walls.Length; ++w )
            {
                for(int x = 0;x < DemoMenuItems.TILE_WIDTH; ++x)
                {
                    for(int y = 0;y < DemoMenuItems.TILE_HEIGHT; ++y)
                    {
                        var d = currentData.walls[w].schema[x, y];
                        if(d.state == Data.DataPointState.eGenerated
                            || d.state == Data.DataPointState.eProfiled)
                        {
                            var nd = new Data.IndexData();
                            nd.walli = w;
                            nd.xi = x;
                            nd.yi = y;
                            currentData.indexes.Add(nd);
                        }
                    }
                }
            }
        }
        private Color   RandomColor()
        {
            Color c = new Color();
            c.a = 1;
            c.r = Random.Range(0, 1.0f);
            c.g = Random.Range(0.0f, 1);
            c.b = Random.Range(0.0f, 1);
            return c;
        }
        private void InstanceLevelData()
        {
            var maxpoints = DiceTextures.Length;
            var jr = new Utils.JfRandomRange(0, maxpoints - 1);
            Texture[] txs = new Texture[maxpoints];
            for (int i = 0; i < maxpoints; ++i)
            {
                txs[i] = DiceTextures[jr.Range()];
            }
            
            // ok,start 
            int count = currentData.indexes.Count;
            var rg = new Utils.JfRandomRange(0, count - 1);
            // while(true)  
            for (int i = 0; i < count / 2 + 1; ++i )
            {
                if(!rg.hasNext())
                {
                    break;
                }
                if(rg.hasNext() && !rg.hasNext2()) // yes,the last one
                {
                    var g = CreateElement(currentData.indexes[rg.Range()]
                                         ,txs[maxpoints - 1],RandomColor());
                    g.GetComponent<CustomTarget>().customtag = "ok";
                }
                else if(rg.hasNext2() && !rg.hasNext3()) // the last paire
                {
                    var g = CreateElement(currentData.indexes[rg.Range()]
                                        , txs[maxpoints - 1],RandomColor());
                    g.GetComponent<CustomTarget>().customtag = "ok";
                }
                else
                {
                    var c = RandomColor();
                    var tx = txs[Random.Range(0, maxpoints - 2)];
                    CreateElement(currentData.indexes[rg.Range()],tx,c);
                    CreateElement(currentData.indexes[rg.Range()],tx,c);
                }
            }
        }
        private void LoadLevelFromProfile()
        {
            var profile = currentData.source;
            var levelView = profile.GetComponent<FLevelView>();
            if(!levelView)
            {
                Debug.LogError("not a valid level profile data");
            }
            currentData.Count = levelView.Count; // if 0,we will sync it latter
            FWallView[] ws = profile.GetComponentsInChildren<FWallView>();
            foreach (FWallView f in ws)
            {
                LoadWallFromProfile(f);
            }
        }

        private void LoadWallFromProfile(FWallView wall)
        {
            var fn = wall.gameObject.name;
            int idx = DemoMenuItems.FaceName2Index(fn);
            currentData.walls[idx].Capacity = wall.capacity;
            // we will fill count latter
            currentData.walls[idx].Count = 0;
            for(int x = 0;x < DemoMenuItems.TILE_WIDTH; ++x)
            {
                for(int y = 0;y < DemoMenuItems.TILE_HEIGHT; ++y)
                {
                    currentData.walls[idx].schema[x, y] = new Data.DataPoint();
                    if(LoadPointFromProfile(wall,idx,x,y))
                    {
                        ++currentData.walls[idx].Count;
                    }
                }
            }
        }

        private bool LoadPointFromProfile(FWallView wall,int idx, int x, int y)
        {
            var fn = DemoMenuItems.PointName(x, y);
            var pt = wall.transform.FindChild(fn);
            var schema = currentData.walls[idx].schema[x, y];
            if(!pt)
            {
                schema.state = Data.DataPointState.eNone;
                return false;
            }
            var pi = pt.gameObject.GetComponent<FPointView>();
            schema.state = Data.DataPointState.eProfiled;
            schema.distort = pi.ViewTrans;
            schema.easy = pi.ViewParts;
            return true;
        }
        private GameObject CreateElement(Data.IndexData lh,Texture t,Color c)
        {
            GameObject go = Instantiate(DicePrefab) as GameObject;
            go.transform.parent = this.transform;
            go.name = lh.walli + "." + lh.xi + "." + lh.yi;
            var tg = go.GetComponent<CustomTarget>();
            tg.DeepColor = c;
            tg.DiceTexture = t;
            var point = currentData.walls[lh.walli].schema[lh.xi, lh.yi];
            Vector3 pos = DemoMenuItems.PointPosition(lh.walli, lh.xi, lh.yi);
            go.transform.localPosition = pos;
            switch (point.easy)
            {
                case PointEasyOrHard.eFix:
                    break;
                case PointEasyOrHard.eShowLess:
                    {
                        var poff = DemoMenuItems.PositionAwayCenter(lh.walli);
                        poff *= Random.Range(0.01f, 0.30f);
                        go.transform.localPosition += poff;
                    }
                    break;
                case PointEasyOrHard.eShowMore:
                    {
                        var poff = DemoMenuItems.PositionAwayCenter(lh.walli);
                        poff *= Random.Range(0.01f, 0.30f);
                        go.transform.localPosition -= poff;
                    }
                    break;
            }
            switch(point.distort)
            {
                case PointDistort.eFaceCamera:
                    {
                        go.transform.LookAt(Vector3.zero);
                    }
                    break;
                case PointDistort.eRandom:
                    {
                        Quaternion qt = Quaternion.Euler(Random.Range(0,360)
                                    ,Random.Range(0,360),Random.Range(0,360));
                        go.transform.localRotation = qt;
                    }
                    break;
                case PointDistort.eFix:
                    break;
            }
            return go;
        }
    }
    
}
