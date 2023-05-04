using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//兵种放置相关
public class ObjectGenerator : MonoBehaviour
{
    public GameObject[] arm;//储存兵的数组

    //外部调用此方法生成兵.传入兵种编号和位置参数，在指定位置生成该兵种的一个兵,返回兵的对象
    public GameObject Generate(int index,Vector3 pos,Quaternion dir)
    {
       GameObject obj= Instantiate(arm[index],pos,dir);
        return obj;
    }
}
