using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectHurt : MonoBehaviour
{

    public string myCamp;//阵营
    private Action callback = null;
    private GameObject startPos, endPos;
    private int damage;//伤害值
    
    private void OnComplete()
    {
        if (callback != null)
        {
            callback.Invoke();
        }
    }
    /// <param name="centerOffset">中心点偏移量，0.5为基准，调整幅度0.01，根据实际调整</param>
    public void MoveTo(Vector3 startPos, Vector3 endPos, float centerOffset, float time = 0.5f, Action callback = null)
    {
        this.callback = callback;
        gameObject.transform.position = startPos;
        gameObject.SetActive(true);
        StartCoroutine(Move(startPos, endPos, centerOffset, time,OnComplete));
    }
 
    //如果有拖尾效果，延迟一定时间再显示，否则尾巴会有残影
    IEnumerator Move(Vector3 startPos, Vector3 endPos, float centerOffset, float time = 0.5f, Action onComplete = null)
    {
        TrailRenderer trail = gameObject.GetComponentInChildren<TrailRenderer>(true);
        yield return new WaitForSeconds(trail ? trail.time : 0);
        //如果路径点较少，可以使用CatmullRom曲线插值类型运动，默认PathType.Linear
        //同时可以设置easetype来使用不同的运动方式，其它easetype:https://blog.csdn.net/w0100746363/article/details/83587485
        //如果想让物体一直朝着路径方向可以加上SetLookAt(0):DOPath(path, time).SetLookAt(0);
        Tween tween = gameObject.transform.DOPath(Path(startPos, endPos, centerOffset, 6), time, PathType.CatmullRom).SetEase(Ease.Linear);
 
        if (onComplete != null)
        {
            tween.OnComplete(new TweenCallback(onComplete));
        }
        if (trail)
            trail.enabled = true;
    }
 
    private Vector3[] Path(Vector3 startTrans, Vector3 endTrans, float centerCtl, int segmentNum)
    {
        var startPoint = startTrans;
        var endPoint = endTrans;
        //中间点：起点和终点的向量相加再乘一个系数，系数可以根据实际调整
        var bezierControlPoint = (startPoint + endPoint) * centerCtl;
 
        //segmentNum为int类型，路径点数量，值越大，路径点越多，曲线越平滑
        Vector3[] path = new Vector3[segmentNum];
        for (int i = 0; i < segmentNum; i++)
        {
            var time = (i + 1) / (float)segmentNum;//归化到0~1范围
            path[i] = GetBezierPoint(time, startPoint, bezierControlPoint+new Vector3(0,3,0), endPoint);//使用贝塞尔曲线的公式取得t时的路径点
        }
        return path;
    }
 
    /// <param name="t">0到1的值，0获取曲线的起点，1获得曲线的终点</param>
    /// <param name="start">曲线的起始位置</param>
    /// <param name="center">决定曲线形状的控制点</param>
    /// <param name="end">曲线的终点</param>
    private Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
    {
        return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
    }

    //初始化调用接口
    public void Fire( GameObject target,int _DamageCoe,float time)
    {
        this.damage = _DamageCoe;
        MoveTo(transform.position, target.transform.position, 0.5f ,time, () =>
        {
            Destroy(gameObject,.3f);
        });
    }
    private void OnTriggerEnter(Collider other)//命中相关
    {
        
        if (!other.CompareTag(myCamp)&& !other.CompareTag("Bullet") )//碰撞的物体不是己方或子弹时
        {
            var @base =other.GetComponent<Base>();
            if (!other.CompareTag("BattleGround"))//如果对方不是战场，则对其造成伤害
            {
                if (@base!=null)
                {
                    @base.UpdateHP(damage);//对基地造成伤害
                    
                }
                else
                {
                    if (other.GetComponent<AI>())
                    {
                        other.GetComponent<AI>().UpdateHP(-damage); //造成伤害
                    }
                    else
                    {
                        other.GetComponent<StormAI>().UpdateHP(-damage); //造成伤害
                    }
                }
               

            }
            //if (father!=null)
            //{
            //    father.GetComponent<AI>().canFire = true;//告诉小兵可以进行下一次开火了
            //}
            
            // Destroy(gameObject,.3f);
            
            //Debug.Log(other);
            
        }
        
    }
    private void OnDestroy()
    {
        Debug.Log(1111);
    }
}



