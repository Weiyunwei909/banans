using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public string myCamp;//阵营
    //public GameObject target;//目标.更新：不再采用追踪的方式
    public Vector3 targetPos;//目标位置
    private Vector3 deltaPos;//方向
    private float distance;//距离
    private float speed;//飞行速度
    private Vector3 iniSpeed;//初速度
    private int damage;//伤害值
    private Rigidbody rb;
    private Attribute atb;
    public bool isReady;//子弹已部署完毕
    private float Offset;
    public GameObject father;//生成此子弹的兵
    // Start is called before the first frame update
    void Start()
    {
        atb = GetComponent<Attribute>();
        gameObject.tag = "Bullet";
        damage = atb.damage;
        //gameObject.tag = tag;
        speed = atb.speed;
        Offset = atb.Offset;
        rb = GetComponent<Rigidbody>();//获取刚体组件
        //Debug.Log(transform.position);
        //计算子弹方向向量,计算距离，并将其变为水平单位向量
        deltaPos = targetPos - transform.position;
        deltaPos.y = 0;
        distance = Vector3.Magnitude(deltaPos);
        Vector3.Normalize(deltaPos);


    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            Shoot();
            isReady = false;
        }
        //targetPos = target.transform.position+new Vector3(0,.5f,0);//更新：targetPos改为由脚本“AI”赋值

        //Debug.Log(targetPos+tag+dirPos);
        // transform.position += dirPos.normalized * speed;更新：改为抛物线而非直线
        
    }

    private void OnTriggerEnter(Collider other)//命中相关
    {
        if (other.tag!=myCamp&& other.tag != "Bullet")//碰撞的物体不是己方或子弹时
        {
            if (other.tag!="BattleGround")//如果对方不是战场，则对其造成伤害
            {
                //Debug.Log(other.tag);
                if (other.GetComponent<AI>())
                {
                    other.GetComponent<AI>().UpdateHP(-damage); //造成伤害
                   // other.GetComponent<AI>().Dizzy(1); //造成眩晕

                }
                else
                {
                    other.GetComponent<StormAI>().UpdateHP(-damage); //造成伤害
                }
                
                
            }
            //if (father!=null)
            //{
            //    father.GetComponent<AI>().canFire = true;//告诉小兵可以进行下一次开火了
            //}
            
            Destroy(gameObject,.3f);
            
            //Debug.Log(other);
            
        }
        
    }
    private void Shoot()
    {
        //以下是为子弹计算并提供初速度
        //iniSpeed = deltaPos * speed;
       // iniSpeed.y = (2f * distance) / (Offset * speed);//这是一个很简单的抛物线计算公式
        //Debug.Log(iniSpeed);
        //rb.velocity = iniSpeed;
        rb.velocity = new Vector3(0,-1,0);
    }
}
