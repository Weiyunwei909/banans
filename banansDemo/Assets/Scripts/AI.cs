using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//用于控制小兵行为
public class AI : MonoBehaviour
{
    //private int faction;//小兵的阵营编号，目前只有0和1。已废弃，采用标签来表示阵营
    public int state;//当前所处状态。-1死亡，0站立，1寻敌，2攻击
    private string targetTag;//目标的标签
    private NavMeshAgent agent;//寻路组件
    private float targetDistance;//储存最近的敌人的距离
    private GameObject target;//储存最近的敌人,作为目标
    private float fireDistance;//射程
    private float fireGap;//攻击间隔，这个值越小攻速越快。更新：改为命中后再进行下一次攻击,攻速通过子弹的speed调整
    private float fireDeltaTime;//计时器，存储上一次开火后过了多久。更新：改为命中后再进行下一次攻击
    public float moveSpeed;
    public bool canFire;
    public bool isImmune;//是否免疫控制
    public GameObject bullet;//子弹预制体
    private float ShootY;//生成子弹的位置的高度
    private int maxHP;
    private int curHP;
    private Attribute atb;
    public GameObject Bullet;//储存子弹
    /// <summary>
    /// 眩晕相关参数
    /// </summary>
    private bool isDizzy;
    private float curDizzyTime;
    private float maxDizzyTime;
    private string BaseTarget;//要攻击的基地
    private bool isAttackingBase;

    
    //public GameObject[] targets=new GameObject[100];//目标对象
    //private float[] distances=new float[100];//与目标的距离
    
    // Start is called before the first frame update
    void Start()
    {
        atb = GetComponent<Attribute>();
        fireDistance =atb.fireDistance;//初始化射程，每个角色不一致
        //fireGap = atb.fireGap;//初始化攻击间隔时间
        //fireGap = .5f;//初始化攻击间隔时间
        state = 1;//初始为寻敌状态
        maxHP = atb.maxHP;
        curHP = maxHP;
        ShootY = atb.ShootY;
        canFire = true;
        //fireDeltaTime = .5f * fireGap;//起手速度默认是攻击间隔的二分之一
        //根据出生点为阵营赋值,贴标签
        if (transform.position.z>0)
        {
            //faction = 1;
            gameObject.tag = "Red";//本身是红方
            targetTag = "Blue";//攻击蓝方
            BaseTarget = "BlueBase";
        }
        else
        {
            //faction = 0;
            gameObject.tag = "Blue";//蓝方
            targetTag = "Red";
            BaseTarget = "RedBase";
        }
        //初始化寻路AI
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.Ins.gamestate==GAMESTATE.End)
        {
            state = 0;
            return;
        }
        if (state==-1)
        {
            return;//死亡状态不再执行任何
        }
        //状态的切换
        if (state == 1 && target != null && targetDistance <= fireDistance )
        {
            //canFire = true;
            state = 2;//进入射程且不处于眩晕状态，切换到开火状态
        }
        if (state == 2 && (targetDistance > fireDistance||target==null))
        {
            state = 1;//超出射程或目标已被摧毁，切换到寻敌状态
        }
        if (state==0&& !isDizzy)
        {
            state = 1;
        }


        if (state==1)
        {
            agent.isStopped = false;
            agent.SetDestination(TargetFinder());
            
        }
        if (state==2)
        {
            //Debug.Log("here");
            agent.isStopped = true;//停止移动
           // fireDeltaTime += Time.deltaTime;
            //Debug.Log("still here");
           // if (fireDeltaTime>=fireGap)//攻击间隔结束就开火
            if (canFire)//更新：子弹命中后更改canFire为true
            {
                canFire = false;
                //fireDeltaTime = 0;
                OnFire(target);//开火
               // Debug.Log(gameObject.name+canFire);

                
            }
            //如果此时攻击的是基地，则还需搜寻一下有没有新的目标可以攻击
            if (isAttackingBase)
            {
                TargetFinder();
            }
        }
        //子弹消失后，方可再次发射子弹
        if (Bullet==null)
        {
            
            canFire = true;
        }
        //处理眩晕事件
        if (isDizzy)
        {
            curDizzyTime += Time.deltaTime;
            if (curDizzyTime>=maxDizzyTime)
            {
                isDizzy = false;
                Debug.Log("晕完了");
            }
        }
        
    }


    //寻敌相关
    private Vector3 TargetFinder()//返回最近的敌人对象的位置，同时更新target
    {
        //targets= GameObject.FindGameObjectsWithTag(targetTag);
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);//寻找并储存目标对象
        float[] distances = new float[targets.Length];//储存各个目标和自己的距离
        if (targets.Length==0)//如果没有找到敌人
        {
            target = null;//清除敌人信息
            //Debug.Log("未发现目标");
            isAttackingBase = true;
            
            target = GameObject.FindGameObjectWithTag(BaseTarget);
            targetDistance = Vector3.Distance(transform.position, target.transform.position);
            return target.transform.position;//原地不动.更新：返回基地的目标
        }
        //如果有敌人，开始计算距离
        targetDistance = Vector3.Distance(transform.position, targets[0].transform.position);//初始化距离，一面出现未赋值的情况
        target = targets[0];//初始化目标对象，一面出现未赋值的情况
        for (int i = 0; i < targets.Length; i++)//遍历计算每个对象的距离以及找出最小的那个
        {
            distances[i] = Vector3.Distance(transform.position, targets[i].transform.position);
            if (distances[i]<targetDistance)
            {
                targetDistance = distances[i];
                target = targets[i];
            }
        }
        if (target!=null)//这句是因为前期有时候出现target未赋值的情况，现在可删掉if，直接return回target的位置
        {
            //Debug.Log(gameObject.tag + "发现目标" + (targets.Length + 1));
            isAttackingBase = false;
            return target.transform.position;
        }
        else
        {
            //Debug.Log("未发现目标");
            isAttackingBase = true;
            target = GameObject.FindGameObjectWithTag(BaseTarget);
            targetDistance = Vector3.Distance(transform.position, target.transform.position);
            return target.transform.position;//原地不动。更新：返回基地的目标
        }
 
    }

    //开火相关
    private void OnFire(GameObject target)
    {
        // Debug.Log("对"+targetTag+"开火");
        Bullet= Instantiate(bullet,transform.position+new Vector3(0,ShootY,0),transform.rotation);
        
        Bullet.GetComponent<Bullet>().targetPos = target.transform.position;
        Bullet.GetComponent<Bullet>().myCamp = gameObject.tag;
        Bullet.GetComponent<Bullet>().father = gameObject;
        Bullet.GetComponent<Bullet>().isReady = true;
        
    }
    public void UpdateHP(int change)
    {
        curHP = Mathf.Clamp(curHP+change, 0, maxHP);
        //Debug.Log(change);
        Debug.Log(curHP);
        if (curHP==0)
        {
            //Debug.Log("被摧毁");
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// 眩晕相关，传入眩晕的时间
    /// </summary>
    /// <param name="time"></param>
    public void Dizzy(int time)//
    {
        maxDizzyTime = time;
        curDizzyTime = 0;
        isDizzy = true;
        state = 0;
        agent.isStopped = true;
        Debug.Log("我晕了");
    }
}
