using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ַ������
public class ObjectGenerator : MonoBehaviour
{
    public GameObject[] arm;//�����������

    //�ⲿ���ô˷������ɱ�.������ֱ�ź�λ�ò�������ָ��λ�����ɸñ��ֵ�һ����,���ر��Ķ���
    public GameObject Generate(int index,Vector3 pos,Quaternion dir)
    {
       GameObject obj= Instantiate(arm[index],pos,dir);
        return obj;
    }
}
