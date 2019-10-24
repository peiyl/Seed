using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SeedType
{
    Linbum,//0
    StoneTree,//1
    ThornTree,//荆棘树
    GuardianTree,//
    Blumen,//
    Mac,//果树
    Saviour,//
    Diva,//女歌手
    Chomper,//
    Cibum,//香烟
    FristSeed,//第一颗树
    None,
}
public enum StateType
{
    Idle,
    BeAttack,
    Die,
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isStart;
    public GameObject index;
    public GameObject fristTreeItem;
    public GameObject[] buttonGO;

    public string[] treePrefabName;
    private RaycastHit2D hit;
    /// <summary>
    /// 当前选中的种子类型
    /// </summary>
    private SeedType seedType = SeedType.None;
    /// <summary>
    /// 记录种子变异获得
    /// </summary>
    private bool[] seedIsGet;
    private SeedType[] linbumVar = new SeedType[] { SeedType.Linbum, SeedType.StoneTree, SeedType.Blumen, SeedType.Diva };
    private SeedType[] stoneTreeVar = new SeedType[] { SeedType.StoneTree, SeedType.ThornTree, SeedType.GuardianTree };
    private SeedType[] blumenVar = new SeedType[] { SeedType.Blumen, SeedType.Mac, SeedType.Saviour };
    private SeedType[] divaVar = new SeedType[] { SeedType.Diva, SeedType.Chomper, SeedType.Cibum };

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        isStart = false;
        seedIsGet = new bool[10];
        for (int i = 0; i < seedIsGet.Length; i++)
        {
            seedIsGet[i] = false;
        }
        UpdateSeedBtnShow();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("鼠标右键点击，当前持有种子为："+seedType);
            RaycastHit2D hit2 = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform != null)
                Debug.Log("鼠标右键点击，当前射线碰撞到：" + hit2.transform.tag);
        }
        if (Input.GetMouseButtonDown(0) && seedType != SeedType.None)
        {
            if (isStart)
            {
                TreePlanting("Green");
            }
            else
            {
                TreePlanting("Send");
            }
        }
    }
    public void TreePlanting(string tag)
    {
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null && hit.transform.tag == tag)
        {
            Debug.Log(hit.transform.tag);
            if (seedType == SeedType.FristSeed)
            {
                isStart = true;
                fristTreeItem.SetActive(false);
                seedIsGet[0] = true;
                buttonGO[0].SetActive(true);
                buttonGO[0].GetComponent<SeedItemBtn>().StartCD();
            }
            else
            {
                buttonGO[(int)seedType].GetComponent<SeedItemBtn>().StartCD();

                if (IsVariation(ref seedType))
                {
                    //变异音效
                    //变异UI提示
                    //显示对应Btn，开始CD
                    buttonGO[(int)seedType].SetActive(true);
                }
                buttonGO[(int)seedType].GetComponent<SeedItemBtn>().StartCD();
            }
            index.SetActive(false);
            Instantiate(Resources.Load<GameObject>("Prefabs/" + treePrefabName[(int)seedType]),new Vector3(hit.point.x, hit.point.y,-1), Quaternion.identity);
            seedType = SeedType.None;
        }
    }
    public void SetTreeSeed(SeedType seedType, Transform transform)
    {
        if (index.transform.position == transform.position && index.activeInHierarchy)
        {
            //如果同一位置点击两次
            index.SetActive(false);
            this.seedType = SeedType.None;
        }
        else
        {
            index.SetActive(true);
            index.transform.position = transform.position;
            this.seedType = seedType;
        }
    }
    private bool IsVariation(ref SeedType seedType)
    {
        SeedType type = seedType;
        switch (seedType)//如果已经变异过了就直接不变异了。
        {
            case SeedType.Linbum:
                if (!SeedIsGet(1, 4, 7))
                {
                    type = Variation(linbumVar);
                }
                break;
            case SeedType.StoneTree:
                if (!SeedIsGet(2, 3))
                {
                    type = Variation(stoneTreeVar);
                }
                break;
            case SeedType.Blumen:
                if (!SeedIsGet(5, 6))
                {
                    type = Variation(blumenVar);
                }
                break;
            case SeedType.Diva:
                if (!SeedIsGet(8, 9))
                {
                    type = Variation(divaVar);
                }
                break;
        }
        //如果数值相同说明没有变异成功
        if (seedType == type)
        {
            return false;
        }
        else
        {
            seedType = type;
            return true;
        }
    }

    private void UpdateSeedBtnShow()
    {
        for (int i = 0; i < seedIsGet.Length; i++)
        {
            buttonGO[i].SetActive(seedIsGet[i]);
        }
    }
    private bool SeedIsGet(params int[] Num)
    {
        for (int i = 0; i < Num.Length; i++)
        {
            if (!seedIsGet[Num[i]])
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 变异
    /// </summary>
    /// <param name="vars">变异组</param>
    /// <returns>变异结果</returns>
    private SeedType Variation(SeedType[] vars)
    {
        int num = Random.Range(0, vars.Length);
        if (!seedIsGet[(int)(vars[num])])
        {
            return vars[num];
        }
        return vars[0];
    }
    #region 弃用

    /// <summary>
    /// 普通树的变异逻辑
    /// </summary>
    /// <returns></returns>
    private SeedType LinbumVariation()
    {
        int num = Random.Range(0, 4);
        //随机到的植物如果已经存在，就不变异了
        switch (num)
        {
            case 1:
                if (!seedIsGet[1])
                {
                    return SeedType.StoneTree;
                }
                break;
            case 2:
                if (!seedIsGet[4])
                {
                    return SeedType.Blumen;
                }
                break;
            case 3:
                if (!seedIsGet[7])
                {
                    return SeedType.Diva;
                }
                break;

        }
        return SeedType.Linbum;
    }
    private SeedType StoneTreeVariation()
    {
        int num = Random.Range(0, 3);
        if (num == 1 && !seedIsGet[2])
        {
            return SeedType.ThornTree;
        }
        else if (num == 2 && !seedIsGet[3])
        {
            return SeedType.GuardianTree;
        }
        else
        {
            return SeedType.StoneTree;
        }
    }
    private SeedType BlumenVariation()
    {
        int num = Random.Range(0, 3);
        if (num == 1 && !seedIsGet[5])
        {
            return SeedType.Mac;
        }
        else if (num == 2 && !seedIsGet[6])
        {
            return SeedType.Saviour;
        }
        else
        {
            return SeedType.Blumen;
        }
    }
    private SeedType DivaVariation()
    {
        int num = Random.Range(0, 3);
        if (num == 1 && !seedIsGet[8])
        {
            return SeedType.Chomper;
        }
        else if (num == 2 && !seedIsGet[9])
        {
            return SeedType.Cibum;
        }
        else
        {
            return SeedType.Diva;
        }
    }
    #endregion
}


