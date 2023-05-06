using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public string myCamp;//��Ӫ
    //public GameObject target;//Ŀ��.���£����ٲ���׷�ٵķ�ʽ
    public Vector3 targetPos;//Ŀ��λ��
    private Vector3 deltaPos;//����
    private float distance;//����
    private float speed;//�����ٶ�
    private Vector3 iniSpeed;//���ٶ�
    private int damage;//�˺�ֵ
    private Rigidbody rb;
    private Attribute atb;
    public bool isReady;//�ӵ��Ѳ������
    private float Offset;
    public GameObject father;//���ɴ��ӵ��ı�
    // Start is called before the first frame update
    void Start()
    {
        atb = GetComponent<Attribute>();
        gameObject.tag = "Bullet";
        damage = atb.damage;
        //gameObject.tag = tag;
        speed = atb.speed;
        Offset = atb.Offset;
        rb = GetComponent<Rigidbody>();//��ȡ�������
        //Debug.Log(transform.position);
        //�����ӵ���������,������룬�������Ϊˮƽ��λ����
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
        //targetPos = target.transform.position+new Vector3(0,.5f,0);//���£�targetPos��Ϊ�ɽű���AI����ֵ

        //Debug.Log(targetPos+tag+dirPos);
        // transform.position += dirPos.normalized * speed;���£���Ϊ�����߶���ֱ��
        
    }

    private void OnTriggerEnter(Collider other)//�������
    {
        if (other.tag!=myCamp&& other.tag != "Bullet")//��ײ�����岻�Ǽ������ӵ�ʱ
        {
            if (other.tag!="BattleGround")//����Է�����ս�������������˺�
            {
                //Debug.Log(other.tag);
                if (other.GetComponent<AI>())
                {
                    other.GetComponent<AI>().UpdateHP(-damage); //����˺�
                   // other.GetComponent<AI>().Dizzy(1); //���ѣ��

                }
                else
                {
                    other.GetComponent<StormAI>().UpdateHP(-damage); //����˺�
                }
                
                
            }
            //if (father!=null)
            //{
            //    father.GetComponent<AI>().canFire = true;//����С�����Խ�����һ�ο�����
            //}
            
            Destroy(gameObject,.3f);
            
            //Debug.Log(other);
            
        }
        
    }
    private void Shoot()
    {
        //������Ϊ�ӵ����㲢�ṩ���ٶ�
        //iniSpeed = deltaPos * speed;
       // iniSpeed.y = (2f * distance) / (Offset * speed);//����һ���ܼ򵥵������߼��㹫ʽ
        //Debug.Log(iniSpeed);
        //rb.velocity = iniSpeed;
        rb.velocity = new Vector3(0,-1,0);
    }
}
