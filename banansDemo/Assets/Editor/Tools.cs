using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tools : Editor
{
    public static int witch = 11;
    public static int hight = 19;

    [MenuItem("Tools/生成地图")]
    public static void CreatMap()
    {
        var parent= GameObject.Find("Parent");
        // for (int i = 0; i < 22; i++)
        // {
        //     for (int j = 0; j<38; j++)
        //     {
        //         GameObject go= GameObject.CreatePrimitive(PrimitiveType.Cube);
        //         go.transform.parent = parent.transform;
        //         go.transform.position = new Vector3(i, 0, j);
        //     }
        // }
        //遍历地图格子，创建方块
        witch *= 1;
        hight *= 1;
        Vector3 center = new Vector3((witch - 1) / 2f, 0, (hight - 1) / 2f);

        //遍历地图格子，创建方块
        for (int x = 0; x < witch; x++)
        {
            for (int y = 0; y < hight; y++)
            {
                //计算方块位置
                Vector3 position = new Vector3(x, 0, y);

                //创建方块
                GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
                block.transform.position = position;
                block.transform.parent = parent.transform;
                //将方块放置到中心点
                if (position == center)
                {
                    block.transform.position = new Vector3(0, 0,0);
                }
                else
                {
                    block.transform.position -= center;
                }
            }
        }
    }

}
