using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBase : Base
{
    protected override void Start()
    {
        base.Start();
        tag = "RedBase";
        hp = 100;

    }
    protected override void Rebounddamage(GameObject target)
    {
        base.Rebounddamage(target);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
