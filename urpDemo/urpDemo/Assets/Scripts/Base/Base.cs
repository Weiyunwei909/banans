using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Base : MonoBehaviour
{
    protected int hp;
    
    protected int damage;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    public virtual void UpdateHP(int attack)
    {
        hp -= attack;
        if (hp<=0)
        {
            OnDie();
        }
    }
   protected virtual void Rebounddamage(GameObject target) //反弹伤害的重写
   {
       target.GetComponent<AI>().UpdateHP(damage);
   }
   protected virtual void OnDie()
   {
       GameState.Ins.SetDestoryTag(tag);
       GameState.Ins.gamestate = GAMESTATE.End;
   }
}
