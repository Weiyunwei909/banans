using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//���ڿ���С����Ϊ
public class AI : MonoBehaviour
{
    //private int faction;//С������Ӫ��ţ�Ŀǰֻ��0��1���ѷ��������ñ�ǩ����ʾ��Ӫ
    public int state;//��ǰ����״̬��-1������0վ����1Ѱ�У�2����
    private string targetTag;//Ŀ��ı�ǩ
    private NavMeshAgent agent;//Ѱ·���
    private float targetDistance;//��������ĵ��˵ľ���
    private GameObject target;//��������ĵ���,��ΪĿ��
    private float fireDistance;//���
    private float fireGap;//������������ֵԽС����Խ�졣���£���Ϊ���к��ٽ�����һ�ι���,����ͨ���ӵ���speed����
    private float fireDeltaTime;//��ʱ�����洢��һ�ο������˶�á����£���Ϊ���к��ٽ�����һ�ι���
    public float moveSpeed;
    public bool canFire;
    public bool isImmune;//�Ƿ����߿���
    public GameObject bullet;//�ӵ�Ԥ����
    private float ShootY;//�����ӵ���λ�õĸ߶�
    private int maxHP;
    private int curHP;
    private Attribute atb;
    public GameObject Bullet;//�����ӵ�
    /// <summary>
    /// ѣ����ز���
    /// </summary>
    private bool isDizzy;
    private float curDizzyTime;
    private float maxDizzyTime;
    private string BaseTarget;//Ҫ�����Ļ���
    private bool isAttackingBase;

    
    //public GameObject[] targets=new GameObject[100];//Ŀ�����
    //private float[] distances=new float[100];//��Ŀ��ľ���
    
    // Start is called before the first frame update
    void Start()
    {
        atb = GetComponent<Attribute>();
        fireDistance =atb.fireDistance;//��ʼ����̣�ÿ����ɫ��һ��
        //fireGap = atb.fireGap;//��ʼ���������ʱ��
        //fireGap = .5f;//��ʼ���������ʱ��
        state = 1;//��ʼΪѰ��״̬
        maxHP = atb.maxHP;
        curHP = maxHP;
        ShootY = atb.ShootY;
        canFire = true;
        //fireDeltaTime = .5f * fireGap;//�����ٶ�Ĭ���ǹ�������Ķ���֮һ
        //���ݳ�����Ϊ��Ӫ��ֵ,����ǩ
        if (transform.position.z>0)
        {
            //faction = 1;
            gameObject.tag = "Red";//�����Ǻ췽
            targetTag = "Blue";//��������
            BaseTarget = "BlueBase";
        }
        else
        {
            //faction = 0;
            gameObject.tag = "Blue";//����
            targetTag = "Red";
            BaseTarget = "RedBase";
        }
        //��ʼ��Ѱ·AI
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
            return;//����״̬����ִ���κ�
        }
        //״̬���л�
        if (state == 1 && target != null && targetDistance <= fireDistance )
        {
            //canFire = true;
            state = 2;//��������Ҳ�����ѣ��״̬���л�������״̬
        }
        if (state == 2 && (targetDistance > fireDistance||target==null))
        {
            state = 1;//������̻�Ŀ���ѱ��ݻ٣��л���Ѱ��״̬
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
            agent.isStopped = true;//ֹͣ�ƶ�
           // fireDeltaTime += Time.deltaTime;
            //Debug.Log("still here");
           // if (fireDeltaTime>=fireGap)//������������Ϳ���
            if (canFire)//���£��ӵ����к����canFireΪtrue
            {
                canFire = false;
                //fireDeltaTime = 0;
                OnFire(target);//����
               // Debug.Log(gameObject.name+canFire);

                
            }
            //�����ʱ�������ǻ��أ�������Ѱһ����û���µ�Ŀ����Թ���
            if (isAttackingBase)
            {
                TargetFinder();
            }
        }
        //�ӵ���ʧ�󣬷����ٴη����ӵ�
        if (Bullet==null)
        {
            
            canFire = true;
        }
        //����ѣ���¼�
        if (isDizzy)
        {
            curDizzyTime += Time.deltaTime;
            if (curDizzyTime>=maxDizzyTime)
            {
                isDizzy = false;
                Debug.Log("������");
            }
        }
        
    }


    //Ѱ�����
    private Vector3 TargetFinder()//��������ĵ��˶����λ�ã�ͬʱ����target
    {
        //targets= GameObject.FindGameObjectsWithTag(targetTag);
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);//Ѱ�Ҳ�����Ŀ�����
        float[] distances = new float[targets.Length];//�������Ŀ����Լ��ľ���
        if (targets.Length==0)//���û���ҵ�����
        {
            target = null;//���������Ϣ
            //Debug.Log("δ����Ŀ��");
            isAttackingBase = true;
            
            target = GameObject.FindGameObjectWithTag(BaseTarget);
            targetDistance = Vector3.Distance(transform.position, target.transform.position);
            return target.transform.position;//ԭ�ز���.���£����ػ��ص�Ŀ��
        }
        //����е��ˣ���ʼ�������
        targetDistance = Vector3.Distance(transform.position, targets[0].transform.position);//��ʼ�����룬һ�����δ��ֵ�����
        target = targets[0];//��ʼ��Ŀ�����һ�����δ��ֵ�����
        for (int i = 0; i < targets.Length; i++)//��������ÿ������ľ����Լ��ҳ���С���Ǹ�
        {
            distances[i] = Vector3.Distance(transform.position, targets[i].transform.position);
            if (distances[i]<targetDistance)
            {
                targetDistance = distances[i];
                target = targets[i];
            }
        }
        if (target!=null)//�������Ϊǰ����ʱ�����targetδ��ֵ����������ڿ�ɾ��if��ֱ��return��target��λ��
        {
            //Debug.Log(gameObject.tag + "����Ŀ��" + (targets.Length + 1));
            isAttackingBase = false;
            return target.transform.position;
        }
        else
        {
            //Debug.Log("δ����Ŀ��");
            isAttackingBase = true;
            target = GameObject.FindGameObjectWithTag(BaseTarget);
            targetDistance = Vector3.Distance(transform.position, target.transform.position);
            return target.transform.position;//ԭ�ز��������£����ػ��ص�Ŀ��
        }
 
    }

    //�������
    private void OnFire(GameObject target)
    {
        // Debug.Log("��"+targetTag+"����");
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
            //Debug.Log("���ݻ�");
            Destroy(this.gameObject);
        }
    }
    /// <summary>
    /// ѣ����أ�����ѣ�ε�ʱ��
    /// </summary>
    /// <param name="time"></param>
    public void Dizzy(int time)//
    {
        maxDizzyTime = time;
        curDizzyTime = 0;
        isDizzy = true;
        state = 0;
        agent.isStopped = true;
        Debug.Log("������");
    }
}
