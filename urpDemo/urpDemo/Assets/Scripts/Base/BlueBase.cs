using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBase : Base
{
    protected override void Start()
    {
        base.Start();
        tag = "BlueBase";
        hp = 100;
    }
    protected override void Rebounddamage(GameObject target)
    {
        base.Rebounddamage(target);
    }
    protected override void OnDie()
    {
        base.OnDie();
    }
}
