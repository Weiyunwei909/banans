using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : Singleton<GameState>
{
   public GAMESTATE gamestate = GAMESTATE.Ready;
   public string destoryTag;

   public void SetDestoryTag(string str)
   {
      destoryTag = str;
   }
}
public enum GAMESTATE
{
   Ready=-1,Start=0,Proceed=1, End=2

}