using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�������ĸ����¼�
public class Operator : MonoBehaviour
{
    //�����������
    public GameObject generator;//�������ɽű����ڶ���
    private  ObjectGenerator generate;//�������ɵķ���
    private Vector3 pos;//����λ��
    private Quaternion dir;//���ɷ���
    // Start is called before the first frame update
    void Start()
    {
        generate = generator.GetComponent<ObjectGenerator>();
        GameState.Ins.gamestate = GAMESTATE.Proceed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.Ins.gamestate!=GAMESTATE.Proceed)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.A)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(0,0);
        } 
        if (Input.GetKeyDown(KeyCode.Q)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(1,0);
        }
        if (Input.GetKeyDown(KeyCode.S)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(0,1);
        } 
        if (Input.GetKeyDown(KeyCode.W)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(1,1);
        }
        if (Input.GetKeyDown(KeyCode.D)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(0,2);
        } 
        if (Input.GetKeyDown(KeyCode.E)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(1,2);
        }
        if (Input.GetKeyDown(KeyCode.F)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(0,3);
        } 
        if (Input.GetKeyDown(KeyCode.R)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(1,3);
        }
        if (Input.GetKeyDown(KeyCode.G)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(0,4);
        } 
        if (Input.GetKeyDown(KeyCode.T)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(1,4);
        }
        if (Input.GetKeyDown(KeyCode.H)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(0,5);
        } 
        if (Input.GetKeyDown(KeyCode.Y)&&GameState.Ins.gamestate==GAMESTATE.Proceed)
        {
            PreGenerator(1,5);
        }
    }

    //��ȡ����λ�á���ϵ�ǰ�İ����������ɱ��ֵķ���
    private void PreGenerator(int Camp,int Species)
    {
        //pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//���������תΪ��������
        // Debug.Log(Input.mousePosition);
        if (Camp==0)
        {
            pos = new Vector3(2, 2f, -10);
            dir = new Quaternion(0,0,0,0);
        }
        else
        {
            pos = new Vector3(0, 1f, 10);
            dir = Quaternion.Euler(new Vector3(0, -180, 0));
        }
        
        generate.Generate(Species, pos,dir);//���ɱ���
    }
}


